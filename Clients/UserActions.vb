Imports System.Windows.Forms

Public Class UserActions
    Public Shared Async Function FavWallpaper(screen As Screen, Optional last As Boolean = False) As Task
        Dim monitorInfo = Registry.GetMonitorInfoFor(screen.DeviceName)
        If last Then
            Await Downloader.AddFavouriteAsync(monitorInfo.LastId) 'ToDo: implement
        Else
            Await Downloader.AddFavouriteAsync(monitorInfo.CurrId) 'ToDo: implement
        End If
    End Function

    Public Shared Sub SaveWallpaper(screen As Screen, Optional last As Boolean = False)
        Dim dirSaved = Registry.GetValue("DirSaved")
        If dirSaved Is Nothing Then
            Log(LogLvl.Error, "Can't parse directory for saved images")
            Return
        End If
        Dim monitorInfo = Registry.GetMonitorInfoFor(screen.DeviceName)
        Dim path As String = ""
        If last Then
            path = monitorInfo.LastOriginal
            If String.IsNullOrWhiteSpace(path) Then path = monitorInfo.LastWallpaper
        Else
            path = monitorInfo.CurrOriginal
            If String.IsNullOrWhiteSpace(path) Then path = monitorInfo.CurrWallpaper
        End If
        If path IsNot Nothing Then
            If IO.File.Exists(path) Then
                Try
                    IO.File.Copy(path, IO.Path.Combine(dirSaved, IO.Path.GetFileName(path)), True)
                Catch ex As Exception
                    Log(LogLvl.Error, "Can't copy file", ex)
                End Try
            Else
                Log(LogLvl.Error, "No file found at " & path)
            End If
        Else
            Log(LogLvl.Error, "No path found for " & screen.DeviceName)
        End If
    End Sub

    Public Shared Sub OpenWallpaper(screen As Screen, Optional last As Boolean = False)
        Dim monitorInfo = Registry.GetMonitorInfoFor(screen.DeviceName)
        Dim url As String = ""
        If last Then
            url = monitorInfo.LastPostUrl
            If String.IsNullOrWhiteSpace(url) Then url = monitorInfo.LastFileUrl
        Else
            url = monitorInfo.CurrPostUrl
            If String.IsNullOrWhiteSpace(url) Then url = monitorInfo.CurrFileUrl
        End If
        If String.IsNullOrWhiteSpace(url) Then
            Log(LogLvl.Error, "Can't get url for " & screen.DeviceName)
            Return
        End If
        Try
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.AppStarting
            System.Diagnostics.Process.Start("explorer.exe", """" & url & """")
        Catch ex As Exception
            Log(LogLvl.Error, "Can't open browser", ex)
        Finally
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
    End Sub
End Class
