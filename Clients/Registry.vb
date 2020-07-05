Public Class Registry
    Private Shared path As String = "Software\" & AppName

    Public Shared Sub SetValue(name As String, value As Object, Optional valueKind As Microsoft.Win32.RegistryValueKind = Microsoft.Win32.RegistryValueKind.String)
        Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path)
        key.SetValue(name, value, valueKind)
        key.Flush()
    End Sub

    Public Shared Function GetValue(name As String) As Object
        Dim key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path, False)
        If key Is Nothing Then
            Return Nothing
        Else
            Return key.GetValue(name, Nothing)
        End If
    End Function

    Public Shared Sub UpdateAppPath()
        Dim key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey($"Software\Microsoft\Windows\CurrentVersion\App Paths\{AppName}.exe", False)
        If key Is Nothing OrElse key.GetValue("") <> PATH_EXE Then
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey($"Software\Microsoft\Windows\CurrentVersion\App Paths\{AppName}.exe")
            key.SetValue("", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
        End If
    End Sub

#Region "Context Menus"

    Private Shared ccmKey = "Software\Classes\DesktopBackground\Shell\" & AppName

    Public Shared Sub CreateCascadedContextMenu()
        'Add cascaded context menu
        Try
            'Main SubKey
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey)
            'Enable sub entries
            key.SetValue("ExtendedSubCommandsKey", "DesktopBackground\Shell\" & AppName, Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("MUIVerb", AppName, Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)

            'Fav current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\favcurrent")
            key.SetValue("MUIVerb", "Add to favourites", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\favcurrent\command")
            key.SetValue("", AppName & ".exe cmt fav")

            'Save current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\savecurrent")
            key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\savecurrent\command")
            key.SetValue("", AppName & ".exe cmt save")

            'Open current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\opencurrent")
            key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\opencurrent\command")
            key.SetValue("", AppName & ".exe cmt open")

        Catch ex As Exception
            Log(LogLvl.Error, "Can't access the registry to add context menu to desktop")
        End Try
    End Sub

    Public Shared Sub DeleteCascadedContextMenu()
        Try
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ccmKey)
        Catch ex As Exception
            Log(LogLvl.Warning, "Could not delete cascaded context menu", ex)
        End Try
    End Sub

    Public Shared Sub CreateContextMenu()
        'Add normal context menu
        Try
            'Fav current
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-fav")
            key.SetValue("MUIVerb", "Add to favourites", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-fav\command")
            key.SetValue("", AppName & ".exe cmt fav")

            'Save current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-save")
            key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-save\command")
            key.SetValue("", AppName & ".exe cmt save")

            'Open current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-open")
            key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-open\command")
            key.SetValue("", AppName & ".exe cmt open")

        Catch ex As Exception
            Log(LogLvl.Error, "Can't access the registry to add context menu to desktop")
        End Try
    End Sub

    Public Shared Sub DeleteContextMenu()
        'Delete normal context menu
        Try
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ccmKey & "-fav")
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ccmKey & "-save")
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ccmKey & "-open")
        Catch ex As Exception
            Log(LogLvl.Warning, "Could not delete normal context menu", ex)
        End Try
    End Sub

#End Region

End Class
