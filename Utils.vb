Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Public Class Utils

    Public Shared Sub SaveScreenshotWithRectangles(Path As String, Optional RedRects As IEnumerable(Of Rectangle) = Nothing, Optional GreenRects As IEnumerable(Of Rectangle) = Nothing, Optional BlueRects As IEnumerable(Of Rectangle) = Nothing)
        Dim bitmap As New Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height)

        Using g = Graphics.FromImage(bitmap)
            g.CopyFromScreen(SystemInformation.VirtualScreen.Left, SystemInformation.VirtualScreen.Top, 0, 0, New Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height))
            If RedRects IsNot Nothing AndAlso RedRects.Count > 0 Then g.DrawRectangles(New Pen(Color.Red, 4) With {.Alignment = Drawing2D.PenAlignment.Inset}, RedRects.ToArray())
            If GreenRects IsNot Nothing AndAlso GreenRects.Count > 0 Then g.DrawRectangles(New Pen(Color.Green, 4) With {.Alignment = Drawing2D.PenAlignment.Inset}, GreenRects.ToArray())
            If BlueRects IsNot Nothing AndAlso BlueRects.Count > 0 Then g.DrawRectangles(New Pen(Color.Blue, 4) With {.Alignment = Drawing2D.PenAlignment.Inset}, BlueRects.ToArray())
        End Using
        Try
            Using fs = New FileStream(Path, FileMode.OpenOrCreate)
                bitmap.Save(fs, Imaging.ImageFormat.Bmp)
            End Using
            Log(LogLvl.Debug, "Saved debug screenshot to " & Path)
        Catch ex As Exception
            Log(LogLvl.Warning, "Can't save file")
        End Try
    End Sub

    Public Shared Function GetRectangleFromEdges(left As Integer, top As Integer, right As Integer, bottom As Integer) As Rectangle
        Return New Rectangle(left, top, Math.Max(right - left, 0), Math.Max(bottom - top, 0))
    End Function

End Class

Public Module Extensions

    <Extension()>
    Public Function IsImage(Path As String) As Boolean
        If String.IsNullOrWhiteSpace(Path) Then Return False

        Dim extension = IO.Path.GetExtension(Path)

        If String.IsNullOrEmpty(extension) Then Return False

        If IMAGE_FILE_FORMATS.Contains(extension) Then Return True

        Return False
    End Function
    Private ReadOnly IMAGE_FILE_FORMATS As String() = {
        ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ' Joint Photographic Experts Group
        ".jp2", ".j2k", ".jpf", ".jpx", ".jpm", ".mj2",   ' JPEG2000
        ".png",                                           ' Portable Network Graphics
        ".gif",                                           ' Graphics Interchange Format
        ".webp",                                          ' WebP (Google)
        ".tiff", ".tif",                                  ' Tag Image File Format
        ".bmp", ".dib",                                   ' Bitmap
        ".heif", ".heic",                                 ' High Efficiency Image File
        ".wmf", ".emf",                                   ' Windows Metafile Formats
        ".svg", ".svgz",                                  ' Scalable Vector Graphics
        ".raw", ".arw", ".cr2", ".nrw", ".k25"            ' RAW
    }

    <Extension()>
    Public Function GetFlags(ByVal value As [Enum]) As IEnumerable(Of [Enum])
        Return GetFlags(value, [Enum].GetValues(value.[GetType]()).Cast(Of [Enum])().ToArray())
    End Function

    <Extension()>
    Public Function GetIndividualFlags(ByVal value As [Enum]) As IEnumerable(Of [Enum])
        Return GetFlags(value, GetFlagValues(value.[GetType]()).ToArray())
    End Function

    Private Function GetFlags(ByVal value As [Enum], ByVal values As [Enum]()) As IEnumerable(Of [Enum])
        Dim bits As ULong = Convert.ToUInt64(value)
        Dim results As List(Of [Enum]) = New List(Of [Enum])()

        For i As Integer = values.Length - 1 To 0 Step -1
            Dim mask As ULong = Convert.ToUInt64(values(i))
            If i = 0 AndAlso mask = 0L Then Exit For

            If (bits And mask) = mask Then
                results.Add(values(i))
                bits -= mask
            End If
        Next

        If bits <> 0L Then Return Enumerable.Empty(Of [Enum])()
        If Convert.ToUInt64(value) <> 0L Then Return results
        If bits = Convert.ToUInt64(value) AndAlso values.Length > 0 AndAlso Convert.ToUInt64(values(0)) = 0L Then Return values.Take(1)
        Return Enumerable.Empty(Of [Enum])()
    End Function

    Private Iterator Function GetFlagValues(ByVal enumType As Type) As IEnumerable(Of [Enum])
        Dim flag As ULong = &H1

        For Each value In [Enum].GetValues(enumType).Cast(Of [Enum])()
            Dim bits As ULong = Convert.ToUInt64(value)
            'yield return value;
            If bits = 0L Then Continue For ' skip the zero value
            While flag < bits
                flag <<= 1
            End While

            If flag = bits Then Yield value
        Next
    End Function

