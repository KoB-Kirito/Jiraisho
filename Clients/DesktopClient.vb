Imports System.Drawing
Imports System.Windows.Forms
Imports Jiraisho.NativeMethod
Imports Newtonsoft.Json

Public Class DesktopClient
    Public ReadOnly Property Monitors As SortedDictionary(Of Integer, Monitor)

    'PInvoke
    Private Shared _desktopWallpaper As IDesktopWallpaper = New DesktopWallpaperClass()
    Private Shared _desktopWallpaperLock As New Object

    Public Sub New()

        Monitors = New SortedDictionary(Of Integer, Monitor)

        'Get correlation of DeviceName > DeviceId
        Log(LogLvl.Debug, "Enumerating display devices", Source:="New DesktopClient")
        Dim currMonitor As New DISPLAY_DEVICE
        currMonitor.cb = Runtime.InteropServices.Marshal.SizeOf(currMonitor)
        Dim i = 0
        While User32.EnumDisplayDevices(vbNullString, i, currMonitor, &H0)
            i += 1
            Log(LogLvl.Debug, currMonitor.ToString(), Source:="New DesktopClient")

            If currMonitor.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop) Then
                'Get id
                Dim id As Integer
                If Not Integer.TryParse(currMonitor.DeviceName.Substring(11), id) Then
                    Log(LogLvl.Warning, "Was not able to get the id from " & currMonitor.DeviceName & $" ({currMonitor.DeviceName.Substring(11)})", Source:="New DesktopClient")
                    Continue While
                End If

                'DEBUG, maybe some of this may be useful?
                Log(LogLvl.Debug, $"Unused device info: cb = {currMonitor.cb}, key = {currMonitor.DeviceKey}, string = {currMonitor.DeviceString}")

                'Get DeviceName
                Dim newMonitor = New Monitor(id, currMonitor.DeviceName)

                'Get DeviceId
                User32.EnumDisplayDevices(currMonitor.DeviceName, 0, currMonitor, &H1)
                newMonitor.DeviceId = currMonitor.DeviceID

                ' Get resolution
                Dim rect As User32.RECT = Nothing
                Dim hr = _desktopWallpaper.GetMonitorRECT(currMonitor.DeviceID, rect)
                If hr <> HRESULT.S_OK Then
                    Log(LogLvl.Warning, $"Can't get resolution of monitor {newMonitor.DeviceName}. Result was {hr.ToString()}", Source:="New DesktopClient")
                    Continue While
                End If
                newMonitor.Rectangle = User32.ToRectangle(rect)

                Monitors.Add(id, newMonitor)
                Registry.UpdateMonitor(newMonitor)
                Log(LogLvl.Debug, "New monitor added: " & newMonitor.ToString(), Source:="New DesktopClient")
                'Log(LogLvl.Debug, currMonitor.ToString())
            End If
        End While
        Log(LogLvl.Debug, "Interfaces: " & i & " Connected monitors: " & Monitors.Count, Source:="New DesktopClient")


        If Monitors.Count = 0 Then
            Log(LogLvl.Critical, "No monitor detected!", Source:="New DesktopClient")
        End If

        'Check config
        'Style
        If CFG.Style Is Nothing Then CFG.Style = New SortedList(Of Integer, CustomStyle)
        For Each monitor In Monitors
            If Not CFG.Style.ContainsKey(monitor.Key) Then
                CFG.Style.Add(monitor.Key, CustomStyle.FitRight) 'ToDo: change default
            Else
                'ToDo: Check if value is in range
            End If
        Next
        'History
        If CFG.MaxHistory < Monitors.Count Then
            CFG.MaxHistory = Monitors.Count
            Log(LogLvl.Warning, $"MaxHistory set to {Monitors.Count}, because there are that many monitors.", Source:="New DesktopClient")
        End If

        Log(LogLvl.Info, $"{Monitors.Count} monitors found", Source:="New DesktopClient")

        'Set style to center = No processing on windows side
        SetGlobalStyle(Style.Center)
    End Sub


    Public Sub SetWallpaper(MonitorId As Integer, FilePath As String)
        SetWallpaper(Desktop.Monitors(MonitorId).DeviceId, FilePath)
    End Sub

    Public Sub SetWallpaper(DeviceId As String, FilePath As String)
        Dim hr As HRESULT
        SyncLock _desktopWallpaperLock
            Try
                hr = _desktopWallpaper.SetWallpaper(DeviceId, FilePath)
            Catch ex As Exception
                Log(LogLvl.Warning, "SetWallpaper failed", ex)
                hr = HRESULT.E_FAIL
            End Try
        End SyncLock
        If hr <> HRESULT.S_OK Then
            Log(LogLvl.Error, "SetWallpaper failed. Result = " & hr.ToString())
        End If
    End Sub

    Public Sub SetGlobalStyle(style As Style)
        SyncLock _desktopWallpaperLock
            _desktopWallpaper.SetPosition(style)
        End SyncLock
    End Sub

    Public Function IsDesktopVisible(MonitorId As Integer) As Boolean
        Return IsDesktopVisible(Me.Monitors(MonitorId).DeviceName)
    End Function

    Public Function IsDesktopVisible(DeviceName As String) As Boolean
        Log(LogLvl.Trace, "Called with " & DeviceName)

        'If feature disabled
        If CFG.SkipObscuredMonitors = False Then
            Log(LogLvl.Debug, "Feature is disabled")
            Return True
        End If

        'Iterate all windows
        'Log(LogLvl.Debug, "Iterating all windows...")

        'Exlude workspace windows
        Dim shellWindow = User32.GetShellWindow()
        Dim desktopWindow = User32.GetDesktopWindow()

        'Get screen
        Dim sScreen As Screen = Nothing
        For Each screen In System.Windows.Forms.Screen.AllScreens
            If screen.DeviceName = DeviceName Then
                sScreen = screen
                Log(LogLvl.Info, $"Screen: {sScreen.DeviceName} ({sScreen.Bounds.ToString()})[WorkingArea:{sScreen.WorkingArea.ToString()}]")
                Exit For
            End If
        Next
        If sScreen Is Nothing Then
            Log(LogLvl.Warning, "Screen is nothing")
            'Failback
            Return True
        End If

        Dim workingArea = sScreen.WorkingArea

        'Store found windows
        Dim windowRects = New List(Of Rectangle)()

        Dim filter As User32.EnumWindowsDelegate = Function(ByVal hWnd, ByRef lParam)
