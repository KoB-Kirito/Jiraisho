Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.Processing

Public Class ImageManipulation
    'ToDo: Implement

    Public Shared Function CreateWallpaper(Source As Image, TargetScreen As Rectangle, Style As CustomStyle) As Image
        Select Case Style
            Case CustomStyle.FitLeft
                Return Source.Clone(Sub(x)
                                        x.Resize(New ResizeOptions() With {
                                                 .Mode = ResizeMode.Max,
                                                 .Size = New Size(TargetScreen.Width, TargetScreen.Height),
                                                 .Position = AnchorPositionMode.Left})
                                    End Sub)

            Case CustomStyle.FitRight
                Return Source.Clone(Sub(x)
                                        x.Resize(New ResizeOptions() With {
                                                 .Mode = ResizeMode.Max,
                                                 .Size = New Size(TargetScreen.Width, TargetScreen.Height),
                                                 .Position = AnchorPositionMode.Right})
                                    End Sub)

            Case Else
                Log(LogLvl.Warning, "No custom style set")
                Return Nothing

        End Select
    End Function

End Class

Public Enum CustomStyle
    None
    FitLeft
    FitRight
End Enum
