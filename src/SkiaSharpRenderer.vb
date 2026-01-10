Imports System.Runtime.InteropServices
Imports SkiaSharp

Public Class SkiaSharpRenderer
  Private m_pixelData As SKBitmap
  Private m_pixmap As SKPixmap
  Private m_width As Integer
  Private m_height As Integer
  Private m_scale As Integer
  Private ReadOnly m_paint As SKPaint

  Public Property Scale As Integer
    Get
      Return m_scale
    End Get
    Private Set(value As Integer)
      m_scale = value
    End Set
  End Property

  Public ReadOnly Property Width As Integer
    Get
      Return m_width
    End Get
  End Property

  Public ReadOnly Property Height As Integer
    Get
      Return m_height
    End Get
  End Property

  Public Sub New(width As Integer, height As Integer, scale As Integer)
    m_width = width
    m_height = height
    m_scale = scale
    m_pixelData = New SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul)
    m_pixmap = m_pixelData.PeekPixels()
    m_paint = New SKPaint With {.IsAntialias = False}
  End Sub

  Public Sub UpdatePixels(pixels As Pixel())
    If pixels Is Nothing OrElse m_pixmap Is Nothing Then Exit Sub

    Dim length As Integer = Math.Min(pixels.Length, m_width * m_height)
    Dim pixelPtr As IntPtr = m_pixmap.GetPixels()

    For i = 0 To length - 1
      Dim pixel As Pixel = pixels(i)
      ' Use direct memory access for better performance
      Dim offset = i * 4
      Marshal.WriteByte(pixelPtr + offset, pixel.R)
      Marshal.WriteByte(pixelPtr + offset + 1, pixel.G)
      Marshal.WriteByte(pixelPtr + offset + 2, pixel.B)
      Marshal.WriteByte(pixelPtr + offset + 3, pixel.A)
    Next i
  End Sub

  Public Sub Render(canvas As SKCanvas)
    If canvas Is Nothing OrElse m_pixelData Is Nothing Then Exit Sub

    ' Clear the canvas with black
    canvas.Clear(SKColors.Black)

    ' Calculate scale transform to center and scale the game view
    canvas.Save()

    Dim scaledWidth = m_width * m_scale
    Dim scaledHeight = m_height * m_scale
    Dim canvasWidth = canvas.LocalClipBounds.Width
    Dim canvasHeight = canvas.LocalClipBounds.Height

    ' Calculate aspect ratio preservation 
    Dim scaleX As Single = canvasWidth / scaledWidth
    Dim scaleY As Single = canvasHeight / scaledHeight
    Dim scale As Single = Math.Min(scaleX, scaleY)

    Dim scaledBitmapWidth = scaledWidth * scale
    Dim scaledBitmapHeight = scaledHeight * scale
    Dim offsetX As Single = (canvasWidth - scaledBitmapWidth) / 2
    Dim offsetY As Single = (canvasHeight - scaledBitmapHeight) / 2

    canvas.Translate(offsetX, offsetY)
    canvas.Scale(scale, scale)

    ' Draw the game pixel data with nearest neighbor filtering 
    canvas.DrawBitmap(m_pixelData, 0, 0, m_paint)

    canvas.Restore()
  End Sub

  Public Sub Resize(width As Integer, height As Integer)
    m_pixelData?.Dispose()
    m_width = width
    m_height = height
    m_pixelData = New SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul)
    m_pixmap = m_pixelData.PeekPixels()
  End Sub

  Public Sub Dispose()
    m_paint?.Dispose()
    m_pixelData?.Dispose()
  End Sub
End Class