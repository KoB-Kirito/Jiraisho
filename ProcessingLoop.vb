Imports System.IO

Module ProcessingLoop
    Public SlideshowTimer As Timers.Timer

    Public Desktop As DesktopClient
    Public Downloader As DownloadClient

    Public CurrImages As New SortedList(Of Integer, MyImage)
    Public NextImage As MyImage

    Private _isStillProcessing As Boolean
    Private _lastMonitor As Integer

    Public Sub ProcessingLoop(sender As Object, e As Timers.ElapsedEventArgs)
        If _isStillProcessing Then
            Log(LogLvl.Warning, "Loop is still processing...")
            Return ' Never run the loop concurrent
        End If
        Try
            _isStillProcessing = True

            'Next image will be nothing on the first run
            If NextImage IsNot Nothing Then
                'Get next available monitor
                Dim nextMonitor = GetNextAvailableMonitor()
                Log(LogLvl.Debug, "Next monitor: " & nextMonitor)

                'Skip if no monitor is available
                If nextMonitor = -1 Then
                    Log(LogLvl.Warning, "No monitor was available for output")

                    'Check every 5 seconds if no monitor was available
                    SlideshowTimer.Stop()
                    SlideshowTimer.Interval = 5000
                    SlideshowTimer.Start()

                    Return
                End If

                'Save next image to history
                Dim filePath = IO.Path.Combine(CFG.DirHistory, $"{NextImage.SourceBooru}_{NextImage.Id}{NextImage.Extension}")
                Try
                    Using fs = IO.File.OpenWrite(filePath)
                        NextImage.Stream.CopyTo(fs)
                    End Using
                Catch ex As Exception
                    Log(LogLvl.Warning, "Failed to copy filestream", ex)
                Finally
                    NextImage.Stream.Dispose() 'Can be removed from memory now
                End Try
                NextImage.Filepath = filePath

                'Set next image wallpaper
                Desktop.SetWallpaper(nextMonitor, filePath)

                'Set next image as curr image
                If CurrImages.ContainsKey(nextMonitor) Then
                    CurrImages(nextMonitor) = NextImage
                Else
                    CurrImages.Add(nextMonitor, NextImage)
                End If

                'Update registry
                Dim currMonitorName = Desktop.Monitors(nextMonitor).DeviceName
                Registry.SetValue(currMonitorName & "-postUrl", CurrImages(nextMonitor).PostUrl.AbsoluteUri)
                Registry.SetValue(currMonitorName & "-fileUrl", CurrImages(nextMonitor).FileUrl.AbsoluteUri)
                Registry.SetValue(currMonitorName & "-filePath", CurrImages(nextMonitor).Filepath)

                'Reset timer on success
                SlideshowTimer.Stop()
                SlideshowTimer.Interval = CFG.IntervalInSeconds * 1000
                SlideshowTimer.Start()
            End If

            'Delete everything but the last 10 'ToDo: Configurable?
            Dim di = New DirectoryInfo(CFG.DirHistory)
            Dim fiArray = di.GetFiles()
            If fiArray.Count > CFG.MaxHistory Then
                Array.Sort(fiArray, Function(x, y) StringComparer.OrdinalIgnoreCase.Compare(x.CreationTime, y.CreationTime))
                For i = 0 To fiArray.GetUpperBound(0) - CFG.MaxHistory
                    Try
                        fiArray(i).Delete()
                    Catch ex As Exception
                        Log(LogLvl.Warning, "Can't delete file in history", ex)
                    End Try
                Next
            End If

            'Get new next image
            Dim dl = Task.Run(Async Function() As Task
                                  NextImage = Await Downloader.GetRandomImageAsync()
                              End Function)


            'Process next image
            'ToDo: manipulate image -> fill empty areas?


            dl.Wait(5000)
        Finally
            _isStillProcessing = False
        End Try

    End Sub

    Public Function GetNextAvailableMonitor(Optional DontAlterLastMonitor As Boolean = False) As Integer
        Dim count = Desktop.Monitors.Count
        Log(LogLvl.Debug, "Monitors.Count: " & count)
        If count = 0 Then Return -1 'If dict is empty
        If count = 1 Then 'If dict has only one item
            Dim firstKey = Desktop.Monitors.First().Key
            If Desktop.IsDesktopVisible(firstKey) Then
                _lastMonitor = firstKey
                Return firstKey
            Else
                Return -1
            End If
        End If
        Dim currLowestKey = Desktop.Monitors.First().Key
        Dim currHighestKey = Desktop.Monitors.Last().Key
        'In case _lastMonitor is not in range anymore
        If _lastMonitor < currLowestKey OrElse
        _lastMonitor > currHighestKey Then _lastMonitor = currHighestKey
        Dim i = _lastMonitor 'Starting point
        Log(LogLvl.Debug, $"Start={i} currLowest={currLowestKey} currHighest={currHighestKey}")
        Do
            'Increment iterator
            If i >= currHighestKey Then
                'Go to lowest if end is reached
                i = currLowestKey
            Else
                i += 1
            End If

            If Not Desktop.Monitors.ContainsKey(i) Then
                'If _lastKey is reached again, but not in dict
                If i = _lastMonitor Then Return -1

                Continue Do
            End If

            Log(LogLvl.Debug, "Checking " & i)
            If Desktop.IsDesktopVisible(i) Then
                If Not DontAlterLastMonitor Then _lastMonitor = i
                Return i
            End If

            'Checked everything if lastKey is reached again
            If i = _lastMonitor Then Return -1
        Loop
    End Function

End Module
