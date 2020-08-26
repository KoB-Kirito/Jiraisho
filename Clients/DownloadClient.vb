Imports System.CodeDom
Imports System.IO
Imports System.Net.Http
Imports System.Threading
Imports BooruSharp.Booru
Imports BooruSharp.Others
Imports Newtonsoft.Json
Imports SixLabors.ImageSharp

Class DownloadClient
    Public Shared ReadOnly Property Sources As New List(Of String) From {
        "Pixiv",
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
    Private _httpClientHandler As HttpClientHandler
    Private _currentSource As ABooru
    Private _asyncLock As New SemaphoreSlim(1, 1)

    Public Async Function SetCurrentSourceAsync(Source As String, Username As String, Password As String) As Task
        Try
            Await _asyncLock.WaitAsync()
            If _currentSource IsNot Nothing Then
                Log(LogLvl.Warning, $"Source({_currentSource.GetType().Name}) get's overwritten")
            End If
            Select Case Source
                Case "Pixiv"
                    Dim pixiv As New Pixiv

                    'Try refreshToken
                    Dim refreshToken As String = Registry.GetRefreshToken(Source)
                    If Not String.IsNullOrWhiteSpace(refreshToken) Then
                        Try
                            Await pixiv.LoginAsync(refreshToken)
                        Catch ex As Exception
                            Log(LogLvl.Warning, "Init with token failed.", ex)
                        End Try
                    End If

                    'Try username and password
                    If pixiv Is Nothing Then
                        If String.IsNullOrWhiteSpace(Username) OrElse String.IsNullOrWhiteSpace(Password) Then
                            'Reset settings
                            CFG.Source = "Konachan"
                            'Crash, ToDo: Make this smoother
                            Log(LogLvl.Critical, "Pixiv only works with username and password!")
                        End If
                        Try
                            Await pixiv.LoginAsync(Username, Password)
                        Catch ex As Exception
                            'Reset settings
                            CFG.Source = "Konachan"

                            Log(LogLvl.Critical, "Pixiv authentification failed", ex) 'ToDo: Improve user experience
                        End Try
                    End If

                    'Save refreshToken
                    Registry.SetRefreshToken(pixiv.RefreshToken)

                    _currentSource = pixiv

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
                _currentSource.HttpClient = _httpClient
            End If
        Finally
            _asyncLock.Release()
        End Try
    End Function

    Public Sub New()
        Log(LogLvl.Trace, "Called", Source:="New DownloadClient")

        _httpClientHandler = New HttpClientHandler
        _httpClient = New HttpClient(_httpClientHandler)

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

    Public Async Function GetRandomImageAsync(Monitor As Monitor) As Task(Of BooruSharp.Search.Post.SearchResult?)
        'ToDo: Redo this spaghetti part...
        'Log(LogLvl.Trace, "Called")

        'Init tags, add rating if set, add custom tags from config
        Dim tags As New List(Of String)
        If CFG.Rating <> Rating.All Then tags.Add(ratingToString(CFG.Rating))
        If CFG.CustomTags IsNot Nothing Then
            For Each tag In CFG.CustomTags
                tags.Add(tag)
            Next
        End If

        'Log(LogLvl.Debug, "Search string: " & String.Join(" ", tags))

        Dim result As BooruSharp.Search.Post.SearchResult
        Dim ratioOfNextMonitor = Monitor.Rectangle.Width / Monitor.Rectangle.Height
        Dim resolutionV = Monitor.Rectangle.Width * Monitor.Rectangle.Height

        Try
            Await _asyncLock.WaitAsync() 'ToDo: Remove if bool on processing loop is sufficient
            Dim tries As Integer
            'Retry loop
            Do
                tries += 1
                If tries >= 10 Then
                    Log(LogLvl.Warning, "Max tries reached")
                    Return Nothing
                End If

                Dim results As BooruSharp.Search.Post.SearchResult()

                If _currentSource.HasMultipleRandomAPI() Then
                    Log(LogLvl.Trace, $"Calling GetRandomImagesAsync with (100, {If(tags.Count = 0, "Nothing", String.Join(" ", tags))})")
                    results = Await _currentSource.GetRandomPostsAsync(100, tags.ToArray())
                    Log(LogLvl.Debug, $"Got {results.Length} search results")
                Else
                    'Just get one
                    Log(LogLvl.Trace, $"Calling GetRandomImageAsync with ({If(tags.Count = 0, "Nothing", String.Join(" ", tags))})")
                    ReDim results(0)
                    results(0) = Await _currentSource.GetRandomPostAsync(tags.ToArray())
                    Log(LogLvl.Debug, $"Got one result")
                End If

                'Checks
                For Each res In results
                    Log(LogLvl.Trace, $"Checking {res.id} [{res.width} x {res.height}px]({res.postUrl})")

                    'Is image
                    If Not res.fileUrl.LocalPath.IsImage Then
                        Log(LogLvl.Warning, $"Extension indicates that file is not an image ({res.fileUrl.LocalPath})")
                        Continue For
                    End If

                    'Is desktop resolution
                    If CFG.OnlyDesktopRatio Then
                        Dim ratioOfPost = res.width / res.height                                                                                                    'ToDo: Make deviation customizable
                        If (Not CFG.AllowSmallDeviations AndAlso Not ratioOfPost = ratioOfNextMonitor) OrElse (CFG.AllowSmallDeviations AndAlso Math.Abs(ratioOfNextMonitor - ratioOfPost) > 0.2) Then
                            Log(LogLvl.Debug, $"Ratio is not okay ({Math.Abs(ratioOfNextMonitor - ratioOfPost)}) allowDeviation: {CFG.AllowSmallDeviations}")
                            Continue For
                        End If
                    End If
                    'Min resolution
                    If CFG.MinResolution > 0 Then
                        Dim rResolutionV = res.width * res.height
                        If rResolutionV < resolutionV * CFG.MinResolution Then
                            Log(LogLvl.Warning, $"Resolution is lower than minimum ({rResolutionV} < {resolutionV} [{CFG.MinResolution}])")
                            Continue For
                        End If
                    End If

                    'Only reached if everything is okay
                    'Just use the first one that satisfies all requirements
                    Log(LogLvl.Info, $"Passed: {res.id} [{res.width} x {res.height}px]({res.postUrl}) Tries: {tries}")
                    result = res
                    Exit Do 'Exit point for loop
                Next
            Loop
        Catch ex As Exception
            Log(LogLvl.Debug, "Exception:", ex)
            Log(LogLvl.Error, CFG.Source & " replied: " & ex.Message)
            Return Nothing
        Finally
            _asyncLock.Release()
        End Try

        Return result
    End Function

    Public Async Function DownloadFileAsync(Url As Uri) As Task(Of IO.Stream)
        Try
            Dim response As HttpResponseMessage
            If CFG.Source = "Pixiv" Then
                Dim pixiv As Pixiv = TryCast(_currentSource, Pixiv)

                Log(LogLvl.Debug, $"Pixiv > Download with token (*****{pixiv.Token.Substring(pixiv.Token.Length - 5)})")

                Dim request = New HttpRequestMessage(New HttpMethod("GET"), Url)
                request.Headers.Add("Authorization", "Bearer " & pixiv.Token)
                response = Await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead)
            Else
                response = Await _httpClient.GetAsync(Url, HttpCompletionOption.ResponseContentRead)
            End If

            response.EnsureSuccessStatusCode()
            Return Await response.Content.ReadAsStreamAsync()
        Catch ex As Exception
            Log(LogLvl.Warning, $"Failed to download {Url}", ex)
            Return Nothing
        End Try
    End Function

    Public Async Function AddFavouriteAsync(postId As Integer) As Task
        If _currentSource.Auth IsNot Nothing Then
            _currentSource.Auth = New BooruAuth(CFG.Username, CFG.Password)
        End If

        If _currentSource.Auth IsNot Nothing Then
            Await _currentSource.AddFavoriteAsync(postId)
        End If
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