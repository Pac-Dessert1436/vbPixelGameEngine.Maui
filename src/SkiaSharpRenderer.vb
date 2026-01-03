Imports SkiaSharp

Public Class SkiaSharpRenderer
    Private m_pixelData As SKBitmap
    Private m_width As Integer
  Private m_height As Integer
  Private m_scale As Integer

  Public Property Scale As Integer
    Get
      Return m_scale
    End Get
    Private Set(value As Integer)
      m_scale = value
    End Set
  End Property


  Public Sub New(width As Integer, height As Integer, scale As Integer)
    m_width = width
    m_height = height
    m_scale = scale
    m_pixelData = New SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul)
  End Sub

  Public Sub UpdatePixels(pixels As Pixel())
        If pixels Is Nothing OrElse m_pixelData Is Nothing Then Return
        
        Dim length As Integer = Math.Min(pixels.Length, m_width * m_height)
        For i = 0 To length - 1
            Dim pixel As Pixel = pixels(i)
            m_pixelData.SetPixel(i Mod m_width, i \ m_width, New SKColor(pixel.R, pixel.G, pixel.B, pixel.A))
        Next
    End Sub

    Public Sub Render(canvas As SKCanvas)
        If canvas IsNot Nothing AndAlso m_pixelData IsNot Nothing Then
            canvas.DrawBitmap(m_pixelData, 0, 0)
        End If
    End Sub

    Public Sub Dispose()
        m_pixelData?.Dispose()
    End Sub
End Class