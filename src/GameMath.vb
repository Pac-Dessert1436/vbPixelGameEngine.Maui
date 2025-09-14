Public Module GameMath

  Private m_rnd As New Random
  Private m_seed As Integer

  Public Property RandomSeed As Integer
    Get
      Return m_seed
    End Get
    Set(value As Integer)
      m_seed = value
      m_rnd = New Random(value)
    End Set
  End Property

  Sub New()
    m_seed = Environment.TickCount
  End Sub

  Public Function RandomByte() As Byte
    Return RandomBytes(1)(0)
  End Function

  Public Function RandomBytes(count As Integer) As Byte()
    Dim b(count - 1) As Byte  ' Use traditional array definition as a shorthand.
    m_rnd.NextBytes(b)
    Return b
  End Function

  Public Function RandomInt(min As Integer, max As Integer) As Integer
    Return m_rnd.Next(min, max + 1)  ' It might be better to include upper bound.
  End Function

  Public Function RandomFloat(Optional min As Single = 0, Optional max As Single = 1) As Single
    Return m_rnd.NextSingle() * (max - min) + min
  End Function

  Public Function MinkoDist(vec1 As Vf2d, vec2 As Vf2d, p As Integer) As Single
    If p <= 0 Then Throw New ArgumentException(
      "Minkowski distance requires a positive order parameter (p > 0).", NameOf(p))

    Dim absDiffX = MathF.Abs(vec1.x - vec2.x)
    Dim absDiffY = MathF.Abs(vec1.y - vec2.y)
    ' Handle special cases for p=1 (Manhattan), p=2 (Euclidean) and p->inf (Chebyshev).
    Select Case p
      Case 1
        Return absDiffX + absDiffY
      Case 2
        Return MathF.Sqrt(absDiffX * absDiffX + absDiffY * absDiffY)
      Case Integer.MaxValue
        Return MathF.Max(absDiffX, absDiffY)
      Case Else  ' General formula for Minkowski distance
        Return CSng((absDiffX ^ p + absDiffY ^ p) ^ (1 / p))
    End Select
  End Function

  Public Function JacDist(rectA As RectF, rectB As RectF) As Single
    If rectA.IsEmpty OrElse rectB.IsEmpty Then Return 1.0F
    Dim overlapArea As Single

    With RectF.Intersect(rectA, rectB)
      overlapArea = If(.IsEmpty, 0.0F, .width * .height)
    End With

    Dim areaA As Single = rectA.width * rectA.height
    Dim areaB As Single = rectB.width * rectB.height

    Dim unionArea As Single = areaA + areaB - overlapArea
    If unionArea <= 0.0F Then Return 1.0F

    Return 1.0F - (overlapArea / unionArea)
  End Function

  <Runtime.CompilerServices.Extension>
  Public Function Rotate(vec As Vf2d, radians As Single) As Vf2d
    Dim x = vec.x * MathF.Cos(radians) - vec.y * MathF.Sin(radians)
    Dim y = vec.x * MathF.Sin(radians) + vec.y * MathF.Cos(radians)
    Return New Vf2d(x, y)
  End Function

  <Runtime.CompilerServices.Extension>
  Public Function Rotate(vec As Vi2d, radians As Single) As Vi2d
    Dim x = CInt(Fix(vec.x * MathF.Cos(radians) - vec.y * MathF.Sin(radians)))
    Dim y = CInt(Fix(vec.x * MathF.Sin(radians) + vec.y * MathF.Cos(radians)))
    Return New Vi2d(x, y)
  End Function

  <Runtime.CompilerServices.Extension>
  Public Function Reflect(vec As Vf2d, normal As Vf2d) As Vf2d
    ' Reflected vector = Original vector - 2 * DotProduct * Normal
    Dim x = vec.x - 2 * vec.DotF(normal) * normal.x
    Dim y = vec.y - 2 * vec.DotF(normal) * normal.y
    Return New Vf2d(x, y)
  End Function

  <Runtime.CompilerServices.Extension>
  Public Function Reflect(vec As Vi2d, normal As Vi2d, Optional precise As Boolean = False) As Vi2d
    If precise Then
      Dim resF = New Vf2d(vec.x, vec.y).Reflect(normal)
      Return New Vi2d(CInt(Fix(resF.x)), CInt(Fix(resF.y)))
    Else
      Dim x = vec.x - 2 * vec.Dot(normal) * normal.x
      Dim y = vec.y - 2 * vec.Dot(normal) * normal.y
      Return New Vi2d(Fix(x), Fix(y))
    End If
  End Function
End Module