#If DEBUG Then
                                                       ''Debug: Print every window
                                                       'Dim [class], title As New System.Text.StringBuilder(512)
                                                       'If User32.GetClassName(hWnd, [class], 512) AndAlso User32.GetWindowText(hWnd, title, 512) Then
                                                       '    Log(LogLvl.Trace, $"{hWnd.ToString()} : [{[class].ToString()}] {title.ToString()}")
                                                       'Else
                                                       '    Log(LogLvl.Warning, "Can't get title or class for: " & hWnd.ToString())
                                                       'End If
#End If

                                                       'Skip
                                                       If hWnd = shellWindow OrElse hWnd = desktopWindow OrElse Not User32.IsWindowVisible(hWnd) Then Return True

                                                       'Experimental additional checks
                                                       Dim style As WindowStyles = User32.GetWindowLong(hWnd, User32.GWL_STYLE)
                                                       If style.HasFlag(WindowStyles.WS_POPUP) Then Return True 'Popup-windows seem to be invisible most of the time or not important
                                                       Dim flagString = String.Join(" : ", style.GetFlags)

                                                       Dim rect As Rectangle
                                                       If User32.GetWindowRect(hWnd, rect) Then
                                                           'Debug
                                                           Dim [class], title As New System.Text.StringBuilder(512)
                                                           User32.GetClassName(hWnd, [class], 512)
                                                           User32.GetWindowText(hWnd, title, 512)

                                                           'Only add windows that intersect with desktop
                                                           If rect.IntersectsWith(workingArea) Then
                                                               Log(LogLvl.Info, $"{hWnd.ToString(),8} ({rect.Width * rect.Height,8}) {rect.ToString(),-40} [{[class].ToString()}] {title.ToString()} {flagString}")
                                                               windowRects.Add(rect)
                                                           ElseIf rect.Width > 0 OrElse rect.Height > 0 Then
                                                               'Debug
                                                               'Log(LogLvl.Trace, $"{hWnd.ToString(),8} ({rect.Width * rect.Height}) {rect.ToString()} [{[class].ToString()}] {title.ToString()} {flagString}")
                                                           End If
                                                       End If

                                                       Return True
                                                   End Function

        If User32.EnumWindows(filter, IntPtr.Zero) Then
            If windowRects.Count = 0 Then Return True

            'Get rectangle of monitor
            Log(LogLvl.Debug, $"{windowRects.Count} windows intersect with {sScreen.DeviceName} {workingArea.ToString()}")

            'subtract all windows from the desktop
            Dim remaining = workingArea.Subtract(windowRects)

            If GlobalLogLevel <= LogLvl.Debug Then Utils.SaveScreenshotWithRectangles(DIR_CONFIG & "\remaining.bmp", windowRects, remaining)

            'Get accumulated volume of the remaining area
            Dim remainingV As Integer
            For Each rect In remaining
                Dim intersection = Rectangle.Intersect(workingArea, rect)
                Log(LogLvl.Trace, $"intersection={intersection.ToString()}")
                Log(LogLvl.Debug, remainingV)
                remainingV += intersection.Width * intersection.Height
                Log(LogLvl.Debug, remainingV)
            Next

            'Get volume of the working area
            Dim workingAreaV = workingArea.Width * workingArea.Height
            Dim remainingPercent = remainingV / workingAreaV

            Log(LogLvl.Debug, $"{Math.Round(remainingPercent, 2)} = {remainingV} / {workingAreaV}")


            If remainingPercent < 0.1 Then
                Log(LogLvl.Warning, "remainingPercent: " & Math.Round(remainingPercent * 100, 0) & "%")
                Return False
            Else
                Log(LogLvl.Info, "remainingPercent: " & Math.Round(remainingPercent * 100, 0) & "%")
                Return True
            End If

        Else
            Log(LogLvl.Warning, "EnumWindows returned False")
        End If

        Return False
    End Function

