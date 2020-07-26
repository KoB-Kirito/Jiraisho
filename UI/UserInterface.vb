Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports BooruSharp.Search.Post
Imports SixLabors.ImageSharp
Imports System.Drawing.Text
Imports System.Security.Policy

Public Class UserInterface
    Inherits ApplicationContext

    Friend WithEvents TrayIcon As New NotifyIcon
    Friend WithEvents HotkeyListener As HotkeyListenerWindow

    Private dropdownInterval As ToolStripMenuItem
    Private dropdownRating As ToolStripMenuItem
    Private dropdownStyle As ToolStripMenuItem

    Public Sub New()
        Log(LogLvl.Trace, "Called", Source:="New UserInterface")

        'Setup tray icon
        TrayIcon.Text = AppName
        TrayIcon.Icon = My.Resources.icon 'ToDo: embed in app

        Dim contextMenu = New ContextMenuStrip
        contextMenu.ShowImageMargin = False

        'Save current image
        'contextMenu.Items.Add(New ToolStripMenuItem("Save current image", Nothing, New EventHandler(AddressOf SaveCurrentImage), HotkeyListenerWindow.ModifierToKeys(CFG.HK_SaveCurrent.Item1) Or CFG.HK_SaveCurrent.Item2))

        'Open current image
        'contextMenu.Items.Add(New ToolStripMenuItem("Open current image", Nothing, New EventHandler(AddressOf OpenCurrentImage), HotkeyListenerWindow.ModifierToKeys(CFG.HK_OpenCurrent.Item1) Or CFG.HK_OpenCurrent.Item2))

        '-----------------
        'contextMenu.Items.Add(New ToolStripSeparator)

        'Interval
        dropdownInterval = New ToolStripMenuItem("Interval")
        AddHandler dropdownInterval.DropDown.Closing, AddressOf PreventClosingOnClick
        dropdownInterval.DropDownItems.Add(New ToolStripMenuItem("10 sec", Nothing, New EventHandler(AddressOf SetInterval), "10000"))
        dropdownInterval.DropDownItems.Add(New ToolStripMenuItem("20 sec", Nothing, New EventHandler(AddressOf SetInterval), "20000"))
        dropdownInterval.DropDownItems.Add(New ToolStripMenuItem("30 sec", Nothing, New EventHandler(AddressOf SetInterval), "30000"))
        dropdownInterval.DropDownItems.Add(New ToolStripMenuItem("1 min", Nothing, New EventHandler(AddressOf SetInterval), "60000"))
        dropdownInterval.DropDownItems.Add(New ToolStripMenuItem("2 min", Nothing, New EventHandler(AddressOf SetInterval), "120000"))
        dropdownInterval.DropDownItems.Add(New ToolStripMenuItem("5 min", Nothing, New EventHandler(AddressOf SetInterval), "300000"))
        dropdownInterval.DropDownItems.Add(New ToolStripMenuItem("15 min", Nothing, New EventHandler(AddressOf SetInterval), "900000"))
        dropdownInterval.DropDownItems.Add(New ToolStripMenuItem("30 min", Nothing, New EventHandler(AddressOf SetInterval), "1800000"))
        For Each item As ToolStripMenuItem In dropdownInterval.DropDownItems
            If Integer.Parse(item.Name) / 1000 = CFG.IntervalInSeconds Then
                item.Checked = True
                Exit For
            End If
        Next
        contextMenu.Items.Add(dropdownInterval)

        'Rating
        dropdownRating = New ToolStripMenuItem("Rating")
        AddHandler dropdownRating.DropDown.Closing, AddressOf PreventClosingOnClick
        dropdownRating.DropDownItems.Add(New ToolStripMenuItem("Safe", Nothing, New EventHandler(AddressOf SetRating), "safe"))
        dropdownRating.DropDownItems.Add(New ToolStripMenuItem("Questionable", Nothing, New EventHandler(AddressOf SetRating), "questionable"))
        dropdownRating.DropDownItems.Add(New ToolStripMenuItem("Explicit", Nothing, New EventHandler(AddressOf SetRating), "explicit"))
        For Each item As ToolStripMenuItem In dropdownRating.DropDownItems
            Select Case item.Name
                Case "safe"
                    If CFG.Rating.HasFlag(Rating.Safe) Then item.Checked = True

                Case "questionable"
                    If CFG.Rating.HasFlag(Rating.Questionable) Then item.Checked = True

                Case "explicit"
                    If CFG.Rating.HasFlag(Rating.Explicit) Then item.Checked = True

            End Select
        Next
        contextMenu.Items.Add(dropdownRating)

        'Style
        'dropdownStyle = New ToolStripMenuItem("Style")
        'AddHandler dropdownStyle.DropDown.Closing, AddressOf PreventClosingOnClick
        'dropdownStyle.DropDownItems.Add(New ToolStripMenuItem("Center", Nothing, New EventHandler(AddressOf SetStyle), "Center"))
        'dropdownStyle.DropDownItems.Add(New ToolStripMenuItem("Stretch", Nothing, New EventHandler(AddressOf SetStyle), "Stretch"))
        'dropdownStyle.DropDownItems.Add(New ToolStripMenuItem("Fit", Nothing, New EventHandler(AddressOf SetStyle), "Fit"))
        'dropdownStyle.DropDownItems.Add(New ToolStripMenuItem("Fill", Nothing, New EventHandler(AddressOf SetStyle), "Fill"))
        ''ToDo: Tile and Span do occupy ALL monitors. Add them?
        'For Each item As ToolStripMenuItem In dropdownStyle.DropDownItems
        '    If CFG.Style.ToString() = item.Name Then
        '        item.Checked = True
        '        Exit For
        '    End If
        'Next
        'contextMenu.Items.Add(dropdownStyle)

        '-----------------
        contextMenu.Items.Add(New ToolStripSeparator)

        contextMenu.Items.Add("Settings", Nothing, New EventHandler(AddressOf ShowSettings))
        contextMenu.Items.Add("Exit", Nothing, New EventHandler(AddressOf ExitApp))
        TrayIcon.ContextMenuStrip = contextMenu

        AddHandler TrayIcon.DoubleClick, AddressOf TrayDoubleClick

        TrayIcon.Visible = True


        'Hotkeys
        HotkeyListener = New HotkeyListenerWindow
        AddHandler HotkeyListener.HotkeyPressed, AddressOf HotkeyPressed


        AddHandler Application.ApplicationExit, AddressOf ApplicationExit

        Log(LogLvl.Trace, "Reached end", Source:="New UserInterface")
    End Sub

    Private Sub HotkeyPressed(Hotkey As Hotkey)
        Log(LogLvl.Debug, "Hotkey " & Hotkey.ToString() & " pressed")
        Select Case Hotkey
            Case Hotkey.SaveCurrentImage
                SaveCurrentImage(Nothing, Nothing)

            Case Hotkey.OpenCurrentImage
                OpenCurrentImage(Nothing, Nothing)

            Case Hotkey.FavCurrentImage
                FavCurrentImage(Nothing, Nothing)

            Case Else
                Log(LogLvl.Warning, Hotkey & " pressed")
        End Select
    End Sub

    Private Sub PreventClosingOnClick(sender As Object, e As ToolStripDropDownClosingEventArgs)
        If e.CloseReason = ToolStripDropDownCloseReason.ItemClicked Then
            e.Cancel = True
        End If
    End Sub

    Private Sub ApplicationExit(sender As Object, e As EventArgs)
        If TrayIcon.Visible Then TrayIcon.Visible = False
    End Sub

    Private Sub SaveCurrentImage(sender As Object, e As EventArgs)
        Log(LogLvl.Trace, "Called")

        'Get current screen, save wallpaper of that screen
        Dim activeScreen = Screen.FromPoint(Cursor.Position)
        UserActions.SaveWallpaper(activeScreen)

        Log(LogLvl.Trace, "Reached end")
    End Sub

    Private Sub OpenCurrentImage(sender As Object, e As EventArgs)
        Log(LogLvl.Trace, "Called")

        'Get current screen, save wallpaper of that screen
        Dim activeScreen = Screen.FromPoint(Cursor.Position)
        UserActions.OpenWallpaper(activeScreen)

        Log(LogLvl.Trace, "Reached end")
    End Sub

    Private Sub FavCurrentImage(sender As Object, e As EventArgs)
        Log(LogLvl.Trace, "Called")

        'Get current screen, save wallpaper of that screen
        Dim activeScreen = Screen.FromPoint(Cursor.Position)
        UserActions.FavWallpaper(activeScreen)

        Log(LogLvl.Trace, "Reached end")
    End Sub


    Private Sub SetInterval(sender As ToolStripMenuItem, e As EventArgs)
        Dim Interval As Double
        Double.TryParse(sender.Name, Interval)
        Log(LogLvl.Debug, $"Called with {Interval}")

        'Radio button functionality
        For Each entry In dropdownInterval.DropDownItems
            Dim menuItem = TryCast(entry, ToolStripMenuItem)
            If menuItem Is sender Then
                entry.Checked = True
            Else
                entry.Checked = False
            End If
        Next

        SlideshowTimer.Stop()
        SlideshowTimer.Interval = Interval 'ToDo: Threadsafe?
        SlideshowTimer.Start()
        CFG.IntervalInSeconds = Interval / 1000
    End Sub

    Private Sub SetRating(sender As ToolStripMenuItem, ea As EventArgs)
        'Switch state
        sender.Checked = Not sender.Checked

        'Get curr state
        Dim rating As Rating
        For Each entry As ToolStripMenuItem In dropdownRating.DropDownItems
            Select Case entry.Name
                Case "safe"
                    If entry.Checked Then rating += Rating.Safe
                Case "questionable"
                    If entry.Checked Then rating += Rating.Questionable
                Case "explicit"
                    If entry.Checked Then rating += Rating.Explicit
            End Select
        Next

        'No rating at all is not possible
        If rating = 0 Then
            'Switch back
            sender.Checked = Not sender.Checked
            Return
        End If

        Dim ratingString As String = ""
        Select Case rating
            Case Rating.Safe
                ratingString = "rating:safe"

            Case Rating.Questionable
                ratingString = "rating:questionable"

            Case Rating.Explicit
                ratingString = "rating:explicit"

            Case Rating.NoSafe
                ratingString = "-rating:safe"

            Case Rating.NoQuestionable
                ratingString = "-rating:questionable"

            Case Rating.NoExplicit
                ratingString = "-rating:explicit"

            Case Rating.All
                ratingString = "" ' All

        End Select

        CFG.Rating = rating
        Log(LogLvl.Info, "Rating: " & rating.ToString() & "  ratingString: " & ratingString)
    End Sub

    'Private Sub SetStyle(sender As ToolStripMenuItem, e As EventArgs)
    '    Dim style As Style = [Enum].Parse(GetType(Style), sender.Name)
    '    Desktop.SetGlobalStyle(style)
    '    CFG.Style = style
    '    For Each item As ToolStripMenuItem In dropdownStyle.DropDownItems
    '        If item Is sender Then
    '            item.Checked = True
    '        Else
    '            item.Checked = False
    '        End If
    '    Next
    'End Sub


    Private Sub TrayDoubleClick(sender As Object, e As EventArgs)
        Log(LogLvl.Debug, "Called")
        If SlideshowTimer.Enabled Then
            SlideshowTimer.Stop()
            TrayIcon.ShowBalloonTip(10000, "Info", "Slideshow paused", ToolTipIcon.Info)
        Else
            SlideshowTimer.Start()
            TrayIcon.ShowBalloonTip(10000, "Info", "Slideshow presumed", ToolTipIcon.Info)
        End If
    End Sub

    Public Sub ShowSettings(sender As ToolStripMenuItem, e As EventArgs)
        Log(LogLvl.Debug, "Called")

        'Disable the context menu while settings are opened
        Dim contextMenu = sender.GetCurrentParent()
        Try
            contextMenu.Enabled = False
            SlideshowTimer.Stop()

            'Only keep the form alive while its used,
            'which should be rarely
            Using form = New Settings
                form.ShowDialog()
            End Using
        Finally
            contextMenu.Enabled = True
            SlideshowTimer.Start()
        End Try
    End Sub

    Public Sub ExitApp()
        TrayIcon.Visible = False
        Application.Exit()
    End Sub
End Class