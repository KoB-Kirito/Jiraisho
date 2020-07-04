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
        'ToDo: Hier weiter

        Dim key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\DesktopBackground\shell\Jiraisho")
        If key IsNot Nothing Then
            'If key is present, the context menu is currently enabled
            chbo_desktop_context_menu.Checked = True
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



#End Region

#Region "Source"

        'Source
        If DownloadClient.Sources.Contains(cobo_source.SelectedItem) Then
            tempCFG.Source = cobo_source.SelectedItem
        End If

#End Region

#Region "Search"

        'Custom Tags
        If Not String.IsNullOrWhiteSpace(txbx_custom_tags.Text) Then
            Dim tagsArray = txbx_custom_tags.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            tempCFG.CustomTags = tagsArray
        Else
            tempCFG.CustomTags = Nothing
        End If

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

            'Registry
            If chbo_desktop_context_menu.Checked Then
                Try
                    Dim mainKey = "Software\Classes\DesktopBackground\Shell\Jiraisho"
                    Dim exePath = IO.Path.ChangeExtension(Application.ExecutablePath, "exe")

                    'Main SubKey
                    Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(mainKey)
                    'Enable sub entries
                    key.SetValue("SubCommands", "", Microsoft.Win32.RegistryValueKind.String)

                    'Add current
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(mainKey & "\Shell\add")
                    key.SetValue("MUIVerb", "Add current to favourites", Microsoft.Win32.RegistryValueKind.String)
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(mainKey & "\Shell\add\command")
                    key.SetValue("", $"""{exePath}"" ""-cmt"" ""-add""")

                    'Save current
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(mainKey & "\Shell\save")
                    key.SetValue("MUIVerb", "Save current to disk", Microsoft.Win32.RegistryValueKind.String)
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(mainKey & "\Shell\save\command")
                    key.SetValue("", $"""{exePath}"" ""-cmt"" ""-save""")

                    'Open current
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(mainKey & "\Shell\open")
                    key.SetValue("MUIVerb", "Open current in browser", Microsoft.Win32.RegistryValueKind.String)
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(mainKey & "\Shell\open\command")
                    key.SetValue("", $"""{exePath}"" ""-cmt"" ""-open""")

                Catch ex As Exception
                    Log(LogLvl.Error, "Can't access the registry to add context menu to desktop")
                End Try
            End If

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
End Class
