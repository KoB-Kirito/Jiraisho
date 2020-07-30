Imports System.IO
Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.Processing

Module ProcessingLoop
    Public SlideshowTimer As Timers.Timer

    Public Desktop As DesktopClient
    Public Downloader As DownloadClient

    Private _isStillProcessing As Boolean
    Private _overdueCounter As Integer
    Private _lastMonitor As Integer

    Public Sub GetNextWallpaperForAllMonitors(Optional Overwrite As Boolean = False)
        Dim workers As New List(Of Task)

        For Each monitor In Desktop.Monitors.Values
            If Not Overwrite Then
                'Skip if there is already a valid file saved
                Dim path = Registry.GetNextWallpaperPathFor(monitor)
                If path IsNot Nothing AndAlso IO.File.Exists(path) Then
                    Log(LogLvl.Debug, $"{monitor.DeviceName} has an image already")
                    Continue For
                End If
            End If

            'Get new image
            workers.Add(GetNextWallpaper(monitor))
        Next

        If Not Task.WaitAll(workers.ToArray(), 30000) Then
            'Wait 30 seconds max
            Log(LogLvl.Warning, "Failed to preload wallpapers for each monitor")
        End If
    End Sub

    Public Sub ProcessingLoop(sender As Object, e As Timers.ElapsedEventArgs)
        'Ensure that the loop runs not concurrent
        If _isStillProcessing Then
            'Check if the loop is waiting for another loop for too long
            If _overdueCounter >= 3 Then
                Log(LogLvl.Warning, $"Loop is still processing... (Waited {_overdueCounter} seconds)")
            Else
                Log(LogLvl.Debug, $"Loop is still processing...")
            End If
            _overdueCounter += 1

            'Retry every second
            SetShortRetryTimer(1000)

            Return
        Else
            _overdueCounter = 0
        End If

        'Lock processing loop while processing
        Try
            _isStillProcessing = True

            'Keep track of processing time
            Dim processingTime As New Stopwatch
            processingTime.Start()

            'Check if user is idling
            'ToDo: Make this function configurable
            Dim lastInput As New NativeMethod.User32.LASTINPUTINFO(True)
            If NativeMethod.User32.GetLastInputInfo(lastInput) Then
                Dim idleTime = Environment.TickCount - lastInput.dwTime
                If idleTime > 60000 Then '1min, ToDo: Add to config
                    Log(LogLvl.Warning, $"User is idling for {Math.Round(idleTime / 1000, 0)} seconds. Waiting...")
                    SetShortRetryTimer(10000)
                    Return
                Else
                    Log(LogLvl.Info, $"User is not idling ({Math.Round(idleTime / 1000, 0)} seconds)")
                End If
            Else
                Log(LogLvl.Warning, "Failed to get last input info")
                'Failback -> Just go on and assume the user is not idling
            End If

            'Get next available monitor
            Dim nextMonitorId = GetNextAvailableMonitor()
            Log(LogLvl.Debug, "nextMonitorId: " & nextMonitorId)

            'Skip if no monitor is available
            If nextMonitorId = -1 Then
                Log(LogLvl.Warning, "No monitor was available for output (No desktop was visible)")

                'Retry every 5 seconds if no monitor was available
                SetShortRetryTimer()
                Return
            End If
            Dim nextMonitor = Desktop.Monitors(nextMonitorId)
            Log(LogLvl.Info, $"Processing {nextMonitor.DeviceName.Substring(4)}...")

            'Get next image for nextMonitor
            Dim currPath = Registry.GetNextWallpaperPathFor(nextMonitor)

            'Check next image
            If currPath Is Nothing Then
                Log(LogLvl.Warning, "Could not get path for next image from registry")
            ElseIf Not IO.File.Exists(currPath) Then
                Log(LogLvl.Warning, $"File for next image not found ({currPath})")
            Else
                'Actually set wallpaper
                Desktop.SetWallpaper(nextMonitorId, currPath)

                'Wallpaper was successfully changed -> Reset timer
                ResetTimer()

                'Update registry
                Registry.ShiftWallpapersFor(nextMonitor)

                Log(LogLvl.Info, $"New wallpaper set after {processingTime.ElapsedMilliseconds} ms")
            End If

            'Get next wallpaper
            Dim t = GetNextWallpaper(nextMonitor, processingTime)
            If Not t.Wait(SlideshowTimer.Interval + 10000) Then
                Log(LogLvl.Warning, "Cancelled GetNextWallpaper due beeing 10 seconds overdue")
            End If
            If t.Result Then
                Log(LogLvl.Info, $"Finished after {processingTime.ElapsedMilliseconds} ms")
            Else
                Log(LogLvl.Warning, "Failed to get next wallpaper")
            End If
            t.Dispose()

            'Hint GC to collect now while idling
            Runtime.GCSettings.LargeObjectHeapCompactionMode = Runtime.GCLargeObjectHeapCompactionMode.CompactOnce
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, True) 'ToDo: Make mode configurable
            'Default = Forced, because ~30MB occupied memory looks better than > 300MB

        Finally
            _isStillProcessing = False
        End Try

    End Sub

    Private Async Function GetNextWallpaper(Monitor As Monitor, Optional watch As Stopwatch = Nothing) As Task(Of Boolean)
        Log(LogLvl.Info, "Preparing next wallpaper...")

        'Cheching processingtime
        If watch Is Nothing Then
            watch = New Stopwatch
            watch.Start()
        End If

        'Search next image
        Dim result = Await Downloader.GetRandomImageAsync(Monitor)
        If result Is Nothing Then
            Log(LogLvl.Warning, "Result from downloader was nothing")
            Return False
        End If
        Log(LogLvl.Info, $"Search successfull after {watch.ElapsedMilliseconds} ms")

        'Download image
        Using imageStream = Await Downloader.DownloadFileAsync(result.Value.fileUrl)
            If imageStream Is Nothing Then
                Log(LogLvl.Warning, "Download did not finish in time")
                Return False
            End If
            Log(LogLvl.Debug, $"Download successfull after {watch.ElapsedMilliseconds} ms")

            'Prepare and save image
            Dim originalPath As String = ""
            Dim editedPath As String = ""
            'Get directory
            Dim monitorDirPath = Path.Combine(CFG.DirHistory, Monitor.DeviceName.Substring(4))
            Dim monitorDir = IO.Directory.CreateDirectory(monitorDirPath)

            'Save original image
            Dim resultExtension = Path.GetExtension(result.Value.fileUrl.LocalPath)
            originalPath = Path.Combine(monitorDirPath, $"{CFG.Source}_{result.Value.id}{resultExtension}")
            Using fs = IO.File.Create(originalPath)
                imageStream.CopyTo(fs)
            End Using
            imageStream.Position = 0

            'Load image for alteration
            'ToDo: Skip if not nessesary
            Using source = Image.Load(imageStream)
                'Log(LogLvl.Trace, $"Source before manipulation: {source.Width} x {source.Height} (Monitor: {Monitor.Rectangle.Width} x {Monitor.Rectangle.Height})")

                'Mutate image depending on set style
                Select Case CFG.Style(Monitor.Id)
                    Case CustomStyle.FitLeft
                        Log(LogLvl.Trace, "Mutating image for FitLeft")
                        source.Mutate(Sub(x)
                                          x.Resize(New ResizeOptions() With {
                                                      .Mode = ResizeMode.Pad,
                                                      .Size = New Size(Monitor.Rectangle.Width, Monitor.Rectangle.Height),
                                                      .Position = AnchorPositionMode.Left
                                                   })
                                      End Sub)

                    Case CustomStyle.FitRight
                        Log(LogLvl.Trace, "Mutating image for FitRight")
                        source.Mutate(Sub(x)
                                          x.Resize(New ResizeOptions() With {
                                                       .Mode = ResizeMode.Pad,
                                                       .Size = New Size(Monitor.Rectangle.Width, Monitor.Rectangle.Height),
                                                       .Position = AnchorPositionMode.Right
                                                   })
                                      End Sub)

                    Case Else
                        Log(LogLvl.Warning, "No custom style set")

                        'ToDo: Implement other styles

                End Select

                'Log(LogLvl.Debug, $"Source after manipulation: {source.Width} x {source.Height}")

                'Save file
                editedPath = Path.Combine(monitorDirPath, $"{CFG.Source}_{result.Value.id}_edited.png")
                Using fs = IO.File.Create(editedPath)
                    source.SaveAsPng(fs, New Formats.Png.PngEncoder With {.CompressionLevel = 6})
                End Using
            End Using

            'Only keep the last x wallpapers (last, curr, next)
            Dim fiArray = monitorDir.GetFiles()
            If fiArray.Count > CFG.MaxHistory * 2 Then
                Array.Sort(fiArray, Function(x, y) StringComparer.OrdinalIgnoreCase.Compare(x.CreationTime, y.CreationTime))
                For i = 0 To fiArray.GetUpperBound(0) - CFG.MaxHistory
                    Try
                        fiArray(i).Delete()
                    Catch ex As Exception
                        Log(LogLvl.Warning, "Can't delete file in history", ex)
                    End Try
                Next
            End If
            Log(LogLvl.Info, $"Style applied after {watch.ElapsedMilliseconds} ms")

            'Save to registry
            Registry.SetNextWallpaperFor(Monitor, result.Value, originalPath, editedPath)
        End Using

        Return True
    End Function

    ''' <summary>
    ''' Sets the timer to a short period.
    ''' </summary>
    Private Sub SetShortRetryTimer(Optional newInterval As Double = 5000.0)
        If SlideshowTimer.Interval > newInterval Then
            SlideshowTimer.Stop()
            SlideshowTimer.Interval = newInterval
            SlideshowTimer.Start()
        End If
    End Sub

    ''' <summary>
    ''' Resets the timer to the setting from the config.
    ''' </summary>
    Private Sub ResetTimer()
        Dim currSetInterval As Double = CFG.IntervalInSeconds * 1000
        If SlideshowTimer.Interval <> currSetInterval Then
            SlideshowTimer.Stop()
            SlideshowTimer.Interval = currSetInterval
            SlideshowTimer.Start()
        End If
    End Sub

    Private Function GetNextAvailableMonitor(Optional DontAlterLastMonitor As Boolean = False) As Integer
        Dim count = Desktop.Monitors.Count
        'Log(LogLvl.Debug, "Monitors.Count: " & count)
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
        'Log(LogLvl.Debug, $"Start={i} currLowest={currLowestKey} currHighest={currHighestKey}")
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

            'Log(LogLvl.Debug, "Checking " & i)
            If Desktop.IsDesktopVisible(i) Then
                If Not DontAlterLastMonitor Then _lastMonitor = i
                Return i
            End If

            'Checked everything if lastKey is reached again
            If i = _lastMonitor Then Return -1
        Loop
    End Function

End Module