End Class


Public Class Monitor
    Public Id As Integer
    Public DeviceName As String ' \\.\DISPLAY1
    Public DeviceId As String   ' \\?\DISPLAY#GSM76FE#5&3b13964d&0&UID4354#{e6f07b5f-ee97-4a90-b076-33f57bf4eaa7}
    Public Rectangle As Rectangle

    Public Sub New(Id As Integer, DeviceName As String)
        Me.Id = Id
        Me.DeviceName = DeviceName
    End Sub

    Public Overrides Function ToString() As String
        Return $"DeviceName={{{If(Me.DeviceName, "Nothing")}}} DeviceId={{{If(Me.DeviceId, "Nothing")}}} Rect={{{If(Me.Rectangle.ToString(), "Nothing")}}}"
    End Function
End Class

<JsonConverter(GetType(Converters.StringEnumConverter))>
Public Enum Style
    ''' <summary>
    ''' Center the image; do not stretch.
    ''' </summary>
    Center = 0

    ''' <summary>
    ''' Tile the image across all monitors.
    ''' </summary>
    Tile = 1

    ''' <summary>
    ''' Stretch the image to exactly fit on the monitor.
    ''' </summary>
    Stretch = 2

    ''' <summary>
    ''' Stretch the image to exactly the height or width of the monitor without changing its aspect ratio or cropping the image.
    ''' </summary>
    Fit = 3

    ''' <summary>
    ''' Stretch the image to fill the screen, cropping the image as necessary to avoid letterbox bars.
    ''' </summary>
    Fill = 4

    ''' <summary>
    ''' Spans a single image across all monitors attached to the system.
    ''' </summary>
    Span = 5
End Enum

