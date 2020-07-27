Imports System.IO
Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.Processing

Module ProcessingLoop
    Public SlideshowTimer As Timers.Timer

    Public Desktop As DesktopClient
    Public Downloader As DownloadClient

    Private _isStillProcessing As Boolean
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
            workers.Add(GetNextWallpaperFor(monitor))
        Next

        If Not Task.WaitAll(workers.ToArray(), 30000) Then
            Log(LogLvl.Warning, "Failed to preload wallpapers for each monitor")
        End If
    End Sub

    Public Sub ProcessingLoop(sender As Object, e As Timers.ElapsedEventArgs)
        'Ensure that the loop runs not concurrent
        If _isStillProcessing Then
            Log(LogLvl.Warning, "Loop is still processing...")
            SetShortRetryTimer()
            Return
        End If

        'Lock processing loop while processing
        Try
            _isStillProcessing = True

            'Keep track of processing time
            Dim processingTime As New Stopwatch
            processingTime.Start()

            'Get next available monitor
            Dim lastMonitor = _lastMonitor
            Dim nextMonitorId = GetNextAvailableMonitor()
            Log(LogLvl.Debug, "Next monitor: " & nextMonitorId)

            'Skip if no monitor is available
            If nextMonitorId = -1 Then
                Log(LogLvl.Warning, "No monitor was available for output")

                'Check every 5 seconds if no monitor was available
                SetShortRetryTimer()
                Return
            End If
            Dim nextMonitor = Desktop.Monitors(nextMonitorId)

            'Get next image for nextMonitor
            Dim currPath = Registry.GetNextWallpaperPathFor(nextMonitor)

            'Check next image
            If currPath IsNot Nothing AndAlso IO.File.Exists(currPath) Then
                'Actually set wallpaper
                Desktop.SetWallpaper(nextMonitorId, currPath)

                'Wallpaper was successfully changed -> Reset timer
                ResetTimer()

                'Update registry
                Registry.ShiftWallpapersFor(nextMonitor)
            End If

            'Get next wallpaper
            Dim t = GetNextWallpaperFor(nextMonitor)
            If Not t.Wait(SlideshowTimer.Interval - processingTime.ElapsedMilliseconds - 10) OrElse Not t.Result Then
                Log(LogLvl.Warning, "Failed to get next wallpaper in time")
            Else
                Log(LogLvl.Debug, $"Successfully got next wallpaper in {processingTime.ElapsedMilliseconds} ms")
            End If

        Finally
            _isStillProcessing = False
        End Try

    End Sub

    Private Async Function GetNextWallpaperFor(Monitor As Monitor) As Task(Of Boolean)
        Dim benchmark As New Stopwatch
        benchmark.Start()

        'Search next image
        Dim result = Await Downloader.GetRandomImageAsyncFor(Monitor)
        If result Is Nothing Then
            Log(LogLvl.Warning, "Result from downloader was nothing")
            Return False
        End If
        Log(LogLvl.Debug, $"Search successfull after {benchmark.ElapsedMilliseconds} ms")

        'Download image
        Using imageStream = Await Downloader.DownloadFile(result.Value.fileUrl)
            If imageStream Is Nothing Then
                Log(LogLvl.Warning, "Download did not finish in time")
                Return False
            End If
            Log(LogLvl.Debug, $"Download successfull after {benchmark.ElapsedMilliseconds} ms")

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
            Using source = Image.Load(New Configuration With {.MaxDegreeOfParallelism = Environment.ProcessorCount / 2}, imageStream)
                Log(LogLvl.Trace, $"Source before manipulation: {source.Width} x {source.Height} (Monitor: {Monitor.Rectangle.Width} x {Monitor.Rectangle.Height})")

                'Mutate image depending on set style
                Select Case CFG.Style(Monitor.Id)
                    Case CustomStyle.FitLeft
                        Log(LogLvl.Info, "Mutating image for FitLeft")
                        source.Mutate(Sub(x)
                                          x.Resize(New ResizeOptions() With {
                                                      .Mode = ResizeMode.Pad,
                                                      .Size = New Size(Monitor.Rectangle.Width, Monitor.Rectangle.Height),
                                                      .Position = AnchorPositionMode.Left
                                                   })
                                      End Sub)

                    Case CustomStyle.FitRight
                        Log(LogLvl.Info, "Mutating image for FitRight")
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

                Log(LogLvl.Debug, $"Source after manipulation: {source.Width} x {source.Height}")

                'Save file
                editedPath = Path.Combine(monitorDirPath, $"{CFG.Source}_{result.Value.id}_edited.png")
                Using fs = IO.File.Create(editedPath)
                    source.SaveAsPng(fs, New Formats.Png.PngEncoder With {.CompressionLevel = 6})
                End Using
            End Using

            'Fixes Imagesharps gigantic bufferarrays partly
            'ToDo: Improve this further https://github.com/SixLabors/ImageSharp/discussions/1290
            Runtime.GCSettings.LargeObjectHeapCompactionMode = Runtime.GCLargeObjectHeapCompactionMode.CompactOnce
            GC.Collect()

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
            Log(LogLvl.Debug, $"Image manipulation successfull after {benchmark.ElapsedMilliseconds} ms")

            'Save to registry
            Registry.SetNextWallpaperFor(Monitor, result.Value, originalPath, editedPath)
        End Using

        Return True
    End Function

    ''' <summary>
    ''' Sets the timer to a short period.
    ''' </summary>
    Private Sub SetShortRetryTimer()
        If SlideshowTimer.Interval > 5000.0 Then
            SlideshowTimer.Stop()
            SlideshowTimer.Interval = 5000.0
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
