Imports System.Security.Principal
Imports System.Windows.Forms

Public Class Settings

    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles Me.Load
        Log(LogLvl.Trace, $"Called")

        'Restores the last location from config 'ToDo: Store this in registry?
        Me.Location = CFG.SettingsWindowDefaultPosition

        'ToDo: Restore last active tab?

        'General
        Me.chbo_start_with_windows.Checked = CFG.StartWithWindows
        Me.chbo_check_for_updates.Checked = CFG.CheckForUpdates

        'Source
        cobo_source.DataSource = DownloadClient.Sources
        cobo_source.SelectedItem = CFG.Source
        txbx_username.Text = If(CFG.Username, "")
        txbx_password.Text = If(CFG.Password, "")

        'Search
        If CFG.Rating.HasFlag(Rating.Safe) Then chbx_rating_safe.Checked = True
        If CFG.Rating.HasFlag(Rating.Questionable) Then chbx_rating_questionable.Checked = True
        If CFG.Rating.HasFlag(Rating.Explicit) Then chbx_rating_explicit.Checked = True
        If CFG.CustomTags IsNot Nothing AndAlso CFG.CustomTags.Length > 0 Then
            txbx_custom_tags.Text = String.Join(" ", CFG.CustomTags)
        End If
        chbx_only_desktop_ratio.Checked = CFG.OnlyDesktopRatio
        If chbx_only_desktop_ratio.Checked Then
            chbx_allow_small_deviations.Enabled = True
            chbx_allow_small_deviations.Checked = CFG.AllowSmallDeviations
        Else
            chbx_allow_small_deviations.Enabled = False
        End If
        trba_min_resolution.Value = CFG.MinResolution * 10

        'Output
        txbx_slideshow_interval.Text = CFG.IntervalInSeconds
        'ToDo: Style

        'Files
        txbx_dir_history.Text = CFG.DirHistory
        txbx_dir_saved.Text = CFG.DirSaved
        txbx_max_history.Text = CFG.MaxHistory

        'Hotkeys
        'Check cascaded
        Dim key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\DesktopBackground\shell\" & AppName, False)
        If key IsNot Nothing Then
            'If key is present, the context menu is currently enabled
            chbo_desktop_context_menu.Checked = True
            chbo_context_menu_cascaded.Checked = True
        Else
            'Check normal
            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\DesktopBackground\shell\" & AppName & "-fav", False)
            If key IsNot Nothing Then
                chbo_desktop_context_menu.Checked = True
            Else
                'No context menu is enabled -> Disable sub setting
                chbo_context_menu_cascaded.Enabled = False
            End If
        End If

        ' Replaced with current user version
        'If New WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator) Then
        '    'admin rights => registry access
        '    Dim key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("DesktopBackground\shell\Jiraisho")
        '    If key IsNot Nothing Then
        '        'If key is present, the context menu is currently enabled
        '        chbo_desktop_context_menu.Checked = True
        '    End If
        '    lb_desktop_context_info.Text = ""
        'Else
        '    'No admin rights
        '    chbo_desktop_context_menu.Enabled = False
        '    lb_desktop_context_info.Text = "Run this app as administrator to enable this setting"
        'End If

        'ToDo: Load config values into form

    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        Log(LogLvl.Debug, "Called")

        'Check all changes before applying
        Dim everythingIsOk As Boolean = True
        Dim tempCFG = CFG.Clone()

#Region "General"

        tempCFG.StartWithWindows = chbo_start_with_windows.Checked
        tempCFG.CheckForUpdates = chbo_check_for_updates.Checked

#End Region

#Region "Source"

        'Source
        If DownloadClient.Sources.Contains(cobo_source.SelectedItem) Then
            tempCFG.Source = cobo_source.SelectedItem
        End If

        'Login
        tempCFG.Username = txbx_username.Text
        tempCFG.Password = txbx_password.Text

#End Region

#Region "Search"
        'Rating
        Dim ratingValue As Integer
        If chbx_rating_safe.Checked Then ratingValue += Rating.Safe
        If chbx_rating_questionable.Checked Then ratingValue += Rating.Questionable
        If chbx_rating_explicit.Checked Then ratingValue += Rating.Explicit
        If ratingValue = 0 Then
            everythingIsOk = False
            Log(LogLvl.Error, "At least one rating must be selected")
        Else
            tempCFG.Rating = ratingValue
        End If

        'Custom Tags
        If Not String.IsNullOrWhiteSpace(txbx_custom_tags.Text) Then
            Dim tagsArray = txbx_custom_tags.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            tempCFG.CustomTags = tagsArray
        Else
            tempCFG.CustomTags = Nothing
        End If

        'Only desktop ratio
        tempCFG.OnlyDesktopRatio = chbx_only_desktop_ratio.Checked
        tempCFG.AllowSmallDeviations = chbx_allow_small_deviations.Checked

        'Minimum resolution
        tempCFG.MinResolution = trba_min_resolution.Value * 0.1

#End Region

#Region "Output"

        'Interval
        Dim input = Me.txbx_slideshow_interval.Text.Trim()
        Dim output As Integer
        If Not String.IsNullOrWhiteSpace(input) AndAlso Integer.TryParse(input, output) Then
            If output >= 5 Then
                tempCFG.IntervalInSeconds = output
            Else
                'Input is too small
                Me.txbx_slideshow_interval.BackColor = Drawing.Color.Red
                Me.txbx_slideshow_interval.Text = 5
                MessageBox.Show("The minimum interval is 5 seconds!")
                everythingIsOk = False
                tab_output.Text = "Output*"
            End If
        Else
            'Input is no integer
            Me.txbx_slideshow_interval.BackColor = Drawing.Color.Red
            Me.txbx_slideshow_interval.Text = CFG.IntervalInSeconds
            MessageBox.Show("Interval must be a number!")
            everythingIsOk = False
            tab_output.Text = "Output*"
        End If


