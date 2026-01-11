Imports SixLabors.Fonts
Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.PixelFormats
Imports SixLabors.ImageSharp.Drawing.Processing
Imports System.IO

Public Class BitmapFont
    Private ReadOnly _font As Font
    Private ReadOnly _fontSize As Single
    Private _color As Pixel = Presets.WHITE

    Public Sub New(fontPathOrName As String, fontSize As Single, Optional color As Pixel = Nothing)
        _fontSize = fontSize
        ' Set default color if not provided
        If color Is Nothing Then _color = Presets.WHITE Else _color = color

        ' Load font (prefer custom file over system font)
        Dim fontCollection As New FontCollection()
        If File.Exists(fontPathOrName) Then
            ' Load custom font file (.ttf/.otf)
            Dim fontFamily = fontCollection.Install(fontPathOrName)
            _font = fontFamily.CreateFont(fontSize, FontStyle.Regular)
        Else
            ' Load system font (e.g., "Consolas", "Microsoft YaHei")
            Dim fontFamily = SystemFonts.Get(fontPathOrName)
            If fontFamily Is Nothing Then
                Throw New ArgumentException($"Font '{fontPathOrName}' not found.")
            End If
            _font = fontFamily.CreateFont(fontSize, FontStyle.Regular)
        End If
    End Sub

    ' Render text to Pixel array for pixel engine
    Public Function RenderTextToPixels(text As String) As (pixels As Pixel(), width As Integer, height As Integer)
        If String.IsNullOrEmpty(text) Then
            Return (Array.Empty(Of Pixel)(), 0, 0)
        End If

        ' Measure text size to determine bitmap size
        Dim textOptions = New TextOptions(_font)
        Dim textSize = TextMeasurer.Measure(text, textOptions)
        Dim width = CInt(Math.Ceiling(textSize.Width))
        Dim height = CInt(Math.Ceiling(textSize.Height))

        ' Create image and render text
        Using image = New Image(Of Rgba32)(width, height)
            image.Mutate(Sub(ctx)
                             ' Clear transparent background
                             ctx.Clear(Rgba32.Transparent)
                             ' Draw text (convert color to ImageSharp's Rgba32)
                             Dim textColor = New Rgba32(_color.R, _color.G, _color.B, _color.A)
                             ctx.DrawText(textOptions, text, textColor)
                         End Sub)

        ' Extract pixel data from image
        Dim pixels = New Pixel(width * height - 1) {}
        For y = 0 To height - 1
            For x = 0 To width - 1
                Dim rgba = image(x, y)
                pixels(y * width + x) = New Pixel(rgba.R, rgba.G, rgba.B, rgba.A)
            Next
        Next

        Return (pixels, width, height)
        End Using
    End Function

    Public Function RenderTextToSprite(text As String) As Sprite
        Dim (pixels, width, height) = RenderTextToPixels(text)
        Dim sprite = New Sprite(width, height)
        For y = 0 To height - 1
            For x = 0 To width - 1
                sprite.SetPixel(x, y, pixels(y * width + x))
            Next
        Next
        Return sprite
    End Function

    Public Sub SetColor(color As Pixel)
        _color = color
    End Sub

End Class