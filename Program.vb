Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports SixLabors.ImageSharp

Module Program

    Public CFG As Config

    Public DIR_CONFIG As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppName)
    Private PATH_CONFIG As String = Path.Combine(DIR_CONFIG, ".\config.json")

    Sub Main(args As String())
        ' Registers app path to enable direct calls
        Registry.UpdateAppPath()

        ' Catches context calls, parses arguments, sets global loglevel
        ParseArgs(args)

        ' Hook exit event, saves current config and disposes logger
        AddHandler AppDomain.CurrentDomain.ProcessExit, AddressOf ProcessExit

        ' First call of Log() initializes the logger
        Log(LogLvl.Info, "Jiraisho v0.1")
        Log(LogLvl.Debug, $"IsHardwareAccelerated = {Numerics.Vector.IsHardwareAccelerated}")

        ' Load Config from json
        LoadConfig()

        ' Create directorys set in config
        CreateDirectorys()

        ' Manages downloads of new wallpapers
        Downloader = New DownloadClient

        ' Check internet connection
        CheckInternetConnection().GetAwaiter().GetResult()

        ' Manages monitors, desktops and wallpapers
        Desktop = New DesktopClient

        ' Handles the actual work, downloads new image, sets it as background
        StartProcessingLoop()

        ' Start user interface (tray icon, hotkeys, settings)
        StartUI() ' Blocks until UI closes
    End Sub


    Private Sub ParseArgs(args As String())
#Region "Context Menu Catch"
        If args IsNot Nothing AndAlso args.Length > 0 AndAlso args(0).Contains("cmt") Then
            Try
                FileLogDisabled = True

                'Get current screen
                Dim currScreen = Screen.FromPoint(Cursor.Position)

                Select Case args(1)
                    Case "fav"
                    'ToDo: implement

                    Case "save"
                        Dim dirSaved = Registry.GetValue("DirSaved")
                        If dirSaved Is Nothing Then
                            Log(LogLvl.Error, "Can't parse directory for saved images")
                            Exit Select
                        End If
                        Dim path = Registry.GetValue(currScreen.DeviceName & "-filePath")
                        If path IsNot Nothing Then
                            If IO.File.Exists(path) Then
                                Try
                                    File.Copy(path, IO.Path.Combine(dirSaved, IO.Path.GetFileName(path)), True)
                                Catch ex As Exception
                                    Log(LogLvl.Error, "Can't copy file", ex)
                                End Try
                            Else
                                Log(LogLvl.Error, "No file found at " & path)
                            End If
                        Else
                            Log(LogLvl.Error, "No path found for " & currScreen.DeviceName)
                        End If

                    Case "open"
                        Dim url = Registry.GetValue(currScreen.DeviceName & "-postUrl")
                        If String.IsNullOrWhiteSpace(url) Then url = Registry.GetValue(currScreen.DeviceName & "-fileUrl")
                        If String.IsNullOrWhiteSpace(url) Then
                            Log(LogLvl.Error, "Can't get url for " & currScreen.DeviceName)
                            Exit Select
                        End If
                        Try
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.AppStarting
                            System.Diagnostics.Process.Start("explorer.exe", url)
                        Catch ex As Exception
                            Log(LogLvl.Error, "Can't open browser", ex)
                        Finally
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                        End Try

                    Case Else
                        Log(LogLvl.Error, "Can't process " & args(1))

                End Select
            Catch ex As Exception
                MessageBox.Show(ex.ToString(), AppName & " - Error")
            Finally
                'Always exit on context menu calls
                Environment.Exit(0)
            End Try
        End If
#End Region

        'Ensure to exit if the app is already running
        If System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1 Then
            MessageBox.Show("App is already running!", AppName)
            Environment.Exit(0)
        End If

#If DEBUG Then
        'Always log everything while debugging
        GlobalLogLevel = LogLvl.Trace
#Else
        If args IsNot Nothing AndAlso args.Length > 0 Then
            'First arg = loglevel, enables logging
            If Not String.IsNullOrWhiteSpace(args(0)) Then
                Select Case args(0).Trim().ToLowerInvariant()
                    Case "0", "trace"
                        GlobalLogLevel = LogLvl.Trace

                    Case "1", "debug"
                        GlobalLogLevel = LogLvl.Debug

                    Case "2", "info", "information", "enabled", "log", "true"
                        GlobalLogLevel = LogLvl.Info

                    Case "3", "warning"
                        GlobalLogLevel = LogLvl.Warning

                    Case "4", "error"
                        GlobalLogLevel = LogLvl.Error

                    Case "5", "critical"
                        GlobalLogLevel = LogLvl.Critical

                    Case Else
                        'Will also disable all message boxes
                        GlobalLogLevel = 6
                        FileLogDisabled = True

                End Select
            End If
        Else
            'Disable file-logging by default
            GlobalLogLevel = LogLvl.Error
            FileLogDisabled = True
        End If
