Imports System.Collections.Concurrent
Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Module Logger 'Static
    Public AppName As String = Assembly.GetCallingAssembly.GetName().Name
    Public GlobalLogLevel As LogLvl = LogLvl.Trace
    Public FileLogDisabled As Boolean

    Private _fileLogger As SimpleFileLogger
    Private _triedInit As Boolean 'Only try once
    Private PATH_LOG As String = Path.Combine(DIR_CONFIG, "log.txt")

    Public Enum LogLvl
        Trace
        Debug
        Info
        Warning
        [Error]
        Critical
    End Enum

    Public Sub Log(LogLvl As LogLvl, Message As String, Optional ex As Exception = Nothing, <CallerMemberName> Optional Source As String = "", <CallerLineNumber> Optional Line As Integer = 0)
        If Not FileLogDisabled AndAlso Not _triedInit AndAlso _fileLogger Is Nothing Then
            _triedInit = True
            Try
                _fileLogger = New SimpleFileLogger(PATH_LOG)
                _fileLogger.WriteLog("Check https://github.com/KoB-Kirito/Jiraisho/releases for updates")
            Catch fEx As Exception
                MessageBox.Show("Can't create log-file" & vbCrLf & fEx.ToString(), AppName & " - " & "Error")
            End Try
        End If

        If LogLvl < GlobalLogLevel Then Return

        If Not FileLogDisabled AndAlso _fileLogger IsNot Nothing Then
            _fileLogger.WriteLog($"{LogLvl.ToString(),8} [{Date.Now:dd/MM/yy HH:mm:ss}] {Source}({Line,2}) {Message}{If(ex IsNot Nothing, vbCrLf & ex.ToString(), "")}")
        End If
        If LogLvl >= LogLvl.Error Then
            If SlideshowTimer IsNot Nothing Then SlideshowTimer.Stop()
            MessageBox.Show($"{Message}{If(ex IsNot Nothing, vbCrLf & "(" & ex.Message & ")", "")}{If(LogLvl = LogLvl.Critical, vbCrLf & "Critical error, closing...", "")}", AppName & " - " & LogLvl.ToString())
            If LogLvl = LogLvl.Critical Then Environment.Exit(0)
            If SlideshowTimer IsNot Nothing Then SlideshowTimer.Start()
        End If
    End Sub

    Public Sub DisposeLogger()
        Log(LogLvl.Debug, "Logger disposed")
        If _fileLogger IsNot Nothing Then _fileLogger.Dispose()
    End Sub
End Module

Class SimpleFileLogger
    Private _tWriter As TextWriter
    Private _queue As BlockingCollection(Of String)
    Private _backgroundTask As Task

    Public Sub New(Path As String)
        ' Open file or create new one
        Me._tWriter = New StreamWriter(Path, False, Text.Encoding.UTF8)

        ' Using BlockingCollection as a queue for thread-safety
        Me._queue = New BlockingCollection(Of String)(1024)

        ' Starting a background task that does the actual work
        ' The foreach will run infinite until Dispose() calls .CompleteAdding on the queue,
        ' because the enumerator returned blocks while the collection is empty
        ' The thread will just do nothing while the queue is empty, because the enumerator
        ' uses semaphores to wait for content, so it's very resource-friendly
        Me._backgroundTask = Task.Factory.StartNew(Sub()
                                                       For Each message In _queue.GetConsumingEnumerable()
                                                           WriteMessageToFile(message, _queue.Count = 0)
                                                       Next
                                                   End Sub, TaskCreationOptions.LongRunning)
    End Sub

    Private Sub WriteMessageToFile(Message As String, flush As Boolean)
        ' Adds the message to the writer
        _tWriter.WriteLine(Message)
        ' Actually writes the buffer of the writer to the file
        ' This only happens everytime the queue get's empty
        If flush Then _tWriter.Flush()
    End Sub


    ''' <summary>
    ''' Use this to write to the log-file.
    ''' </summary>
    Public Sub WriteLog(LogString As String)
        If Not _queue.IsAddingCompleted Then
            Try
                _queue.Add(LogString)
                Return
            Catch ex As InvalidOperationException
                ' CompleteAdding could be called after the if checked, but before the Add
                ' But we don't want to actually throw this exception
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Use this to dispose the file-logger
    ''' </summary>
    Public Sub Dispose()
        ' This causes the queue to not accept new values and stops the enumerator to block,
        ' so the foreach-loop can exit
        _queue.CompleteAdding()

        ' This gives the backgroundtask some time to finish
        Try
            _backgroundTask.Wait(1500)
        Catch ex As TaskCanceledException ' This exception is expected, so we want to catch it
        Catch ex As AggregateException When ex.InnerExceptions.Count = 1 AndAlso TypeOf ex.InnerExceptions(0) Is TaskCanceledException
        End Try

        _tWriter.Close()
    End Sub
End Class