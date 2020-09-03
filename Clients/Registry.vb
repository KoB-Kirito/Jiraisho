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
            Dim sKey = key.CreateSubKey(CFG.Source)
            sKey.SetValue("refreshToken", Token, Microsoft.Win32.RegistryValueKind.String)
        Catch ex As Exception
            Log(LogLvl.Error, "Could not write refreshToken to registry", ex)
        End Try
    End Sub

    Public Shared Function GetRefreshToken(Source As String) As String
        If Source <> "Pixiv" Then Throw New Exception("Only pixiv supports a refreshToken")

        Try
            Dim key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(MAIN_KEY)
            Dim skey = key.OpenSubKey(CFG.Source, False)
            Return skey.GetValue("refreshToken")
        Catch ex As Exception
            Log(LogLvl.Warning, "Failed to get refreshToken", ex)
            Return Nothing
        End Try
    End Function



#Region "Context Menus"

    Private Shared ReadOnly ccmKey As String = "Software\Classes\DesktopBackground\Shell\" & AppName

    Public Shared Sub CreateContextMenu(Type As ContextMenuType, Optional Fav As Boolean = True,
                                        Optional Dislike As Boolean = True, Optional Save As Boolean = True,
                                        Optional Open As Boolean = True, Optional FavLast As Boolean = True,
                                        Optional DislikeLast As Boolean = True, Optional SaveLast As Boolean = True,
                                        Optional OpenLast As Boolean = True)

        Dim topLevelSeparatorSet As Boolean
        Dim key As Microsoft.Win32.RegistryKey

        Try
            Select Case Type
                Case ContextMenuType.Direct
                    'Ordering is reversed in top level

                    'Open last
                    If OpenLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-openlast")
                        key.SetValue("MUIVerb", "Open last in browser", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                        topLevelSeparatorSet = True
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-openlast\command")
                        key.SetValue("", AppName & ".exe cmt openlast")
                    End If

                    'Save last
                    If SaveLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-savelast")
                        key.SetValue("MUIVerb", "Save last to disk", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then
                            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                            topLevelSeparatorSet = True
                        End If
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-savelast\command")
                        key.SetValue("", AppName & ".exe cmt savelast")
                    End If

                    'Dislike last
                    If DislikeLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-3-dislikelast")
                        key.SetValue("MUIVerb", "Don't show last again", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then
                            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                            topLevelSeparatorSet = True
                        End If
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-3-dislikelast\command")
                        key.SetValue("", AppName & ".exe cmt dislikelast")
                    End If

                    'Fav last
                    If FavLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-4-favlast")
                        key.SetValue("MUIVerb", "Add last to favourites", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then
                            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                            topLevelSeparatorSet = True
                        End If
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-4-favlast\command")
                        key.SetValue("", AppName & ".exe cmt favlast")
                    End If

                    'Add another separator beetween
                    topLevelSeparatorSet = False

                    'Open current
                    If Open Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-5-open")
                        key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then
                            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                            topLevelSeparatorSet = True
                        End If
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-5-open\command")
                        key.SetValue("", AppName & ".exe cmt open")
                    End If

                    'Save current
                    If Save Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-6-save")
                        key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then
                            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                            topLevelSeparatorSet = True
                        End If
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-6-save\command")
                        key.SetValue("", AppName & ".exe cmt save")
                    End If

                    'Dislike current
                    If Dislike Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-7-dislike")
                        key.SetValue("MUIVerb", "Don't show again", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then
                            key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                            topLevelSeparatorSet = True
                        End If
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-7-dislike\command")
                        key.SetValue("", AppName & ".exe cmt dislike")
                    End If

                    'Fav current
                    If Fav Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-8-fav")
                        key.SetValue("MUIVerb", "Add to favourites", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-8-fav\command")
                        key.SetValue("", AppName & ".exe cmt fav")
                    End If

                Case ContextMenuType.Cascaded
                    'Jiraisho
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey)
                    key.SetValue("ExtendedSubCommandsKey", "DesktopBackground\Shell\" & AppName, Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("MUIVerb", AppName, Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)

                    'Jiraisho -> Open current
                    If Open Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\4open")
                        key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("CommandFlags", &H40, Microsoft.Win32.RegistryValueKind.DWord)
                        topLevelSeparatorSet = True
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\4open\command")
                        key.SetValue("", AppName & ".exe cmt open")
                    End If

                    'Jiraisho -> Save current
                    If Save Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\3save")
                        key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then
                            key.SetValue("CommandFlags", &H40, Microsoft.Win32.RegistryValueKind.DWord)
                            topLevelSeparatorSet = True
                        End If
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\3save\command")
                        key.SetValue("", AppName & ".exe cmt save")
                    End If

                    'Jiraisho -> Dislike current
                    If Dislike Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\2dislike")
                        key.SetValue("MUIVerb", "Don't show again", Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then
                            key.SetValue("CommandFlags", &H40, Microsoft.Win32.RegistryValueKind.DWord)
                            topLevelSeparatorSet = True
                        End If
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\2dislike\command")
                        key.SetValue("", AppName & ".exe cmt dislike")
                    End If

                    'Jiraisho -> Fav current
                    If Fav Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\1fav")
                        key.SetValue("MUIVerb", "Add to favourites", Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then key.SetValue("CommandFlags", &H40, Microsoft.Win32.RegistryValueKind.DWord)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\1fav\command")
                        key.SetValue("", AppName & ".exe cmt fav")
                    End If

                    'Jiraisho -> Fav last
                    If FavLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\5favlast")
                        key.SetValue("MUIVerb", "Add last to favourites", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\5favlast\command")
                        key.SetValue("", AppName & ".exe cmt favlast")
                    End If

                    'Jiraisho -> Dislike last
                    If DislikeLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\6dislikelast")
                        key.SetValue("MUIVerb", "Don't show last again", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\6dislikelast\command")
                        key.SetValue("", AppName & ".exe cmt dislikelast")
                    End If

                    'Jiraisho -> Save last
                    If SaveLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\7savelast")
                        key.SetValue("MUIVerb", "Save last to disk", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\7savelast\command")
                        key.SetValue("", AppName & ".exe cmt savelast")
                    End If

                    'Jiraisho -> Open last
                    If OpenLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\8openlast")
                        key.SetValue("MUIVerb", "Open last in browser", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\8openlast\command")
                        key.SetValue("", AppName & ".exe cmt openlast")
                    End If

                Case ContextMenuType.Grouped
                    'Last first, to be able to set separator right
                    'Last
                    If FavLast OrElse DislikeLast OrElse SaveLast OrElse OpenLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last") 'Ordering is reversed in top level
                        key.SetValue("ExtendedSubCommandsKey", "DesktopBackground\Shell\" & AppName & "-1-last", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("MUIVerb", "Last", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                        topLevelSeparatorSet = True
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                    End If

                    'Last -> Fav
                    If FavLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last\shell\1-fav")
                        key.SetValue("MUIVerb", "Add to favourites", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last\shell\1-fav\command")
                        key.SetValue("", AppName & ".exe cmt favlast")
                    End If

                    'Last -> Dislike
                    If DislikeLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last\shell\2-dislike")
                        key.SetValue("MUIVerb", "Don't show again", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last\shell\2-dislike\command")
                        key.SetValue("", AppName & ".exe cmt dislikelast")
                    End If

                    'Last -> Save
                    If SaveLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last\shell\3-save")
                        key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last\shell\3-save\command")
                        key.SetValue("", AppName & ".exe cmt savelast")
                    End If

                    'Last -> Open
                    If OpenLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last\shell\4-open")
                        key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-1-last\shell\4-open\command")
                        key.SetValue("", AppName & ".exe cmt openlast")
                    End If


                    'Current
                    If Fav OrElse Dislike OrElse Save OrElse Open Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current")
                        key.SetValue("ExtendedSubCommandsKey", "DesktopBackground\Shell\" & AppName & "-2-current", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("MUIVerb", "Current", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                        If Not topLevelSeparatorSet Then key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)
                    End If

                    'Current -> Fav
                    If Fav Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current\shell\1-fav")
                        key.SetValue("MUIVerb", "Add to favourites", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current\shell\1-fav\command")
                        key.SetValue("", AppName & ".exe cmt fav")
                    End If

                    'Current -> Dislike
                    If Dislike Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current\shell\2-dislike")
                        key.SetValue("MUIVerb", "Don't show again", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current\shell\2-dislike\command")
                        key.SetValue("", AppName & ".exe cmt dislike")
                    End If

                    'Current -> Save
                    If Save Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current\shell\3-save")
                        key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current\shell\3-save\command")
                        key.SetValue("", AppName & ".exe cmt save")
                    End If

                    'Current -> Open
                    If Open Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current\shell\4-open")
                        key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "-2-current\shell\4-open\command")
                        key.SetValue("", AppName & ".exe cmt open")
                    End If

                Case ContextMenuType.CascadedGrouped
                    Log(LogLvl.Trace, "Creating cascaded grouped context menu")

                    'Jiraisho
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey)
                    key.SetValue("ExtendedSubCommandsKey", "DesktopBackground\Shell\" & AppName, Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("MUIVerb", AppName, Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("Position", "Top", Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("SeparatorAfter", "", Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("Icon", PATH_EXE, Microsoft.Win32.RegistryValueKind.String)


                    'Jiraisho -> Current
                    If Fav OrElse Dislike OrElse Save OrElse Open Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current")
                        key.SetValue("MUIVerb", "Current", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("ExtendedSubCommandsKey", $"DesktopBackground\Shell\{AppName}\shell\current", Microsoft.Win32.RegistryValueKind.String)
                    End If

                    'Jiraisho -> Current -> Fav
                    If Fav Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current\shell\1-fav")
                        key.SetValue("MUIVerb", "Add to favourites", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current\shell\1-fav\command")
                        key.SetValue("", AppName & ".exe cmt fav")
                    End If

                    'Jiraisho -> Current -> Dislike
                    If Dislike Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current\shell\2-dislike")
                        key.SetValue("MUIVerb", "Don't show again", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current\shell\2-dislike\command")
                        key.SetValue("", AppName & ".exe cmt dislike")
                    End If

                    'Jiraisho -> Current -> Save
                    If Save Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current\shell\3-save")
                        key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current\shell\3-save\command")
                        key.SetValue("", AppName & ".exe cmt save")
                    End If

                    'Jiraisho -> Current -> Open
                    If Open Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current\shell\4-open")
                        key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\current\shell\4-open\command")
                        key.SetValue("", AppName & ".exe cmt open")
                    End If


                    'Jiraisho -> Last
                    If FavLast OrElse DislikeLast OrElse SaveLast OrElse OpenLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last")
                        key.SetValue("MUIVerb", "Last", Microsoft.Win32.RegistryValueKind.String)
                        key.SetValue("ExtendedSubCommandsKey", $"DesktopBackground\Shell\{AppName}\shell\last", Microsoft.Win32.RegistryValueKind.String)
                    End If

                    'Jiraisho -> Last -> Fav
                    If FavLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last\shell\1-fav")
                        key.SetValue("MUIVerb", "Add to favourites", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last\shell\1-fav\command")
                        key.SetValue("", AppName & ".exe cmt favlast")
                    End If

                    'Jiraisho -> Last -> Dislike
                    If DislikeLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last\shell\2-dislike")
                        key.SetValue("MUIVerb", "Don't show again", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last\shell\2-dislike\command")
                        key.SetValue("", AppName & ".exe cmt dislikelast")
                    End If

                    'Jiraisho -> Last -> Save
                    If SaveLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last\shell\3-save")
                        key.SetValue("MUIVerb", "Save to disk", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last\shell\3-save\command")
                        key.SetValue("", AppName & ".exe cmt savelast")
                    End If

                    'Jiraisho -> Last -> Open
                    If OpenLast Then
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last\shell\4-open")
                        key.SetValue("MUIVerb", "Open in browser", Microsoft.Win32.RegistryValueKind.String)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ccmKey & "\shell\last\shell\4-open\command")
                        key.SetValue("", AppName & ".exe cmt openlast")
                    End If

                Case Else
                    Throw New Exception($"Unknown type ({Type})")

            End Select

        Catch ex As Exception
            Log(LogLvl.Error, "Can't access the registry to add context menu to desktop", ex)
        End Try
    End Sub

    Public Shared Sub DeleteContextMenu()
        Log(LogLvl.Trace, "Deleting context menu")

        Dim key As Microsoft.Win32.RegistryKey
        Try
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\Classes\DesktopBackground\Shell\")
        Catch ex As Exception
            Log(LogLvl.Error, "Could not get or create registry key", ex)
            Return
        End Try

        Dim names As String()
        Try
            names = key.GetSubKeyNames()
        Catch ex As Exception
            Log(LogLvl.Error, "Could not get subkey names", ex)
            Return
        End Try

        For Each name In names
            Log(LogLvl.Trace, name)
            If name.Contains(AppName) Then
                Try
                    Log(LogLvl.Debug, "Deleting " & name)
                    key.DeleteSubKeyTree(name)
                Catch ex As Exception
                    Log(LogLvl.Error, "Can't delete registry key " & name, ex)
                End Try
            End If
        Next

        Log(LogLvl.Debug, "Context menu deleted")
    End Sub

#End Region

End Class

<JsonConverter(GetType(Converters.StringEnumConverter))>
Public Enum ContextMenuType As Integer
    None = 0
    Direct = 1
    Cascaded = 2
    Grouped = 3
    CascadedGrouped = 4
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