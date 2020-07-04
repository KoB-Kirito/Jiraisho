<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Settings
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tab_general = New System.Windows.Forms.TabPage()
        Me.chbo_check_for_updates = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chbo_start_with_windows = New System.Windows.Forms.CheckBox()
        Me.tab_source = New System.Windows.Forms.TabPage()
        Me.bt_check_login = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txbx_password = New System.Windows.Forms.TextBox()
        Me.txbx_username = New System.Windows.Forms.TextBox()
        Me.cobo_source = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tab_search = New System.Windows.Forms.TabPage()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.trba_min_resolution = New System.Windows.Forms.TrackBar()
        Me.chbx_allow_small_deviations = New System.Windows.Forms.CheckBox()
        Me.chbx_only_desktop_ratio = New System.Windows.Forms.CheckBox()
        Me.lb_custom_tags_info = New System.Windows.Forms.Label()
        Me.txbx_custom_tags = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chbx_rating_explicit = New System.Windows.Forms.CheckBox()
        Me.chbx_rating_questionable = New System.Windows.Forms.CheckBox()
        Me.chbx_rating_safe = New System.Windows.Forms.CheckBox()
        Me.tab_output = New System.Windows.Forms.TabPage()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txbx_slideshow_interval = New System.Windows.Forms.TextBox()
        Me.tab_files = New System.Windows.Forms.TabPage()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txbx_max_history = New System.Windows.Forms.TextBox()
        Me.bt_dir_saved = New System.Windows.Forms.Button()
        Me.bt_dir_history = New System.Windows.Forms.Button()
        Me.txbx_dir_saved = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txbx_dir_history = New System.Windows.Forms.TextBox()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lb_desktop_context_info = New System.Windows.Forms.Label()
        Me.chbo_desktop_context_menu = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog_history = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog_saved = New System.Windows.Forms.FolderBrowserDialog()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tab_general.SuspendLayout()
        Me.tab_source.SuspendLayout()
        Me.tab_search.SuspendLayout()
        CType(Me.trba_min_resolution, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.tab_output.SuspendLayout()
        Me.tab_files.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(333, 327)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(170, 33)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 3)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(77, 27)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.Location = New System.Drawing.Point(89, 3)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(77, 27)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Abbrechen"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tab_general)
        Me.TabControl1.Controls.Add(Me.tab_source)
        Me.TabControl1.Controls.Add(Me.tab_search)
        Me.TabControl1.Controls.Add(Me.tab_output)
        Me.TabControl1.Controls.Add(Me.tab_files)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 5
        Me.TabControl1.Size = New System.Drawing.Size(507, 324)
        Me.TabControl1.TabIndex = 1
        '
        'tab_general
        '
        Me.tab_general.BackColor = System.Drawing.SystemColors.Control
        Me.tab_general.Controls.Add(Me.chbo_check_for_updates)
        Me.tab_general.Controls.Add(Me.Label1)
        Me.tab_general.Controls.Add(Me.Label2)
        Me.tab_general.Controls.Add(Me.chbo_start_with_windows)
        Me.tab_general.Location = New System.Drawing.Point(4, 24)
        Me.tab_general.Name = "tab_general"
        Me.tab_general.Padding = New System.Windows.Forms.Padding(3)
        Me.tab_general.Size = New System.Drawing.Size(499, 296)
        Me.tab_general.TabIndex = 0
        Me.tab_general.Text = "General"
        '
        'chbo_check_for_updates
        '
        Me.chbo_check_for_updates.AutoSize = True
        Me.chbo_check_for_updates.Location = New System.Drawing.Point(20, 60)
        Me.chbo_check_for_updates.Name = "chbo_check_for_updates"
        Me.chbo_check_for_updates.Size = New System.Drawing.Size(123, 19)
        Me.chbo_check_for_updates.TabIndex = 3
        Me.chbo_check_for_updates.Text = "Check for Updates"
        Me.chbo_check_for_updates.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Startup"
        '
        'Label2
        '
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(12, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(476, 2)
        Me.Label2.TabIndex = 2
        '
        'chbo_start_with_windows
        '
        Me.chbo_start_with_windows.AutoSize = True
        Me.chbo_start_with_windows.Location = New System.Drawing.Point(20, 35)
        Me.chbo_start_with_windows.Name = "chbo_start_with_windows"
        Me.chbo_start_with_windows.Size = New System.Drawing.Size(128, 19)
        Me.chbo_start_with_windows.TabIndex = 0
        Me.chbo_start_with_windows.Text = "Start with Windows"
        Me.chbo_start_with_windows.UseVisualStyleBackColor = True
        '
        'tab_source
        '
        Me.tab_source.BackColor = System.Drawing.SystemColors.Control
        Me.tab_source.Controls.Add(Me.bt_check_login)
        Me.tab_source.Controls.Add(Me.Label9)
        Me.tab_source.Controls.Add(Me.Label8)
        Me.tab_source.Controls.Add(Me.Label7)
        Me.tab_source.Controls.Add(Me.Label6)
        Me.tab_source.Controls.Add(Me.Label5)
        Me.tab_source.Controls.Add(Me.txbx_password)
        Me.tab_source.Controls.Add(Me.txbx_username)
        Me.tab_source.Controls.Add(Me.cobo_source)
        Me.tab_source.Controls.Add(Me.Label4)
        Me.tab_source.Controls.Add(Me.Label3)
        Me.tab_source.Location = New System.Drawing.Point(4, 24)
        Me.tab_source.Name = "tab_source"
        Me.tab_source.Padding = New System.Windows.Forms.Padding(3)
        Me.tab_source.Size = New System.Drawing.Size(499, 296)
        Me.tab_source.TabIndex = 1
        Me.tab_source.Text = "Source"
        '
        'bt_check_login
        '
        Me.bt_check_login.Location = New System.Drawing.Point(295, 92)
        Me.bt_check_login.Name = "bt_check_login"
        Me.bt_check_login.Size = New System.Drawing.Size(75, 23)
        Me.bt_check_login.TabIndex = 4
        Me.bt_check_login.Text = "Check"
        Me.bt_check_login.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.Label9.Location = New System.Drawing.Point(24, 146)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(131, 15)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "for non-registered users"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.Label8.Location = New System.Drawing.Point(24, 132)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(341, 15)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Authentification is optional, but some features are not available"
        '
        'Label7
        '
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.Location = New System.Drawing.Point(25, 126)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(344, 2)
        Me.Label7.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(160, 74)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 15)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Password"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(24, 74)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 15)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Username"
        '
        'txbx_password
        '
        Me.txbx_password.Location = New System.Drawing.Point(160, 92)
        Me.txbx_password.Name = "txbx_password"
        Me.txbx_password.Size = New System.Drawing.Size(120, 23)
        Me.txbx_password.TabIndex = 4
        '
        'txbx_username
        '
        Me.txbx_username.Location = New System.Drawing.Point(24, 92)
        Me.txbx_username.Name = "txbx_username"
        Me.txbx_username.Size = New System.Drawing.Size(120, 23)
        Me.txbx_username.TabIndex = 4
        '
        'cobo_source
        '
        Me.cobo_source.FormattingEnabled = True
        Me.cobo_source.Location = New System.Drawing.Point(16, 12)
        Me.cobo_source.Name = "cobo_source"
        Me.cobo_source.Size = New System.Drawing.Size(141, 23)
        Me.cobo_source.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 15)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Login"
        '
        'Label3
        '
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Location = New System.Drawing.Point(12, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(476, 2)
        Me.Label3.TabIndex = 2
        '
        'tab_search
        '
        Me.tab_search.BackColor = System.Drawing.SystemColors.Control
        Me.tab_search.Controls.Add(Me.Label17)
        Me.tab_search.Controls.Add(Me.Label16)
        Me.tab_search.Controls.Add(Me.Label15)
        Me.tab_search.Controls.Add(Me.trba_min_resolution)
        Me.tab_search.Controls.Add(Me.chbx_allow_small_deviations)
        Me.tab_search.Controls.Add(Me.chbx_only_desktop_ratio)
        Me.tab_search.Controls.Add(Me.lb_custom_tags_info)
        Me.tab_search.Controls.Add(Me.txbx_custom_tags)
        Me.tab_search.Controls.Add(Me.Label10)
        Me.tab_search.Controls.Add(Me.GroupBox1)
        Me.tab_search.Location = New System.Drawing.Point(4, 24)
        Me.tab_search.Name = "tab_search"
        Me.tab_search.Size = New System.Drawing.Size(499, 296)
        Me.tab_search.TabIndex = 2
        Me.tab_search.Text = "Search"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(401, 67)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(35, 15)
        Me.Label17.TabIndex = 10
        Me.Label17.Text = "100%"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(291, 67)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(23, 15)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "0%"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(287, 20)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(110, 15)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "Minimal Resolution"
        '
        'trba_min_resolution
        '
        Me.trba_min_resolution.Location = New System.Drawing.Point(287, 38)
        Me.trba_min_resolution.Maximum = 5
        Me.trba_min_resolution.Name = "trba_min_resolution"
        Me.trba_min_resolution.Size = New System.Drawing.Size(143, 45)
        Me.trba_min_resolution.TabIndex = 7
        '
        'chbx_allow_small_deviations
        '
        Me.chbx_allow_small_deviations.AutoSize = True
        Me.chbx_allow_small_deviations.Location = New System.Drawing.Point(32, 165)
        Me.chbx_allow_small_deviations.Name = "chbx_allow_small_deviations"
        Me.chbx_allow_small_deviations.Size = New System.Drawing.Size(144, 19)
        Me.chbx_allow_small_deviations.TabIndex = 6
        Me.chbx_allow_small_deviations.Text = "Allow small deviations"
        Me.chbx_allow_small_deviations.UseVisualStyleBackColor = True
        '
        'chbx_only_desktop_ratio
        '
        Me.chbx_only_desktop_ratio.AutoSize = True
        Me.chbx_only_desktop_ratio.Location = New System.Drawing.Point(16, 140)
        Me.chbx_only_desktop_ratio.Name = "chbx_only_desktop_ratio"
        Me.chbx_only_desktop_ratio.Size = New System.Drawing.Size(259, 19)
        Me.chbx_only_desktop_ratio.TabIndex = 5
        Me.chbx_only_desktop_ratio.Text = "Find only images with same ratio as desktop"
        Me.chbx_only_desktop_ratio.UseVisualStyleBackColor = True
        '
        'lb_custom_tags_info
        '
        Me.lb_custom_tags_info.AutoSize = True
        Me.lb_custom_tags_info.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.lb_custom_tags_info.Location = New System.Drawing.Point(16, 122)
        Me.lb_custom_tags_info.Name = "lb_custom_tags_info"
        Me.lb_custom_tags_info.Size = New System.Drawing.Size(0, 15)
        Me.lb_custom_tags_info.TabIndex = 4
        '
        'txbx_custom_tags
        '
        Me.txbx_custom_tags.Location = New System.Drawing.Point(16, 96)
        Me.txbx_custom_tags.Name = "txbx_custom_tags"
        Me.txbx_custom_tags.PlaceholderText = "Ex: blue_sky cloud 1girl"
        Me.txbx_custom_tags.Size = New System.Drawing.Size(300, 23)
        Me.txbx_custom_tags.TabIndex = 4
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(16, 78)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(78, 15)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Custom Tags:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chbx_rating_explicit)
        Me.GroupBox1.Controls.Add(Me.chbx_rating_questionable)
        Me.GroupBox1.Controls.Add(Me.chbx_rating_safe)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 16)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(246, 50)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Rating"
        '
        'chbx_rating_explicit
        '
        Me.chbx_rating_explicit.AutoSize = True
        Me.chbx_rating_explicit.Location = New System.Drawing.Point(174, 22)
        Me.chbx_rating_explicit.Name = "chbx_rating_explicit"
        Me.chbx_rating_explicit.Size = New System.Drawing.Size(64, 19)
        Me.chbx_rating_explicit.TabIndex = 4
        Me.chbx_rating_explicit.Text = "Explicit"
        Me.chbx_rating_explicit.UseVisualStyleBackColor = True
        '
        'chbx_rating_questionable
        '
        Me.chbx_rating_questionable.AutoSize = True
        Me.chbx_rating_questionable.Location = New System.Drawing.Point(70, 22)
        Me.chbx_rating_questionable.Name = "chbx_rating_questionable"
        Me.chbx_rating_questionable.Size = New System.Drawing.Size(96, 19)
        Me.chbx_rating_questionable.TabIndex = 4
        Me.chbx_rating_questionable.Text = "Questionable"
        Me.chbx_rating_questionable.UseVisualStyleBackColor = True
        '
        'chbx_rating_safe
        '
        Me.chbx_rating_safe.AutoSize = True
        Me.chbx_rating_safe.Location = New System.Drawing.Point(16, 22)
        Me.chbx_rating_safe.Name = "chbx_rating_safe"
        Me.chbx_rating_safe.Size = New System.Drawing.Size(48, 19)
        Me.chbx_rating_safe.TabIndex = 4
        Me.chbx_rating_safe.Text = "Safe"
        Me.chbx_rating_safe.UseVisualStyleBackColor = True
        '
        'tab_output
        '
        Me.tab_output.BackColor = System.Drawing.SystemColors.Control
        Me.tab_output.Controls.Add(Me.Label13)
        Me.tab_output.Controls.Add(Me.Label14)
        Me.tab_output.Controls.Add(Me.Label12)
        Me.tab_output.Controls.Add(Me.Label11)
        Me.tab_output.Controls.Add(Me.txbx_slideshow_interval)
        Me.tab_output.Location = New System.Drawing.Point(4, 24)
        Me.tab_output.Name = "tab_output"
        Me.tab_output.Size = New System.Drawing.Size(499, 296)
        Me.tab_output.TabIndex = 3
        Me.tab_output.Text = "Output"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(16, 9)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(60, 15)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Slideshow"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(64, 53)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(58, 15)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "(seconds)"
        '
        'Label12
        '
        Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label12.Location = New System.Drawing.Point(16, 16)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(476, 2)
        Me.Label12.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(24, 30)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(49, 15)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Interval:"
        '
        'txbx_slideshow_interval
        '
        Me.txbx_slideshow_interval.Location = New System.Drawing.Point(24, 48)
        Me.txbx_slideshow_interval.Name = "txbx_slideshow_interval"
        Me.txbx_slideshow_interval.Size = New System.Drawing.Size(37, 23)
        Me.txbx_slideshow_interval.TabIndex = 5
        '
        'tab_files
        '
        Me.tab_files.BackColor = System.Drawing.SystemColors.Control
        Me.tab_files.Controls.Add(Me.Label20)
        Me.tab_files.Controls.Add(Me.txbx_max_history)
        Me.tab_files.Controls.Add(Me.bt_dir_saved)
        Me.tab_files.Controls.Add(Me.bt_dir_history)
        Me.tab_files.Controls.Add(Me.txbx_dir_saved)
        Me.tab_files.Controls.Add(Me.Label19)
        Me.tab_files.Controls.Add(Me.Label18)
        Me.tab_files.Controls.Add(Me.txbx_dir_history)
        Me.tab_files.Location = New System.Drawing.Point(4, 24)
        Me.tab_files.Name = "tab_files"
        Me.tab_files.Size = New System.Drawing.Size(499, 296)
        Me.tab_files.TabIndex = 4
        Me.tab_files.Text = "Files"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(8, 141)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(69, 15)
        Me.Label20.TabIndex = 7
        Me.Label20.Text = "Max history"
        '
        'txbx_max_history
        '
        Me.txbx_max_history.Location = New System.Drawing.Point(8, 159)
        Me.txbx_max_history.Name = "txbx_max_history"
        Me.txbx_max_history.Size = New System.Drawing.Size(33, 23)
        Me.txbx_max_history.TabIndex = 6
        '
        'bt_dir_saved
        '
        Me.bt_dir_saved.Location = New System.Drawing.Point(338, 88)
        Me.bt_dir_saved.Name = "bt_dir_saved"
        Me.bt_dir_saved.Size = New System.Drawing.Size(75, 23)
        Me.bt_dir_saved.TabIndex = 5
        Me.bt_dir_saved.Text = "Change..."
        Me.bt_dir_saved.UseVisualStyleBackColor = True
        '
        'bt_dir_history
        '
        Me.bt_dir_history.Location = New System.Drawing.Point(338, 31)
        Me.bt_dir_history.Name = "bt_dir_history"
        Me.bt_dir_history.Size = New System.Drawing.Size(75, 23)
        Me.bt_dir_history.TabIndex = 4
        Me.bt_dir_history.Text = "Change..."
        Me.bt_dir_history.UseVisualStyleBackColor = True
        '
        'txbx_dir_saved
        '
        Me.txbx_dir_saved.Location = New System.Drawing.Point(8, 88)
        Me.txbx_dir_saved.Name = "txbx_dir_saved"
        Me.txbx_dir_saved.Size = New System.Drawing.Size(324, 23)
        Me.txbx_dir_saved.TabIndex = 3
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(8, 70)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(79, 15)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "Saved Images"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(8, 13)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(45, 15)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "History"
        '
        'txbx_dir_history
        '
        Me.txbx_dir_history.Location = New System.Drawing.Point(8, 31)
        Me.txbx_dir_history.Name = "txbx_dir_history"
        Me.txbx_dir_history.Size = New System.Drawing.Size(324, 23)
        Me.txbx_dir_history.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.lb_desktop_context_info)
        Me.TabPage1.Controls.Add(Me.chbo_desktop_context_menu)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(499, 296)
        Me.TabPage1.TabIndex = 5
        Me.TabPage1.Text = "Hotkeys"
        '
        'lb_desktop_context_info
        '
        Me.lb_desktop_context_info.AutoSize = True
        Me.lb_desktop_context_info.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.lb_desktop_context_info.Location = New System.Drawing.Point(25, 266)
        Me.lb_desktop_context_info.Name = "lb_desktop_context_info"
        Me.lb_desktop_context_info.Size = New System.Drawing.Size(117, 15)
        Me.lb_desktop_context_info.TabIndex = 6
        Me.lb_desktop_context_info.Text = "Admin rights needed"
        '
        'chbo_desktop_context_menu
        '
        Me.chbo_desktop_context_menu.AutoSize = True
        Me.chbo_desktop_context_menu.Location = New System.Drawing.Point(25, 244)
        Me.chbo_desktop_context_menu.Name = "chbo_desktop_context_menu"
        Me.chbo_desktop_context_menu.Size = New System.Drawing.Size(233, 19)
        Me.chbo_desktop_context_menu.TabIndex = 5
        Me.chbo_desktop_context_menu.Text = "Add Hotkeys to Desktop Context Menu"
        Me.chbo_desktop_context_menu.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label24)
        Me.GroupBox3.Controls.Add(Me.ComboBox2)
        Me.GroupBox3.Controls.Add(Me.Label25)
        Me.GroupBox3.Controls.Add(Me.TextBox2)
        Me.GroupBox3.Controls.Add(Me.Label26)
        Me.GroupBox3.Location = New System.Drawing.Point(214, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(200, 70)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Open Current Image"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(97, 41)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(15, 15)
        Me.Label24.TabIndex = 4
        Me.Label24.Text = "+"
        '
        'ComboBox2
        '
        Me.ComboBox2.AutoCompleteCustomSource.AddRange(New String() {"Ctrl", "Alt", "Shift"})
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(17, 38)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(74, 23)
        Me.ComboBox2.TabIndex = 0
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(118, 20)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(26, 15)
        Me.Label25.TabIndex = 3
        Me.Label25.Text = "Key"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(118, 38)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(52, 23)
        Me.TextBox2.TabIndex = 2
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(17, 20)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(52, 15)
        Me.Label26.TabIndex = 1
        Me.Label26.Text = "Modifier"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label23)
        Me.GroupBox2.Controls.Add(Me.ComboBox1)
        Me.GroupBox2.Controls.Add(Me.Label22)
        Me.GroupBox2.Controls.Add(Me.TextBox1)
        Me.GroupBox2.Controls.Add(Me.Label21)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 70)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Save Current Image"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(97, 41)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(15, 15)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "+"
        '
        'ComboBox1
        '
        Me.ComboBox1.AutoCompleteCustomSource.AddRange(New String() {"Ctrl", "Alt", "Shift"})
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(17, 38)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(74, 23)
        Me.ComboBox1.TabIndex = 0
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(118, 20)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(26, 15)
        Me.Label22.TabIndex = 3
        Me.Label22.Text = "Key"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(118, 38)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(52, 23)
        Me.TextBox1.TabIndex = 2
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(17, 20)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(52, 15)
        Me.Label21.TabIndex = 1
        Me.Label21.Text = "Modifier"
        '
        'Settings
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLight
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(507, 363)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Settings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Settings"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.tab_general.ResumeLayout(False)
        Me.tab_general.PerformLayout()
        Me.tab_source.ResumeLayout(False)
        Me.tab_source.PerformLayout()
        Me.tab_search.ResumeLayout(False)
        Me.tab_search.PerformLayout()
        CType(Me.trba_min_resolution, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tab_output.ResumeLayout(False)
        Me.tab_output.PerformLayout()
        Me.tab_files.ResumeLayout(False)
        Me.tab_files.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As Windows.Forms.TabControl
    Friend WithEvents tab_general As Windows.Forms.TabPage
    Friend WithEvents tab_source As Windows.Forms.TabPage
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents chbo_start_with_windows As Windows.Forms.CheckBox
    Friend WithEvents tab_search As Windows.Forms.TabPage
    Friend WithEvents tab_output As Windows.Forms.TabPage
    Friend WithEvents tab_files As Windows.Forms.TabPage
    Friend WithEvents chbo_check_for_updates As Windows.Forms.CheckBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents bt_check_login As Windows.Forms.Button
    Friend WithEvents Label9 As Windows.Forms.Label
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents Label6 As Windows.Forms.Label
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents txbx_password As Windows.Forms.TextBox
    Friend WithEvents txbx_username As Windows.Forms.TextBox
    Friend WithEvents cobo_source As Windows.Forms.ComboBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents chbx_rating_explicit As Windows.Forms.CheckBox
    Friend WithEvents chbx_rating_questionable As Windows.Forms.CheckBox
    Friend WithEvents chbx_rating_safe As Windows.Forms.CheckBox
    Friend WithEvents txbx_custom_tags As Windows.Forms.TextBox
    Friend WithEvents Label10 As Windows.Forms.Label
    Friend WithEvents lb_custom_tags_info As Windows.Forms.Label
    Friend WithEvents Label14 As Windows.Forms.Label
    Friend WithEvents Label12 As Windows.Forms.Label
    Friend WithEvents Label11 As Windows.Forms.Label
    Friend WithEvents txbx_slideshow_interval As Windows.Forms.TextBox
    Friend WithEvents Label13 As Windows.Forms.Label
    Friend WithEvents chbx_allow_small_deviations As Windows.Forms.CheckBox
    Friend WithEvents chbx_only_desktop_ratio As Windows.Forms.CheckBox
    Friend WithEvents Label17 As Windows.Forms.Label
    Friend WithEvents Label16 As Windows.Forms.Label
    Friend WithEvents Label15 As Windows.Forms.Label
    Friend WithEvents trba_min_resolution As Windows.Forms.TrackBar
    Friend WithEvents Label20 As Windows.Forms.Label
    Friend WithEvents txbx_max_history As Windows.Forms.TextBox
    Friend WithEvents bt_dir_saved As Windows.Forms.Button
    Friend WithEvents bt_dir_history As Windows.Forms.Button
    Friend WithEvents txbx_dir_saved As Windows.Forms.TextBox
    Friend WithEvents Label19 As Windows.Forms.Label
    Friend WithEvents Label18 As Windows.Forms.Label
    Friend WithEvents txbx_dir_history As Windows.Forms.TextBox
    Friend WithEvents FolderBrowserDialog_history As Windows.Forms.FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog_saved As Windows.Forms.FolderBrowserDialog
    Friend WithEvents TabPage1 As Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As Windows.Forms.GroupBox
    Friend WithEvents ComboBox1 As Windows.Forms.ComboBox
    Friend WithEvents Label22 As Windows.Forms.Label
    Friend WithEvents TextBox1 As Windows.Forms.TextBox
    Friend WithEvents Label21 As Windows.Forms.Label
    Friend WithEvents Label23 As Windows.Forms.Label
    Friend WithEvents chbo_desktop_context_menu As Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As Windows.Forms.GroupBox
    Friend WithEvents Label24 As Windows.Forms.Label
    Friend WithEvents ComboBox2 As Windows.Forms.ComboBox
    Friend WithEvents Label25 As Windows.Forms.Label
    Friend WithEvents TextBox2 As Windows.Forms.TextBox
    Friend WithEvents Label26 As Windows.Forms.Label
    Friend WithEvents lb_desktop_context_info As Windows.Forms.Label
End Class
