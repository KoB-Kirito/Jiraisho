Imports Newtonsoft.Json

Public Class Registry
    Private Shared PATH_EXE As String = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName
    Private Shared MAIN_KEY As String = "Software\" & AppName
    Private Const NEXT_ID As String = "nextId"
    Private Const NEXT_ORIGINAL As String = "nextOriginal"
    Private Const NEXT_WALLPAPER As String = "nextWallpaper"
    Private Const NEXT_POST_URL As String = "nextPostUrl"
    Private Const NEXT_FILE_URL As String = "nextFileUrl"
    Private Const CURR_ID As String = "currId"
    Private Const CURR_ORIGINAL As String = "currOriginal"
    Private Const CURR_WALLPAPER As String = "currWallpaper"
    Private Const CURR_POST_URL As String = "currPostUrl"
    Private Const CURR_FILE_URL As String = "currFileUrl"
    Private Const LAST_ID As String = "lastId"
    Private Const LAST_ORIGINAL As String = "lastOriginal"
    Private Const LAST_WALLPAPER As String = "lastWallpaper"
    Private Const LAST_POST_URL As String = "lastPostUrl"
    Private Const LAST_FILE_URL As String = "lastFileUrl"

    Public Shared Sub SetValue(name As String, value As Object, Optional valueKind As Microsoft.Win32.RegistryValueKind = Microsoft.Win32.RegistryValueKind.String)
        Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
        key.SetValue(name, value, valueKind)
        key.Flush()
    End Sub

    Public Shared Function GetValue(name As String) As Object
        Dim key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(MAIN_KEY, False)
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

    Public Shared Sub UpdateMonitor(Monitor As Monitor)
        Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
        Dim mKey = key.CreateSubKey(Monitor.DeviceName.Substring(4))
        mKey.SetValue("Width", Monitor.Rectangle.Width, Microsoft.Win32.RegistryValueKind.DWord)
        mKey.SetValue("Height", Monitor.Rectangle.Height, Microsoft.Win32.RegistryValueKind.DWord)
    End Sub

    Public Shared Function GetNextWallpaperPathFor(Monitor As Monitor) As String
        Try
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
            Dim mKey = key.CreateSubKey(Monitor.DeviceName.Substring(4), False)
            Return mKey.GetValue(NEXT_WALLPAPER)
        Catch ex As Exception
            Log(LogLvl.Warning, "Accessing registry failed", ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function GetMonitorInfoFor(Monitor As Monitor) As MonitorInfo
        Return GetMonitorInfoFor(Monitor.DeviceName)
    End Function

    Public Shared Function GetMonitorInfoFor(MonitorName As String) As MonitorInfo
        Try
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
            Dim mKey = key.CreateSubKey(MonitorName.Substring(4), False)

            Return New MonitorInfo With
                {
                    .Width = mKey.GetValue("Width"),
                    .Heigh = mKey.GetValue("Height"),
 _
                    .LastId = mKey.GetValue(LAST_ID),
                    .LastOriginal = mKey.GetValue(LAST_ORIGINAL),
                    .LastWallpaper = mKey.GetValue(LAST_WALLPAPER),
                    .LastPostUrl = mKey.GetValue(LAST_POST_URL),
                    .LastFileUrl = mKey.GetValue(LAST_FILE_URL),
 _
                    .CurrId = mKey.GetValue(CURR_ID),
                    .CurrOriginal = mKey.GetValue(CURR_ORIGINAL),
                    .CurrWallpaper = mKey.GetValue(CURR_WALLPAPER),
                    .CurrPostUrl = mKey.GetValue(CURR_POST_URL),
                    .CurrFileUrl = mKey.GetValue(CURR_FILE_URL),
 _
                    .NextId = mKey.GetValue(NEXT_ID),
                    .NextOriginal = mKey.GetValue(NEXT_ORIGINAL),
                    .NextWallpaper = mKey.GetValue(NEXT_WALLPAPER),
                    .NextPostUrl = mKey.GetValue(NEXT_POST_URL),
                    .NextFileUrl = mKey.GetValue(NEXT_FILE_URL)
                }
        Catch ex As Exception
            Log(LogLvl.Warning, "Failed to get monitor info", ex)
            Return Nothing
        End Try
    End Function

    Public Shared Sub ShiftWallpapersFor(Monitor As Monitor)
        Try
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
            Dim mKey = key.CreateSubKey(Monitor.DeviceName.Substring(4))

            'Set last = current
            mKey.SetValue(LAST_ID, If(mKey.GetValue(CURR_ID), 0), Microsoft.Win32.RegistryValueKind.DWord)
            mKey.SetValue(LAST_ORIGINAL, If(mKey.GetValue(CURR_ORIGINAL), ""), Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(LAST_WALLPAPER, If(mKey.GetValue(CURR_WALLPAPER), ""), Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(LAST_POST_URL, If(mKey.GetValue(CURR_POST_URL), ""), Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(LAST_FILE_URL, If(mKey.GetValue(CURR_FILE_URL), ""), Microsoft.Win32.RegistryValueKind.String)

            'Set current = next
            mKey.SetValue(CURR_ID, If(mKey.GetValue(NEXT_ID), 0), Microsoft.Win32.RegistryValueKind.DWord)
            mKey.SetValue(CURR_ORIGINAL, If(mKey.GetValue(NEXT_ORIGINAL), ""), Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(CURR_WALLPAPER, If(mKey.GetValue(NEXT_WALLPAPER), ""), Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(CURR_POST_URL, If(mKey.GetValue(NEXT_POST_URL), ""), Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(CURR_FILE_URL, If(mKey.GetValue(NEXT_FILE_URL), ""), Microsoft.Win32.RegistryValueKind.String)

            'Set next = null
            mKey.SetValue(NEXT_ID, 0, Microsoft.Win32.RegistryValueKind.DWord)
            mKey.SetValue(NEXT_ORIGINAL, "", Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(NEXT_WALLPAPER, "", Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(NEXT_POST_URL, "", Microsoft.Win32.RegistryValueKind.String)
            mKey.SetValue(NEXT_FILE_URL, "", Microsoft.Win32.RegistryValueKind.String)

            mKey.Flush()
        Catch ex As Exception
            Log(LogLvl.Debug, "Failed to access registry", ex)
            Log(LogLvl.Error, "Failed to access registry")
        End Try
    End Sub

    Public Shared Sub SetNextWallpaperFor(Monitor As Monitor, Post As BooruSharp.Search.Post.SearchResult, OriginalPath As String, EditedPath As String)
        Try
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
            Dim monKey = key.CreateSubKey(Monitor.DeviceName.Substring(4))

            monKey.SetValue(NEXT_ID, Post.id, Microsoft.Win32.RegistryValueKind.DWord)
            monKey.SetValue(NEXT_ORIGINAL, OriginalPath, Microsoft.Win32.RegistryValueKind.String)
            monKey.SetValue(NEXT_WALLPAPER, EditedPath, Microsoft.Win32.RegistryValueKind.String)
            monKey.SetValue(NEXT_POST_URL, Post.postUrl.AbsoluteUri, Microsoft.Win32.RegistryValueKind.String)
            monKey.SetValue(NEXT_FILE_URL, Post.fileUrl.AbsoluteUri, Microsoft.Win32.RegistryValueKind.String)

            monKey.Flush()
        Catch ex As Exception
            Log(LogLvl.Debug, "Failed to access registry", ex)
            Log(LogLvl.Error, "Failed to access registry")
        End Try
    End Sub

    Public Shared Sub AddFavourite(Source As String, Id As Integer)
        Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
        key.CreateSubKey(Source & "\Favourites\" & Id).Flush()
    End Sub

    Public Shared Sub AddDislike(Source As String, Id As Integer)
        Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
        key.CreateSubKey(Source & "\Dislikes\" & Id).Flush()
    End Sub

    Public Shared Sub SetRefreshToken(Token As String)
        'If CFG.Source <> "Pixiv" Then Throw New Exception("Only pixiv supports a refreshToken")

        Try
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
            key.CreateSubKey(CFG.Source)
            key.SetValue("refreshToken", Token, Microsoft.Win32.RegistryValueKind.String)
        Catch ex As Exception
            Log(LogLvl.Error, "Could not write refreshToken to registry", ex)
        End Try
    End Sub

    Public Shared Function GetRefreshToken(Source As String) As String
        If Source <> "Pixiv" Then Throw New Exception("Only pixiv supports a refreshToken")

        Try
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
            key.OpenSubKey(CFG.Source, False)
            Return key.GetValue("refreshToken")
        Catch ex As Exception
            Log(LogLvl.Warning, "Failed to get refreshToken", ex)
            Return Nothing
        End Try
    End Function



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

            'Fav last
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\favlast")
            key.SetValue("MUIVerb", "Add last to favourites", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\favlast\command")
            key.SetValue("", AppName & ".exe cmt favlast")

            'Save current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\savecurrent")
            key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\savecurrent\command")
            key.SetValue("", AppName & ".exe cmt save")

            'Save last
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\savelast")
            key.SetValue("MUIVerb", "Save last to disk", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\savelast\command")
            key.SetValue("", AppName & ".exe cmt savelast")

            'Open current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\opencurrent")
            key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\opencurrent\command")
            key.SetValue("", AppName & ".exe cmt open")

            'Open current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\openlast")
            key.SetValue("MUIVerb", "Open last in browser", Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\openlast\command")
            key.SetValue("", AppName & ".exe cmt openlast")

        Catch ex As Exception
            Log(LogLvl.Error, "Can't access the registry to add context menu to desktop")
        End Try
    End Sub

    Public Shared Sub DeleteCascadedContextMenu()
        Try
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ccmKey)
        Catch ex As Exception
            Log(LogLvl.Warning, "Could not delete cascaded context menu: " & ex.Message)
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
            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String) 'ToDo: check order
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-fav\command")
            key.SetValue("", AppName & ".exe cmt fav")

            'Fav last
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-favlast")
            key.SetValue("MUIVerb", "Add last to favourites", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-favlast\command")
            key.SetValue("", AppName & ".exe cmt favlast")

            'Save current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-save")
            key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-save\command")
            key.SetValue("", AppName & ".exe cmt save")

            'Save last
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-savelast")
            key.SetValue("MUIVerb", "Save last to disk", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-savelast\command")
            key.SetValue("", AppName & ".exe cmt savelast")

            'Open current
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-open")
            key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-open\command")
            key.SetValue("", AppName & ".exe cmt open")

            'Open last
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-openlast")
            key.SetValue("MUIVerb", "Open last in browser", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
            key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-openlast\command")
            key.SetValue("", AppName & ".exe cmt openlast")

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
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ccmKey & "-favlast")
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ccmKey & "-savelast")
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ccmKey & "-openlast")
        Catch ex As Exception
            Log(LogLvl.Warning, "Could not delete normal context menu: " & ex.Message)
        End Try
    End Sub

#End Region

End Class

<JsonConverter(GetType(Converters.StringEnumConverter))>
Public Enum ContextMenuType As Integer
    None = 0
    Normal = 1
    Cascaded = 2
End Enum

Public Structure MonitorInfo
    Public Width As Integer
    Public Heigh As Integer

    Public LastId As Integer
    Public LastOriginal As String
    Public LastWallpaper As String
    Public LastPostUrl As String
    Public LastFileUrl As String

    Public CurrId As Integer
    Public CurrOriginal As String
    Public CurrWallpaper As String
    Public CurrPostUrl As String
    Public CurrFileUrl As String

    Public NextId As Integer
    Public NextOriginal As String
    Public NextWallpaper As String
    Public NextPostUrl As String
    Public NextFileUrl As String
End Structure