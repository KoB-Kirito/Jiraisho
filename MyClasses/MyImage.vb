Imports System.IO

Public Class MyImage
    Public Stream As Stream
    Public Width As Integer
    Public Height As Integer
    Public PostUrl As Uri
    Public FileUrl As Uri
    Public Source As String
    Public Id As Integer

    Public Filename As String
    Public Extension As String
    Public SourceBooru As String

    Public Filepath As String

    Public Sub New(ByRef Stream As Stream, Width As Integer, Height As Integer, postUrl As Uri, fileUrl As Uri, Source As String, Id As Integer)
        Me.Stream = Stream
        Me.Width = Width
        Me.Height = Height
        Me.PostUrl = postUrl
        Me.FileUrl = fileUrl
        Me.Source = Source
        Me.Id = Id

        Dim fileExt = fileUrl.LocalPath
        Me.Filename = Path.GetFileNameWithoutExtension(fileExt)
        Me.Extension = Path.GetExtension(fileExt)
        Me.SourceBooru = CFG.Source 'ToDo: Rather pass this
    End Sub
End Class
