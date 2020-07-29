Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports SixLabors.ImageSharp

Module Program

    Sub Main(args As String()) 'Entry point

        ' Handles context menu calls and exits then
        CatchContextMenuCall(args)

        ' Don't allow concurrent instances of the app
        ExitIfAppIsAlreadyRunning()

        ' Sets global loglevel, parsed from arguments
        SetLogLevel(args)

        ' Hook exit event, saves current config and disposes logger
        AddHandler AppDomain.CurrentDomain.ProcessExit, AddressOf ProcessExit

        ' First call of Log() initializes the logger
        Log(LogLvl.Info, "Jiraisho v0.1")
        Log(LogLvl.Debug, $"IsHardwareAccelerated = {Numerics.Vector.IsHardwareAccelerated}")

        ' Registers/Updates app path to enable direct calls via jiraisho.exe
        Registry.UpdateAppPath()

        ' Load Config from json
        LoadConfig()

        ' Create directorys set in config
        CreateDirectorys()

        ' Init downloader, manages downloads of new wallpapers
        Downloader = New DownloadClient

        ' Check internet connection
        CheckInternetConnection().GetAwaiter().GetResult()

        ' Init desktop, manages monitors, desktops and wallpapers
        Desktop = New DesktopClient

        ' Image manipuation setting
        InitImageSharp()

        ' Handles the actual work, downloads new image, sets it as background
        StartProcessingLoop()

        ' Start user interface (tray icon, hotkeys, settings)
        StartUI() ' Blocks until UI closes

    End Sub


    Private Sub CatchContextMenuCall(args As String())
        If args IsNot Nothing AndAlso args.Length > 0 AndAlso args(0).Contains("cmt") Then
            'App was called from context menu
            Try
                'Can't access the same logfile from two applications
                FileLogDisabled = True

                'Get selected screen
                Dim selectedScreen = Screen.FromPoint(Cursor.Position)

                Select Case args(1)
                    Case "fav"
                        UserActions.FavWallpaper(selectedScreen)

                    Case "save"
                        UserActions.SaveWallpaper(selectedScreen)

                    Case "open"
                        UserActions.OpenWallpaper(selectedScreen)

                    Case "favlast"
                        UserActions.FavWallpaper(selectedScreen, last:=True)

                    Case "savelast"
                        UserActions.SaveWallpaper(selectedScreen, last:=True)

                    Case "openlast"
                        UserActions.OpenWallpaper(selectedScreen, last:=True)

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
    End Sub

    Private Sub ExitIfAppIsAlreadyRunning()
        If System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1 Then
            MessageBox.Show("App is already running!", AppName)
            Environment.Exit(0)
        End If
    End Sub

    Private Sub SetLogLevel(args As String())
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

#Region "Config"
    Public CFG As Config
    Public DIR_CONFIG As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppName)
    Private PATH_CONFIG As String = Path.Combine(DIR_CONFIG, ".\config.json")

    Private Sub LoadConfig()
        Log(LogLvl.Trace, "Called")

        Dim defaultConfig = New Config With {
            .Source = "Konachan",
            .Rating = Rating.Safe,
            .IntervalInSeconds = 30,
            .SkipObscuredMonitors = True,
            .DirHistory = Path.Combine(Path.GetTempPath(), AppName, "History"),
            .DirSaved = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), AppName),
            .MaxHistory = 10,
            .HK_SaveCurrent = (HK_Modifier.MOD_ALT, Keys.S),
            .HK_OpenCurrent = (HK_Modifier.MOD_ALT, Keys.O),
            .HK_FavCurrent = (HK_Modifier.MOD_ALT, Keys.F),
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
#End Region

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
            Await Task.Delay(4000)
        Loop

        Log(LogLvl.Trace, "Reached end")
    End Function

    Private Sub InitImageSharp()
        'ToDo: Configurable?
        Configuration.Default.MaxDegreeOfParallelism = Environment.ProcessorCount / 2
        'ToDo: Performance testing: memory vs cpu, -> Make configurable
        Configuration.Default.MemoryAllocator = SixLabors.ImageSharp.Memory.ArrayPoolMemoryAllocator.CreateWithModeratePooling()
    End Sub

    Private Sub StartProcessingLoop()
        Log(LogLvl.Trace, "Called")

        'Ensure that there is a wallpaper for every monitor already
        GetNextWallpaperForAllMonitors()

        SlideshowTimer = New Timers.Timer
        AddHandler SlideshowTimer.Elapsed, AddressOf ProcessingLoop.ProcessingLoop
        SlideshowTimer.Interval = 3000 'Set interval gets set after first wallpaper change
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
        <JsonProperty(Order:=11)> Public Style As SortedList(Of Integer, CustomStyle)
        <JsonProperty(Order:=12)> Public SkipObscuredMonitors As Boolean

        'Files
        <JsonProperty(Order:=13)> Public DirHistory As String
        <JsonProperty(Order:=14)> Public DirSaved As String
        <JsonProperty(Order:=15)> Public MaxHistory As Integer

        'Hotkeys
        <JsonProperty(Order:=16)> Public HK_SaveCurrent As (HK_Modifier, Keys)
        <JsonProperty(Order:=17)> Public HK_OpenCurrent As (HK_Modifier, Keys)
        <JsonProperty(Order:=18)> Public HK_FavCurrent As (HK_Modifier, Keys)

        'ContextMenu
        <JsonProperty(Order:=19)> Public ContextMenu As ContextMenuType

        'Misc
        <JsonProperty(Order:=20)> Public SettingsWindowDefaultPosition As Drawing.Point 'ToDo: Save this in registry?

        Public Function Clone() As Config
            Return Me.MemberwiseClone()
        End Function
    End Class

End Module