#End If
    End Sub

    Private Sub ProcessExit(sender As Object, e As EventArgs)
        Dim json = JsonConvert.SerializeObject(CFG, Formatting.Indented)
        Try
            Dim fi = New System.IO.FileInfo(PATH_CONFIG)
            fi.Directory.Create()
            File.WriteAllText(PATH_CONFIG, json, New UTF8Encoding(False))
        Catch ex As Exception
            Log(LogLvl.Warning, "Could not save config", ex)
        End Try

        DisposeLogger()
    End Sub

    Private Sub LoadConfig()
        Log(LogLvl.Trace, "Called")

        Dim defaultConfig = New Config With {
            .Source = "Konachan",
            .Rating = Rating.Safe,
            .IntervalInSeconds = 30,
            .Style = Style.Fit,
            .SkipObscuredMonitors = True,
            .DirHistory = Path.Combine(Path.GetTempPath(), AppName, "History"),
            .DirSaved = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), AppName),
            .MaxHistory = 10,
            .HK_SaveCurrent = (HK_Modifier.MOD_ALT, Keys.S),
            .HK_OpenCurrent = (HK_Modifier.MOD_ALT, Keys.O),
            .ContextMenu = ContextMenuType.Cascaded,
            .SettingsWindowDefaultPosition = New Drawing.Point(256, 256) 'ToDo: Store this in registry
        }

        If Not File.Exists(PATH_CONFIG) Then
            CFG = defaultConfig
            Dim json = JsonConvert.SerializeObject(CFG, Formatting.Indented)
            Try
                Dim fi = New System.IO.FileInfo(PATH_CONFIG)
                fi.Directory.Create()
                File.WriteAllText(PATH_CONFIG, json, New UTF8Encoding(False))
            Catch ex As Exception
                Log(LogLvl.Warning, "Can't write config file", ex)
            End Try
        Else
            Try
                Dim json = File.ReadAllText(PATH_CONFIG, New UTF8Encoding(False))
                CFG = JsonConvert.DeserializeObject(Of Config)(json)
            Catch ex As Exception
                CFG = defaultConfig
                Dim json = JsonConvert.SerializeObject(CFG, Formatting.Indented)
                Dim fi = New System.IO.FileInfo(PATH_CONFIG)
                fi.Directory.Create()
                File.WriteAllText(PATH_CONFIG, json, New UTF8Encoding(False))
                Log(LogLvl.Error, "Can't load config. Config file reset to defaults.", ex)
            End Try
        End If

        'Check values
        If Not DownloadClient.Sources.Contains(CFG.Source) Then
            CFG.Source = defaultConfig.Source
        End If
        If CFG.Rating < 0 OrElse CFG.Rating > Rating.All Then
            CFG.Rating = defaultConfig.Rating
        End If
        If CFG.MinResolution < 0! OrElse CFG.MinResolution > 1.0! Then
            CFG.MinResolution = defaultConfig.MinResolution
        End If
        If CFG.IntervalInSeconds < 5 Then
            CFG.IntervalInSeconds = 5
        End If
        If CFG.Style < 0 OrElse CFG.Style > 5 Then
            CFG.Style = defaultConfig.Style
        End If
        If String.IsNullOrWhiteSpace(CFG.DirHistory) Then
            CFG.DirHistory = defaultConfig.DirHistory
        End If
        If String.IsNullOrWhiteSpace(CFG.DirSaved) Then
            CFG.DirSaved = defaultConfig.DirSaved
        End If
        'Max history get's checked in the DesktopClient constructor
        Dim modi = CFG.HK_SaveCurrent.Item1
        Dim key = CFG.HK_SaveCurrent.Item2
        If modi < HK_Modifier.MOD_ALT OrElse modi > HK_Modifier.MOD_CONTROL_SHIFT OrElse modi = HK_Modifier.MOD_WIN OrElse modi = HK_Modifier.MOD_NOREPEAT Then
            CFG.HK_SaveCurrent = defaultConfig.HK_SaveCurrent
        End If
        modi = CFG.HK_OpenCurrent.Item1
        key = CFG.HK_OpenCurrent.Item2
        If modi < HK_Modifier.MOD_ALT OrElse modi > HK_Modifier.MOD_CONTROL_SHIFT OrElse modi = HK_Modifier.MOD_WIN OrElse modi = HK_Modifier.MOD_NOREPEAT Then
            CFG.HK_OpenCurrent = defaultConfig.HK_OpenCurrent
        End If
        If CFG.ContextMenu < ContextMenuType.None OrElse CFG.ContextMenu > ContextMenuType.Cascaded Then
            CFG.ContextMenu = defaultConfig.ContextMenu
        End If
        If CFG.SettingsWindowDefaultPosition.X < 0 OrElse CFG.SettingsWindowDefaultPosition.Y < 0 Then
            CFG.SettingsWindowDefaultPosition = defaultConfig.SettingsWindowDefaultPosition
        End If

        'Update registry
        Registry.SetValue("DirSaved", CFG.DirSaved)
        Select Case CFG.ContextMenu
            Case ContextMenuType.None
                Registry.DeleteContextMenu()
                Registry.DeleteCascadedContextMenu()

            Case ContextMenuType.Normal
                Registry.DeleteCascadedContextMenu()
                Registry.CreateContextMenu()

            Case ContextMenuType.Cascaded
                Registry.DeleteContextMenu()
                Registry.CreateCascadedContextMenu()

        End Select


        Log(LogLvl.Trace, "Reached end")
    End Sub

    Private Sub CreateDirectorys()
        Log(LogLvl.Trace, "Called")

        Try
            IO.Directory.CreateDirectory(CFG.DirHistory)
        Catch ex As Exception
            Log(LogLvl.Critical, "Can't create history directory.", ex)
        End Try

        Try
            IO.Directory.CreateDirectory(CFG.DirSaved)
        Catch ex As Exception
            Log(LogLvl.Critical, "Can't create saved directory.", ex)
        End Try

        Log(LogLvl.Trace, "Reached end")
    End Sub

    Private Async Function CheckInternetConnection() As Task
        Log(LogLvl.Trace, "Called")

        Dim failCount As Integer
        Do While Not Await Downloader.CheckInternetConnection()
            failCount += 1
            If failCount > 3 Then
                Log(LogLvl.Critical, $"Can't reach the internet.{vbCrLf}Can't download wallpapers without access to the internet.")
            End If
            Await Task.Delay(5000)
        Loop

        Log(LogLvl.Trace, "Reached end")
    End Function

    Public Sub StartProcessingLoop()
        Log(LogLvl.Trace, "Called")

        'Run once at start to get first image
        ProcessingLoop.ProcessingLoop(Nothing, Nothing)

        SlideshowTimer = New Timers.Timer
        AddHandler SlideshowTimer.Elapsed, AddressOf ProcessingLoop.ProcessingLoop
        SlideshowTimer.Interval = 5000 'Set interval gets set after first wallpaper change
        SlideshowTimer.AutoReset = True
        SlideshowTimer.Start()

        Log(LogLvl.Debug, $"Diashow timer started")
    End Sub

    Sub StartUI()
        Log(LogLvl.Trace, "Called")

        'Run UI on a seperate thread, because it needs a STAThread
        Dim UIThread = New Thread(Sub() Application.Run(New UserInterface))
        UIThread.SetApartmentState(ApartmentState.STA)
        UIThread.Start()
        UIThread.Join() 'Blocks until Application closes
    End Sub


    Public Class Config
        'General
        <JsonProperty(Order:=0)> Public StartWithWindows As Boolean
        <JsonProperty(Order:=1)> Public CheckForUpdates As Boolean

        'Source
        <JsonProperty(Order:=2)> Public Source As String
        <JsonProperty(Order:=3)> Public Username As String
        <JsonProperty(Order:=4)> Friend Password As String 'ToDo: Encrypt?

        'Search
        <JsonProperty(Order:=5)> Public Rating As Rating
        <JsonProperty(Order:=6)> Public CustomTags As String()
        <JsonProperty(Order:=7)> Public OnlyDesktopRatio As Boolean
        <JsonProperty(Order:=8)> Public AllowSmallDeviations As Boolean
        <JsonProperty(Order:=9)> Public MinResolution As Single

        'Wallpaper
        <JsonProperty(Order:=10)> Public IntervalInSeconds As Integer
        <JsonProperty(Order:=11)> Public Style As Style
        <JsonProperty(Order:=12)> Public SkipObscuredMonitors As Boolean

        'Files
        <JsonProperty(Order:=13)> Public DirHistory As String
        <JsonProperty(Order:=14)> Public DirSaved As String
        <JsonProperty(Order:=15)> Public MaxHistory As Integer

        'Hotkeys
        <JsonProperty(Order:=16)> Public HK_SaveCurrent As (HK_Modifier, Keys)
        <JsonProperty(Order:=17)> Public HK_OpenCurrent As (HK_Modifier, Keys)

        'ContextMenu
        <JsonProperty(Order:=18)> Public ContextMenu As ContextMenuType

        'Misc
        <JsonProperty(Order:=19)> Public SettingsWindowDefaultPosition As Drawing.Point 'ToDo: Save this in registry?

        Public Function Clone() As Config
            Return Me.MemberwiseClone()
        End Function
    End Class

End Module

