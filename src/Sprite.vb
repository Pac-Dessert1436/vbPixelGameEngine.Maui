Imports System.IO
Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.PixelFormats

Public Class Sprite

#If PGE_DBG_OVERDRAW Then
  Private Shared nOverdrawCount As Integer
#End If

  Private m_pixelColData As Pixel()
  Private ReadOnly m_modeSample As Mode

  Public Enum Mode
    Normal
    Periodic
  End Enum

  Public Property Width As Integer
  Public Property Height As Integer

  Public Sub New()
    m_pixelColData = Nothing
    Width = 0
    Height = 0
  End Sub

  Public Sub New(imageFile As String, Optional pack As ResourcePack = Nothing)
    LoadFromFile(imageFile, pack)
  End Sub

  Public Sub New(w As Integer, h As Integer)
    If m_pixelColData IsNot Nothing Then
      Erase m_pixelColData
    End If
    Width = w
    Height = h
    m_pixelColData = New Pixel(Width * Height - 1) {}
    For i = 0 To Width * Height - 1
      m_pixelColData(i) = New Pixel(&H0, &H0, &H0, &HFF)
    Next
  End Sub

  Protected Overrides Sub Finalize()
    If m_pixelColData IsNot Nothing Then Erase m_pixelColData
  End Sub

  Public Function LoadFromPGESprFile(imageFile As String, Optional pack As ResourcePack = Nothing) As PixelGameEngine.RCode

    If m_pixelColData IsNot Nothing Then Erase m_pixelColData

    Dim ReadData As Action(Of Stream) = Sub(iis As Stream)
                                          Dim bytes = New Byte(3) {}
                                          iis.ReadExactly(bytes, 0, 4)
                                          Width = BitConverter.ToInt32(bytes, 0)
                                          iis.ReadExactly(bytes, 0, 4)
                                          Height = BitConverter.ToInt32(bytes, 0)
                                          m_pixelColData = New Pixel(Width * Height - 1) {}
                                          bytes = New Byte(Width * Height * 4 - 1) {}
                                          iis.ReadExactly(bytes, 0, Width * Height * 4)
                                          Buffer.BlockCopy(bytes, 0, m_pixelColData, 0, Width * Height * 4)
                                        End Sub

    If pack Is Nothing Then
      Using ifs = New FileStream(imageFile, FileMode.Open, FileAccess.Read)
        If ifs IsNot Nothing Then
          ReadData(ifs)
          Return PixelGameEngine.RCode.Ok
        Else
          Return PixelGameEngine.RCode.Fail
        End If
      End Using
    Else
      Dim rb = pack.GetFileBuffer(imageFile)
      Dim iss = New MemoryStream(rb.Data)
      ReadData(iss)
    End If

    Return PixelGameEngine.RCode.Fail

  End Function

  Public Function SaveToPGESprFile(imageFile As String) As PixelGameEngine.RCode

    If m_pixelColData Is Nothing Then
      Return PixelGameEngine.RCode.Fail
    End If

    Using ofs = New FileStream(imageFile, FileMode.Create, FileAccess.Write)
      ofs.Write(BitConverter.GetBytes(Width), 0, 4)
      ofs.Write(BitConverter.GetBytes(Height), 0, 4)
      Dim bytes = New Byte(Width * Height * 4 - 1) {}
      Buffer.BlockCopy(m_pixelColData, 0, bytes, 0, Width * Height * 4)
      ofs.Write(bytes, 0, Width * Height * 4)
    End Using

    Return PixelGameEngine.RCode.Ok

  End Function

  Function LoadFromFile(imageFile As String, Optional pack As ResourcePack = Nothing) As PixelGameEngine.RCode

    If String.IsNullOrWhiteSpace(imageFile) Then Return PixelGameEngine.RCode.Ok

    Try
      If pack IsNot Nothing Then
        Dim rb = pack.GetFileBuffer(imageFile)
        If rb.Data Is Nothing Then Return PixelGameEngine.RCode.NoFile
        Using ms As New MemoryStream(rb.Data)
          Using img As Image(Of Rgba32) = Image.Load(Of Rgba32)(ms)
            Width = img.Width
            Height = img.Height
            m_pixelColData = New Pixel(Width * Height - 1) {}
            ProcessImagePixels(img)
          End Using
        End Using
      Else
        Using img As Image(Of Rgba32) = Image.Load(Of Rgba32)(imageFile)
          Width = img.Width
          Height = img.Height
          m_pixelColData = New Pixel(Width * Height - 1) {}
          ProcessImagePixels(img)
        End Using
      End If
      Return PixelGameEngine.RCode.Ok
    Catch ex As Exception
      Return PixelGameEngine.RCode.Fail
    End Try

  End Function

  Private Sub ProcessImagePixels(image As Image(Of Rgba32))
    For y = 0 To Height - 1
      For x = 0 To Width - 1
        Dim color = image(x, y)
        SetPixel(x, y, New Pixel(color.R, color.G, color.B, color.A))
      Next
    Next
  End Sub

  Public Function SetPixel(x As Integer, y As Integer, p As Pixel) As Boolean
    If x >= 0 AndAlso x < Width AndAlso y >= 0 AndAlso y < Height Then
      m_pixelColData(y * Width + x) = p
      Return True
    Else
      Return False
    End If
  End Function

  Public Function GetPixel(x As Integer, y As Integer) As Pixel
    If m_modeSample = Mode.Normal Then
      If x >= 0 AndAlso x < Width AndAlso y >= 0 AndAlso y < Height Then
        Return m_pixelColData(y * Width + x)
      Else
        Return New Pixel(0, 0, 0, &HFF)
      End If
    Else
      Return m_pixelColData(Math.Abs(y Mod Height) * Width + Math.Abs(x Mod Width))
    End If
  End Function

  Public Function Sample(x As Single, y As Single) As Pixel
    Dim sx = Math.Min(CInt(Fix(x * Width)), Width - 1)
    Dim sy = Math.Min(CInt(Fix(y * Height)), Height - 1)
    Return GetPixel(sx, sy)
  End Function

  Public Function SampleBL(u As Single, v As Single) As Pixel

    u = u * Width - 0.5F
    v = v * Height - 0.5F
    Dim x = CInt(Fix(u))
    Dim y = CInt(Fix(v))
    Dim uRatio = u - x
    Dim vRatio = v - y
    Dim uOpposite = 1.0F - uRatio
    Dim vOpposite = 1.0F - vRatio

    Dim p1 = GetPixel(Math.Max(x, 0), Math.Max(y, 0))
    Dim p2 = GetPixel(Math.Min(x + 1, Width - 1), Math.Max(y, 0))
    Dim p3 = GetPixel(Math.Max(x, 0), Math.Min(y + 1, Height - 1))
    Dim p4 = GetPixel(Math.Min(x + 1, Width - 1), Math.Min(y + 1, Height - 1))

    Return New Pixel(CByte((p1.R * uOpposite + p2.R * uRatio) * vOpposite + (p3.R * uOpposite + p4.R * uRatio) * vRatio),
                    CByte((p1.G * uOpposite + p2.G * uRatio) * vOpposite + (p3.G * uOpposite + p4.G * uRatio) * vRatio),
                    CByte((p1.B * uOpposite + p2.B * uRatio) * vOpposite + (p3.B * uOpposite + p4.B * uRatio) * vRatio))

  End Function

  Public ReadOnly Property GetData() As Pixel()
    Get
      Return m_pixelColData
    End Get
  End Property

End Class