#Region "Rectangles"
    <Extension()>
    Public Function Subtract(ByVal rect As Rectangle, ByVal subtracted As IEnumerable(Of Rectangle), Optional tolerance As Integer = 4) As List(Of Rectangle)
        Dim output As New List(Of Rectangle) From {rect}

        Dim count As Integer 'debug

        For Each sRect In subtracted
            Dim temp As New List(Of Rectangle)

            For Each bRect In output
                temp.AddRange(bRect.Subtract(sRect, tolerance))
            Next

            output.Clear()
            output.AddRange(temp)

            'Debug
            count += 1
            Utils.SaveScreenshotWithRectangles($"{count}_subtract.bmp", {sRect}, output)
        Next

        Return output
    End Function


    <Extension()>
    Public Function Subtract(ByVal base As Rectangle, ByVal subtraction As Rectangle, Optional tolerance As Integer = 4) As List(Of Rectangle)
        'Base rect is empty or gets erased entirely
        If base.Height <= tolerance OrElse base.Width <= tolerance OrElse base.Equals(subtraction) OrElse
           (subtraction.Left < base.Left AndAlso subtraction.Top < base.Top AndAlso subtraction.Right > base.Right AndAlso subtraction.Bottom > base.Bottom) Then
            'Return empty list
            Return New List(Of Rectangle)
        End If

        'Get intersection
        Dim subt As Rectangle = Rectangle.Intersect(base, subtraction)

        'Return base rect untouched if substraction does not intersect
        If subt.Height <= tolerance OrElse subt.Width <= tolerance Then
            Return New List(Of Rectangle) From {
                base
            }
        End If

        'Generate and return sourrounding rectangles
        Dim results As List(Of Rectangle) = New List(Of Rectangle)

        Dim topL_ = Utils.GetRectangleFromEdges(base.Left, base.Top, subt.Left, subt.Top)
        Dim top__ = Utils.GetRectangleFromEdges(subt.Left, base.Top, subt.Right, subt.Top)
        Dim topR_ = Utils.GetRectangleFromEdges(subt.Right, base.Top, base.Right, subt.Top)
        Dim right = Utils.GetRectangleFromEdges(subt.Right, subt.Top, base.Right, subt.Bottom)
        Dim botR_ = Utils.GetRectangleFromEdges(subt.Right, subt.Bottom, base.Right, base.Bottom)
        Dim bot__ = Utils.GetRectangleFromEdges(subt.Left, subt.Bottom, subt.Right, base.Bottom)
        Dim botL_ = Utils.GetRectangleFromEdges(base.Left, subt.Bottom, subt.Left, base.Bottom)
        Dim left_ = Utils.GetRectangleFromEdges(base.Left, subt.Top, subt.Left, subt.Bottom)

        If topL_.Height > tolerance AndAlso topL_.Width > tolerance Then results.Add(topL_)
        If top__.Height > tolerance AndAlso top__.Width > tolerance Then results.Add(top__)
        If topR_.Height > tolerance AndAlso topR_.Width > tolerance Then results.Add(topR_)
        If right.Height > tolerance AndAlso right.Width > tolerance Then results.Add(right)
        If botR_.Height > tolerance AndAlso botR_.Width > tolerance Then results.Add(botR_)
        If bot__.Height > tolerance AndAlso bot__.Width > tolerance Then results.Add(bot__)
        If botL_.Height > tolerance AndAlso botL_.Width > tolerance Then results.Add(botL_)
        If left_.Height > tolerance AndAlso left_.Width > tolerance Then results.Add(left_)

        Return results
    End Function
#End Region
End Module