#End Region

#Region "Hotkeys"

        'ToDo: Implement
        'https://stackoverflow.com/questions/906899/binding-an-enum-to-a-winforms-combo-box-and-then-setting-it/9541156

#End Region

        ''MaxHistory
        'input = Me.TextBox2.Text.Trim()
        'If Not String.IsNullOrWhiteSpace(input) AndAlso Integer.TryParse(input, output) Then
        '    If output >= WallpaperClient.MonitorCount Then
        '        tempHistory = output
        '    Else
        '        'Input is too small
        '        Me.TextBox2.BackColor = Drawing.Color.Red
        '        Me.TextBox2.Text = WallpaperClient.MonitorCount
        '        MessageBox.Show("The minimum history count is the monitorcount")
        '        everythingIsOk = False
        '    End If
        'Else
        '    'Input is no integer
        '    Me.TextBox2.BackColor = Drawing.Color.Red
        '    Me.TextBox2.Text = CFG.MaxHistory
        '    MessageBox.Show("Input must be a number!")
        '    everythingIsOk = False
        'End If


        ' APPLY SETTINGS

        Log(LogLvl.Debug, "Everything OK = " & everythingIsOk)
        If everythingIsOk Then
            'Apply changes that are not parse from config while running
            'Interval
            SlideshowTimer.Interval = tempCFG.IntervalInSeconds * 1000

            'Current Source
            If CFG.Source <> tempCFG.Source Then
                Downloader.SetCurrentSource(tempCFG.Source)
            End If

            'Context menu
            If chbo_desktop_context_menu.Checked Then
                If chbo_context_menu_cascaded.Checked Then
                    Registry.DeleteContextMenu()
                    Registry.CreateCascadedContextMenu()
                Else
                    Registry.DeleteCascadedContextMenu()
                    Registry.CreateContextMenu()
                End If
            Else
                Registry.DeleteContextMenu()
                Registry.DeleteCascadedContextMenu()
            End If

            'SavedDir
            Registry.SetValue("DirSaved", tempCFG.DirSaved)

            'Hotkeys
            HotkeyListenerWindow.UpdateHotkey(Hotkey.SaveCurrentImage, tempCFG.HK_SaveCurrent.Item1, tempCFG.HK_SaveCurrent.Item2)
            HotkeyListenerWindow.UpdateHotkey(Hotkey.OpenCurrentImage, tempCFG.HK_OpenCurrent.Item1, tempCFG.HK_OpenCurrent.Item2)

            'Save config
            CFG = tempCFG

            'Close window via OK only if everything is OK
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        'Always close on cancel
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub Settings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Log(LogLvl.Debug, $"Called. RestoreBounds.Location = {Location.ToString()}")
        'Saves the last location to config
        CFG.SettingsWindowDefaultPosition = Location
    End Sub

    Private Async Sub txbx_custom_tags_Leave(sender As Object, e As EventArgs) Handles txbx_custom_tags.Leave
        Dim count = Await Downloader.GetPostCountAsync(txbx_custom_tags.Text)
        Select Case count
            Case -1
                'Error
                lb_custom_tags_info.ForeColor = Drawing.Color.DarkRed
                lb_custom_tags_info.Text = "Parsing error"

            Case 0
                lb_custom_tags_info.ForeColor = Drawing.Color.Red
                lb_custom_tags_info.Text = "Results: 0"

            Case 1 To 49
                lb_custom_tags_info.ForeColor = Drawing.Color.DarkOrange
                lb_custom_tags_info.Text = "Results: " & count

            Case 50 To 99
                lb_custom_tags_info.ForeColor = Drawing.Color.Orange
                lb_custom_tags_info.Text = "Results: " & count

            Case 100 To 199
                lb_custom_tags_info.ForeColor = Drawing.Color.GreenYellow
                lb_custom_tags_info.Text = "Results: " & count

            Case Else
                lb_custom_tags_info.ForeColor = Drawing.Color.DarkGreen
                lb_custom_tags_info.Text = "Results: " & count
        End Select
    End Sub

    Private Sub chbx_only_desktop_ratio_CheckedChanged(sender As Object, e As EventArgs) Handles chbx_only_desktop_ratio.CheckedChanged
        'Only enable sub setting if main setting is checked
        If chbx_only_desktop_ratio.Checked Then
            chbx_allow_small_deviations.Enabled = True
        Else
            chbx_allow_small_deviations.Enabled = False
        End If
    End Sub

    Private Sub bt_dir_history_Click(sender As Object, e As EventArgs) Handles bt_dir_history.Click
        Dim res = FolderBrowserDialog_history.ShowDialog()
        If res = DialogResult.OK Then
            txbx_dir_history.Text = FolderBrowserDialog_history.SelectedPath
        End If
    End Sub

    Private Sub bt_dir_saved_Click(sender As Object, e As EventArgs) Handles bt_dir_saved.Click
        Dim res = FolderBrowserDialog_saved.ShowDialog()
        If res = DialogResult.OK Then
            txbx_dir_saved.Text = FolderBrowserDialog_saved.SelectedPath
        End If
    End Sub

    Private Sub chbo_desktop_context_menu_CheckedChanged(sender As Object, e As EventArgs) Handles chbo_desktop_context_menu.CheckedChanged
        'Only enable sub setting if main setting is checked
        If chbo_desktop_context_menu.Checked Then
            chbo_context_menu_cascaded.Enabled = True
        Else
            chbo_context_menu_cascaded.Enabled = False
        End If
    End Sub
End Class
