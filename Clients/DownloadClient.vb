Imports System.CodeDom
Imports System.IO
Imports System.Net.Http
Imports System.Threading
Imports BooruSharp.Booru
Imports Newtonsoft.Json
Imports SixLabors.ImageSharp

Class DownloadClient
    Public Shared ReadOnly Property Sources As New List(Of String) From {
        "Atfbooru",
        "Danbooru",
        "E621",
        "E926",
        "Furrybooru",
        "Gelbooru",
        "Konachan",
        "Lolibooru",
        "Realbooru",
        "Rule34",
        "Safebooru",
        "Sakugabooru",
        "SankakuComplex",
        "Wildcritters", 'custom
        "Xbooru",
        "Yandere",
        "LocalFolder"
    }

    Private _httpClient As HttpClient
    Private _currentSource As ABooru
    Private _asyncLock As New SemaphoreSlim(1, 1)

    Public Sub SetCurrentSource(Source As String)
        Try
            _asyncLock.Wait()
            If _currentSource IsNot Nothing Then
                Log(LogLvl.Warning, $"Source({_currentSource.GetType().Name}) get's overwritten")
            End If
            Select Case Source
                Case "Atfbooru"
                    _currentSource = New Atfbooru()

                Case "Danbooru"
                    _currentSource = New DanbooruDonmai

                Case "E621"
                    _currentSource = New E621

                Case "E926"
                    _currentSource = New E926

                Case "Furrybooru"
                    _currentSource = New Furrybooru

                Case "Gelbooru"
                    _currentSource = New Gelbooru

                Case "Konachan"
                    _currentSource = New Konachan

                Case "Lolibooru"
                    _currentSource = New Lolibooru

                Case "Realbooru"
                    _currentSource = New Realbooru

                Case "Rule34"
                    _currentSource = New Rule34

                Case "Safebooru"
                    _currentSource = New Safebooru

                Case "Sakugabooru"
                    _currentSource = New Sakugabooru

                Case "SankakuComplex"
                    _currentSource = New SankakuComplex

                Case "Wildcritters" ' Custom
                    _currentSource = New Wildcritters

                Case "Xbooru"
                    _currentSource = New Xbooru

                Case "Yandere"
                    _currentSource = New Yandere

                Case Else
                    _currentSource = Nothing
                    Log(LogLvl.Warning, "Source set to Nothing")
            End Select

            If _currentSource IsNot Nothing Then
                _currentSource.SetHttpClient(_httpClient)
            End If
        Finally
            _asyncLock.Release()
        End Try
    End Sub

    Public Sub New()
        Log(LogLvl.Trace, "Called", Source:="New DownloadClient")

        _httpClient = New HttpClient
        If CFG.Source <> "LocalFile" Then
            SetCurrentSource(CFG.Source)
        End If

        Log(LogLvl.Trace, "Reached end", Source:="New DownloadClient")
    End Sub

    Public Async Function CheckInternetConnection() As Task(Of Boolean)
        Try
            Await _currentSource.CheckAvailabilityAsync()
            Return True
        Catch ex As Exception
            Log(LogLvl.Warning, _currentSource.GetType().Name & " is not available", ex)
        End Try

        Return False
    End Function

    Public Async Function GetPostCountAsync(ParamArray Tags As String()) As Task(Of Integer)
        Try
            Await _asyncLock.WaitAsync()
            Return Await _currentSource.GetPostCountAsync(Tags)
        Catch ex As Exception
            Log(LogLvl.Warning, "GetPostCount failed", ex)
            Return -1
        Finally
            _asyncLock.Release()
        End Try
    End Function

    Public Async Function GetRandomImageAsync() As Task(Of MyImage)
        Log(LogLvl.Trace, "Called")

        Dim tags As New List(Of String)
        If CFG.Rating <> Rating.All Then tags.Add(ratingToString(CFG.Rating))
        If CFG.CustomTags IsNot Nothing Then
            For Each tag In CFG.CustomTags
                tags.Add(tag)
            Next
        End If

        Log(LogLvl.Debug, "Search: " & String.Join(" ", tags))

        Dim result As BooruSharp.Search.Post.SearchResult
        Try
            Await _asyncLock.WaitAsync()
            Dim resultOk As Boolean
            Dim tries As Integer
            Do
                tries += 1
                resultOk = True

                result = Await _currentSource.GetRandomImageAsync(tags.ToArray())

                'Checks
                If Not result.fileUrl.LocalPath.IsImage Then
                    Log(LogLvl.Warning, $"IsImage-Check failed. LocalPath: {result.fileUrl.LocalPath}")
                    resultOk = False
                End If
                'ToDo: Implement score limitations etc
            Loop Until resultOk OrElse tries >= 100
            Log(LogLvl.Debug, "Success. Tries: " & tries)
        Catch ex As Exception
            Log(LogLvl.Error, CFG.Source & " replied: " & ex.Message)
            Log(LogLvl.Debug, "Exception", ex)
            Return Nothing
        Finally
            _asyncLock.Release()
        End Try

        'Debug
        Log(LogLvl.Debug, $"Extension: {IO.Path.GetExtension(result.fileUrl.LocalPath)}{vbCrLf}File-Url: {result.fileUrl.AbsoluteUri}{vbCrLf}Post-Url: {result.postUrl.AbsoluteUri}")

        Dim stream = Await _httpClient.GetStreamAsync(result.fileUrl)

        Return New MyImage(stream, result.width, result.height, result.postUrl, result.fileUrl, result.source, result.id)
    End Function

    Private ReadOnly ratingToString As New SortedList(Of Rating, String) From {
        {Rating.Safe, "rating:safe"},
        {Rating.Questionable, "rating:questionable"},
        {Rating.Explicit, "rating:explicit"},
        {Rating.NoSafe, "-rating:safe"},
        {Rating.NoQuestionable, "-rating:questionable"},
        {Rating.NoExplicit, "-rating:explicit"}
    }
End Class

<Flags>
<JsonConverter(GetType(Converters.StringEnumConverter))>
Public Enum Rating As Byte
    None = 0
    Safe = 1
    Questionable = 2
    Explicit = 4
    NoSafe = Questionable + Explicit
    NoQuestionable = Safe + Explicit
    NoExplicit = Safe + Questionable
    All = Safe + Questionable + Explicit
End Enum

Public Class Wildcritters
    Inherits Template.Danbooru

    Public Sub New()
        MyBase.New("archive.wildcritters.ws")
    End Sub

    Public Overrides Function IsSafe() As Boolean
        Return False
    End Function
End Class