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
        Me.txbx_password = New System.Windows.Forms.TextBox()
        Me.txbx_username = New System.Windows.Forms.TextBox()
        Me.bt_check_login = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
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
        Me.chbx_skip_obscured = New System.Windows.Forms.CheckBox()
        Me.txbx_slideshow_interval = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.tab_files = New System.Windows.Forms.TabPage()
        Me.txbx_max_history = New System.Windows.Forms.TextBox()
        Me.txbx_dir_history = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.bt_dir_saved = New System.Windows.Forms.Button()
        Me.bt_dir_history = New System.Windows.Forms.Button()
        Me.txbx_dir_saved = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.chbo_context_menu_cascaded = New System.Windows.Forms.CheckBox()
        Me.chbo_desktop_context_menu = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txbx_hk_open_key = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.cobo_hk_open_modifier = New System.Windows.Forms.ComboBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txbx_hk_save_key = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.cobo_hk_save_modifier = New System.Windows.Forms.ComboBox()
        Me.Label22 = New System.Windows.Forms.Label()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(618, 698)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(316, 70)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(7, 6)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(143, 58)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.Location = New System.Drawing.Point(165, 6)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(143, 58)
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
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(6)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 5
        Me.TabControl1.Size = New System.Drawing.Size(942, 691)
        Me.TabControl1.TabIndex = 1
        '
        'tab_general
        '
        Me.tab_general.BackColor = System.Drawing.SystemColors.Control
        Me.tab_general.Controls.Add(Me.chbo_check_for_updates)
        Me.tab_general.Controls.Add(Me.Label1)
        Me.tab_general.Controls.Add(Me.Label2)
        Me.tab_general.Controls.Add(Me.chbo_start_with_windows)
        Me.tab_general.Location = New System.Drawing.Point(8, 46)
        Me.tab_general.Margin = New System.Windows.Forms.Padding(6)
        Me.tab_general.Name = "tab_general"
        Me.tab_general.Padding = New System.Windows.Forms.Padding(6)
        Me.tab_general.Size = New System.Drawing.Size(926, 637)
        Me.tab_general.TabIndex = 0
        Me.tab_general.Text = "General"
        '
        'chbo_check_for_updates
        '
        Me.chbo_check_for_updates.AutoSize = True
        Me.chbo_check_for_updates.Location = New System.Drawing.Point(37, 128)
        Me.chbo_check_for_updates.Margin = New System.Windows.Forms.Padding(6)
        Me.chbo_check_for_updates.Name = "chbo_check_for_updates"
        Me.chbo_check_for_updates.Size = New System.Drawing.Size(242, 36)
        Me.chbo_check_for_updates.TabIndex = 3
        Me.chbo_check_for_updates.Text = "Check for Updates"
        Me.chbo_check_for_updates.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 19)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 32)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Startup"
        '
        'Label2
        '
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(22, 34)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(884, 4)
        Me.Label2.TabIndex = 2
        '
        'chbo_start_with_windows
        '
        Me.chbo_start_with_windows.AutoSize = True
        Me.chbo_start_with_windows.Location = New System.Drawing.Point(37, 75)
        Me.chbo_start_with_windows.Margin = New System.Windows.Forms.Padding(6)
        Me.chbo_start_with_windows.Name = "chbo_start_with_windows"
        Me.chbo_start_with_windows.Size = New System.Drawing.Size(250, 36)
        Me.chbo_start_with_windows.TabIndex = 0
        Me.chbo_start_with_windows.Text = "Start with Windows"
        Me.chbo_start_with_windows.UseVisualStyleBackColor = True
        '
        'tab_source
        '
        Me.tab_source.BackColor = System.Drawing.SystemColors.Control
        Me.tab_source.Controls.Add(Me.txbx_password)
        Me.tab_source.Controls.Add(Me.txbx_username)
        Me.tab_source.Controls.Add(Me.bt_check_login)
        Me.tab_source.Controls.Add(Me.Label9)
        Me.tab_source.Controls.Add(Me.Label8)
        Me.tab_source.Controls.Add(Me.Label7)
        Me.tab_source.Controls.Add(Me.Label6)
        Me.tab_source.Controls.Add(Me.Label5)
        Me.tab_source.Controls.Add(Me.cobo_source)
        Me.tab_source.Controls.Add(Me.Label4)
        Me.tab_source.Controls.Add(Me.Label3)
        Me.tab_source.Location = New System.Drawing.Point(8, 46)
        Me.tab_source.Margin = New System.Windows.Forms.Padding(6)
        Me.tab_source.Name = "tab_source"
        Me.tab_source.Padding = New System.Windows.Forms.Padding(6)
        Me.tab_source.Size = New System.Drawing.Size(926, 637)
        Me.tab_source.TabIndex = 1
        Me.tab_source.Text = "Source"
        '
        'txbx_password
        '
        Me.txbx_password.Location = New System.Drawing.Point(297, 196)
        Me.txbx_password.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_password.Name = "txbx_password"
        Me.txbx_password.Size = New System.Drawing.Size(219, 39)
        Me.txbx_password.TabIndex = 4
        '
        'txbx_username
        '
        Me.txbx_username.Location = New System.Drawing.Point(45, 196)
        Me.txbx_username.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_username.Name = "txbx_username"
        Me.txbx_username.Size = New System.Drawing.Size(219, 39)
        Me.txbx_username.TabIndex = 4
        '
        'bt_check_login
        '
        Me.bt_check_login.Location = New System.Drawing.Point(548, 196)
        Me.bt_check_login.Margin = New System.Windows.Forms.Padding(6)
        Me.bt_check_login.Name = "bt_check_login"
        Me.bt_check_login.Size = New System.Drawing.Size(139, 49)
        Me.bt_check_login.TabIndex = 4
        Me.bt_check_login.Text = "Check"
        Me.bt_check_login.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.Label9.Location = New System.Drawing.Point(45, 311)
        Me.Label9.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(261, 32)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "for non-registered users"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.Label8.Location = New System.Drawing.Point(45, 282)
        Me.Label8.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(672, 32)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Authentification is optional, but some features are not available"
        '
        'Label7
        '
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.Location = New System.Drawing.Point(46, 269)
        Me.Label7.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(639, 4)
        Me.Label7.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(297, 158)
        Me.Label6.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(111, 32)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Password"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(45, 158)
        Me.Label5.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(121, 32)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Username"
        '
        'cobo_source
        '
        Me.cobo_source.FormattingEnabled = True
        Me.cobo_source.Location = New System.Drawing.Point(30, 26)
        Me.cobo_source.Margin = New System.Windows.Forms.Padding(6)
        Me.cobo_source.Name = "cobo_source"
        Me.cobo_source.Size = New System.Drawing.Size(258, 40)
        Me.cobo_source.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(22, 105)
        Me.Label4.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 32)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Login"
        '
        'Label3
        '
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Location = New System.Drawing.Point(22, 119)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(884, 4)
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
        Me.tab_search.Location = New System.Drawing.Point(8, 46)
        Me.tab_search.Margin = New System.Windows.Forms.Padding(6)
        Me.tab_search.Name = "tab_search"
        Me.tab_search.Size = New System.Drawing.Size(926, 637)
        Me.tab_search.TabIndex = 2
        Me.tab_search.Text = "Search"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(745, 143)
        Me.Label17.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(73, 32)
        Me.Label17.TabIndex = 10
        Me.Label17.Text = "100%"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(540, 143)
        Me.Label16.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(47, 32)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "0%"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(533, 43)
        Me.Label15.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(220, 32)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "Minimal Resolution"
        '
        'trba_min_resolution
        '
        Me.trba_min_resolution.Location = New System.Drawing.Point(533, 81)
        Me.trba_min_resolution.Margin = New System.Windows.Forms.Padding(6)
        Me.trba_min_resolution.Name = "trba_min_resolution"
        Me.trba_min_resolution.Size = New System.Drawing.Size(266, 90)
        Me.trba_min_resolution.TabIndex = 7
        '
        'chbx_allow_small_deviations
        '
        Me.chbx_allow_small_deviations.AutoSize = True
        Me.chbx_allow_small_deviations.Location = New System.Drawing.Point(59, 352)
        Me.chbx_allow_small_deviations.Margin = New System.Windows.Forms.Padding(6)
        Me.chbx_allow_small_deviations.Name = "chbx_allow_small_deviations"
        Me.chbx_allow_small_deviations.Size = New System.Drawing.Size(282, 36)
        Me.chbx_allow_small_deviations.TabIndex = 6
        Me.chbx_allow_small_deviations.Text = "Allow small deviations"
        Me.chbx_allow_small_deviations.UseVisualStyleBackColor = True
        '
        'chbx_only_desktop_ratio
        '
        Me.chbx_only_desktop_ratio.AutoSize = True
        Me.chbx_only_desktop_ratio.Location = New System.Drawing.Point(30, 299)
        Me.chbx_only_desktop_ratio.Margin = New System.Windows.Forms.Padding(6)
        Me.chbx_only_desktop_ratio.Name = "chbx_only_desktop_ratio"
        Me.chbx_only_desktop_ratio.Size = New System.Drawing.Size(519, 36)
        Me.chbx_only_desktop_ratio.TabIndex = 5
        Me.chbx_only_desktop_ratio.Text = "Find only images with same ratio as desktop"
        Me.chbx_only_desktop_ratio.UseVisualStyleBackColor = True
        '
        'lb_custom_tags_info
        '
        Me.lb_custom_tags_info.AutoSize = True
        Me.lb_custom_tags_info.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.lb_custom_tags_info.Location = New System.Drawing.Point(30, 260)
        Me.lb_custom_tags_info.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lb_custom_tags_info.Name = "lb_custom_tags_info"
        Me.lb_custom_tags_info.Size = New System.Drawing.Size(0, 32)
        Me.lb_custom_tags_info.TabIndex = 4
        '
        'txbx_custom_tags
        '
        Me.txbx_custom_tags.Location = New System.Drawing.Point(30, 205)
        Me.txbx_custom_tags.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_custom_tags.Name = "txbx_custom_tags"
        Me.txbx_custom_tags.PlaceholderText = "Ex: blue_sky cloud 1girl"
        Me.txbx_custom_tags.Size = New System.Drawing.Size(554, 39)
        Me.txbx_custom_tags.TabIndex = 4
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(30, 166)
        Me.Label10.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(154, 32)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Custom Tags:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chbx_rating_explicit)
        Me.GroupBox1.Controls.Add(Me.chbx_rating_questionable)
        Me.GroupBox1.Controls.Add(Me.chbx_rating_safe)
        Me.GroupBox1.Location = New System.Drawing.Point(30, 34)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupBox1.Size = New System.Drawing.Size(457, 107)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Rating"
        '
        'chbx_rating_explicit
        '
        Me.chbx_rating_explicit.AutoSize = True
        Me.chbx_rating_explicit.Location = New System.Drawing.Point(323, 47)
        Me.chbx_rating_explicit.Margin = New System.Windows.Forms.Padding(6)
        Me.chbx_rating_explicit.Name = "chbx_rating_explicit"
        Me.chbx_rating_explicit.Size = New System.Drawing.Size(120, 36)
        Me.chbx_rating_explicit.TabIndex = 4
        Me.chbx_rating_explicit.Text = "Explicit"
        Me.chbx_rating_explicit.UseVisualStyleBackColor = True
        '
        'chbx_rating_questionable
        '
        Me.chbx_rating_questionable.AutoSize = True
        Me.chbx_rating_questionable.Location = New System.Drawing.Point(130, 47)
        Me.chbx_rating_questionable.Margin = New System.Windows.Forms.Padding(6)
        Me.chbx_rating_questionable.Name = "chbx_rating_questionable"
        Me.chbx_rating_questionable.Size = New System.Drawing.Size(188, 36)
        Me.chbx_rating_questionable.TabIndex = 4
        Me.chbx_rating_questionable.Text = "Questionable"
        Me.chbx_rating_questionable.UseVisualStyleBackColor = True
        '
        'chbx_rating_safe
        '
        Me.chbx_rating_safe.AutoSize = True
        Me.chbx_rating_safe.Location = New System.Drawing.Point(30, 47)
        Me.chbx_rating_safe.Margin = New System.Windows.Forms.Padding(6)
        Me.chbx_rating_safe.Name = "chbx_rating_safe"
        Me.chbx_rating_safe.Size = New System.Drawing.Size(92, 36)
        Me.chbx_rating_safe.TabIndex = 4
        Me.chbx_rating_safe.Text = "Safe"
        Me.chbx_rating_safe.UseVisualStyleBackColor = True
        '
        'tab_output
        '
        Me.tab_output.BackColor = System.Drawing.SystemColors.Control
        Me.tab_output.Controls.Add(Me.chbx_skip_obscured)
        Me.tab_output.Controls.Add(Me.txbx_slideshow_interval)
        Me.tab_output.Controls.Add(Me.Label13)
        Me.tab_output.Controls.Add(Me.Label14)
        Me.tab_output.Controls.Add(Me.Label12)
        Me.tab_output.Controls.Add(Me.Label11)
        Me.tab_output.Location = New System.Drawing.Point(8, 46)
        Me.tab_output.Margin = New System.Windows.Forms.Padding(6)
        Me.tab_output.Name = "tab_output"
        Me.tab_output.Size = New System.Drawing.Size(926, 637)
        Me.tab_output.TabIndex = 3
        Me.tab_output.Text = "Output"
        '
        'chbx_skip_obscured
        '
        Me.chbx_skip_obscured.AutoSize = True
        Me.chbx_skip_obscured.Checked = True
        Me.chbx_skip_obscured.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chbx_skip_obscured.Location = New System.Drawing.Point(45, 224)
        Me.chbx_skip_obscured.Margin = New System.Windows.Forms.Padding(6)
        Me.chbx_skip_obscured.Name = "chbx_skip_obscured"
        Me.chbx_skip_obscured.Size = New System.Drawing.Size(309, 36)
        Me.chbx_skip_obscured.TabIndex = 6
        Me.chbx_skip_obscured.Text = "Skip monitor if obscured"
        Me.chbx_skip_obscured.UseVisualStyleBackColor = True
        '
        'txbx_slideshow_interval
        '
        Me.txbx_slideshow_interval.Location = New System.Drawing.Point(45, 102)
        Me.txbx_slideshow_interval.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_slideshow_interval.Name = "txbx_slideshow_interval"
        Me.txbx_slideshow_interval.Size = New System.Drawing.Size(65, 39)
        Me.txbx_slideshow_interval.TabIndex = 5
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(30, 19)
        Me.Label13.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(121, 32)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Slideshow"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(119, 113)
        Me.Label14.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(114, 32)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "(seconds)"
        '
        'Label12
        '
        Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label12.Location = New System.Drawing.Point(30, 34)
        Me.Label12.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(884, 4)
        Me.Label12.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(45, 64)
        Me.Label11.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(98, 32)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Interval:"
        '
        'tab_files
        '
        Me.tab_files.BackColor = System.Drawing.SystemColors.Control
        Me.tab_files.Controls.Add(Me.txbx_max_history)
        Me.tab_files.Controls.Add(Me.txbx_dir_history)
        Me.tab_files.Controls.Add(Me.Label20)
        Me.tab_files.Controls.Add(Me.bt_dir_saved)
        Me.tab_files.Controls.Add(Me.bt_dir_history)
        Me.tab_files.Controls.Add(Me.txbx_dir_saved)
        Me.tab_files.Controls.Add(Me.Label19)
        Me.tab_files.Controls.Add(Me.Label18)
        Me.tab_files.Location = New System.Drawing.Point(8, 46)
        Me.tab_files.Margin = New System.Windows.Forms.Padding(6)
        Me.tab_files.Name = "tab_files"
        Me.tab_files.Size = New System.Drawing.Size(926, 637)
        Me.tab_files.TabIndex = 4
        Me.tab_files.Text = "Files"
        '
        'txbx_max_history
        '
        Me.txbx_max_history.Location = New System.Drawing.Point(15, 339)
        Me.txbx_max_history.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_max_history.Name = "txbx_max_history"
        Me.txbx_max_history.Size = New System.Drawing.Size(58, 39)
        Me.txbx_max_history.TabIndex = 6
        '
        'txbx_dir_history
        '
        Me.txbx_dir_history.Location = New System.Drawing.Point(15, 66)
        Me.txbx_dir_history.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_dir_history.Name = "txbx_dir_history"
        Me.txbx_dir_history.Size = New System.Drawing.Size(598, 39)
        Me.txbx_dir_history.TabIndex = 0
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(15, 301)
        Me.Label20.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(138, 32)
        Me.Label20.TabIndex = 7
        Me.Label20.Text = "Max history"
        '
        'bt_dir_saved
        '
        Me.bt_dir_saved.Location = New System.Drawing.Point(628, 188)
        Me.bt_dir_saved.Margin = New System.Windows.Forms.Padding(6)
        Me.bt_dir_saved.Name = "bt_dir_saved"
        Me.bt_dir_saved.Size = New System.Drawing.Size(139, 49)
        Me.bt_dir_saved.TabIndex = 5
        Me.bt_dir_saved.Text = "Change..."
        Me.bt_dir_saved.UseVisualStyleBackColor = True
        '
        'bt_dir_history
        '
        Me.bt_dir_history.Location = New System.Drawing.Point(628, 66)
        Me.bt_dir_history.Margin = New System.Windows.Forms.Padding(6)
        Me.bt_dir_history.Name = "bt_dir_history"
        Me.bt_dir_history.Size = New System.Drawing.Size(139, 49)
        Me.bt_dir_history.TabIndex = 4
        Me.bt_dir_history.Text = "Change..."
        Me.bt_dir_history.UseVisualStyleBackColor = True
        '
        'txbx_dir_saved
        '
        Me.txbx_dir_saved.Location = New System.Drawing.Point(15, 188)
        Me.txbx_dir_saved.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_dir_saved.Name = "txbx_dir_saved"
        Me.txbx_dir_saved.Size = New System.Drawing.Size(598, 39)
        Me.txbx_dir_saved.TabIndex = 3
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(15, 149)
        Me.Label19.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(161, 32)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "Saved Images"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(15, 28)
        Me.Label18.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(89, 32)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "History"
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.chbo_context_menu_cascaded)
        Me.TabPage1.Controls.Add(Me.chbo_desktop_context_menu)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Location = New System.Drawing.Point(8, 46)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(6)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(926, 637)
        Me.TabPage1.TabIndex = 5
        Me.TabPage1.Text = "Hotkeys"
        '
        'chbo_context_menu_cascaded
        '
        Me.chbo_context_menu_cascaded.AutoSize = True
        Me.chbo_context_menu_cascaded.Location = New System.Drawing.Point(91, 574)
        Me.chbo_context_menu_cascaded.Margin = New System.Windows.Forms.Padding(6)
        Me.chbo_context_menu_cascaded.Name = "chbo_context_menu_cascaded"
        Me.chbo_context_menu_cascaded.Size = New System.Drawing.Size(307, 36)
        Me.chbo_context_menu_cascaded.TabIndex = 7
        Me.chbo_context_menu_cascaded.Text = "Cascaded Context Menu"
        Me.chbo_context_menu_cascaded.UseVisualStyleBackColor = True
        '
        'chbo_desktop_context_menu
        '
        Me.chbo_desktop_context_menu.AutoSize = True
        Me.chbo_desktop_context_menu.Location = New System.Drawing.Point(46, 521)
        Me.chbo_desktop_context_menu.Margin = New System.Windows.Forms.Padding(6)
        Me.chbo_desktop_context_menu.Name = "chbo_desktop_context_menu"
        Me.chbo_desktop_context_menu.Size = New System.Drawing.Size(466, 36)
        Me.chbo_desktop_context_menu.TabIndex = 5
        Me.chbo_desktop_context_menu.Text = "Add Hotkeys to Desktop Context Menu"
        Me.chbo_desktop_context_menu.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txbx_hk_open_key)
        Me.GroupBox3.Controls.Add(Me.Label24)
        Me.GroupBox3.Controls.Add(Me.cobo_hk_open_modifier)
        Me.GroupBox3.Controls.Add(Me.Label25)
        Me.GroupBox3.Controls.Add(Me.Label26)
        Me.GroupBox3.Location = New System.Drawing.Point(397, 6)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupBox3.Size = New System.Drawing.Size(371, 149)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Open Current Image"
        '
        'txbx_hk_open_key
        '
        Me.txbx_hk_open_key.Enabled = False
        Me.txbx_hk_open_key.Location = New System.Drawing.Point(219, 81)
        Me.txbx_hk_open_key.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_hk_open_key.Name = "txbx_hk_open_key"
        Me.txbx_hk_open_key.Size = New System.Drawing.Size(93, 39)
        Me.txbx_hk_open_key.TabIndex = 2
        Me.txbx_hk_open_key.Text = "O"
        Me.txbx_hk_open_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(180, 87)
        Me.Label24.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(30, 32)
        Me.Label24.TabIndex = 4
        Me.Label24.Text = "+"
        '
        'cobo_hk_open_modifier
        '
        Me.cobo_hk_open_modifier.AutoCompleteCustomSource.AddRange(New String() {"Ctrl", "Alt", "Shift"})
        Me.cobo_hk_open_modifier.Enabled = False
        Me.cobo_hk_open_modifier.FormattingEnabled = True
        Me.cobo_hk_open_modifier.Items.AddRange(New Object() {"Alt"})
        Me.cobo_hk_open_modifier.Location = New System.Drawing.Point(32, 81)
        Me.cobo_hk_open_modifier.Margin = New System.Windows.Forms.Padding(6)
        Me.cobo_hk_open_modifier.Name = "cobo_hk_open_modifier"
        Me.cobo_hk_open_modifier.Size = New System.Drawing.Size(134, 40)
        Me.cobo_hk_open_modifier.TabIndex = 0
        Me.cobo_hk_open_modifier.Text = "Alt"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(219, 43)
        Me.Label25.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(53, 32)
        Me.Label25.TabIndex = 3
        Me.Label25.Text = "Key"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(32, 43)
        Me.Label26.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(105, 32)
        Me.Label26.TabIndex = 1
        Me.Label26.Text = "Modifier"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txbx_hk_save_key)
        Me.GroupBox2.Controls.Add(Me.Label23)
        Me.GroupBox2.Controls.Add(Me.cobo_hk_save_modifier)
        Me.GroupBox2.Controls.Add(Me.Label22)
        Me.GroupBox2.Controls.Add(Me.Label21)
        Me.GroupBox2.Location = New System.Drawing.Point(15, 6)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupBox2.Size = New System.Drawing.Size(371, 149)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Save Current Image"
        '
        'txbx_hk_save_key
        '
        Me.txbx_hk_save_key.Enabled = False
        Me.txbx_hk_save_key.Location = New System.Drawing.Point(219, 81)
        Me.txbx_hk_save_key.Margin = New System.Windows.Forms.Padding(6)
        Me.txbx_hk_save_key.Name = "txbx_hk_save_key"
        Me.txbx_hk_save_key.Size = New System.Drawing.Size(93, 39)
        Me.txbx_hk_save_key.TabIndex = 2
        Me.txbx_hk_save_key.Text = "S"
        Me.txbx_hk_save_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(180, 87)
        Me.Label23.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(30, 32)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "+"
        '
        'cobo_hk_save_modifier
        '
        Me.cobo_hk_save_modifier.AutoCompleteCustomSource.AddRange(New String() {"Ctrl", "Alt", "Shift"})
        Me.cobo_hk_save_modifier.Enabled = False
        Me.cobo_hk_save_modifier.FormattingEnabled = True
        Me.cobo_hk_save_modifier.Items.AddRange(New Object() {"Alt"})
        Me.cobo_hk_save_modifier.Location = New System.Drawing.Point(32, 81)
        Me.cobo_hk_save_modifier.Margin = New System.Windows.Forms.Padding(6)
        Me.cobo_hk_save_modifier.Name = "cobo_hk_save_modifier"
        Me.cobo_hk_save_modifier.Size = New System.Drawing.Size(134, 40)
        Me.cobo_hk_save_modifier.TabIndex = 0
        Me.cobo_hk_save_modifier.Text = "Alt"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(219, 43)
        Me.Label22.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(53, 32)
        Me.Label22.TabIndex = 3
        Me.Label22.Text = "Key"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(32, 43)
        Me.Label21.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(105, 32)
        Me.Label21.TabIndex = 1
        Me.Label21.Text = "Modifier"
        '
        'Settings
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(13.0!, 32.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLight
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(942, 774)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
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
    Friend WithEvents cobo_hk_save_modifier As Windows.Forms.ComboBox
    Friend WithEvents Label22 As Windows.Forms.Label
    Friend WithEvents txbx_hk_save_key As Windows.Forms.TextBox
    Friend WithEvents Label21 As Windows.Forms.Label
    Friend WithEvents Label23 As Windows.Forms.Label
    Friend WithEvents chbo_desktop_context_menu As Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As Windows.Forms.GroupBox
    Friend WithEvents Label24 As Windows.Forms.Label
    Friend WithEvents cobo_hk_open_modifier As Windows.Forms.ComboBox
    Friend WithEvents Label25 As Windows.Forms.Label
    Friend WithEvents txbx_hk_open_key As Windows.Forms.TextBox
    Friend WithEvents Label26 As Windows.Forms.Label
    Friend WithEvents chbo_context_menu_cascaded As Windows.Forms.CheckBox
    Friend WithEvents chbx_skip_obscured As Windows.Forms.CheckBox
End Class
