Imports System.MathF

Public Module GameMath
  Private m_rnd As New Random
  Private m_seed As Integer

  Private Sub UpdateRNG()
    m_rnd = New Random(m_seed)
  End Sub

  Public Property RandomSeed As Integer
    Get
      Return m_seed
    End Get
    Set(value As Integer)
      m_seed = value
      UpdateRNG()
    End Set
  End Property

  Sub New()
    RefreshRandom()
  End Sub

  Public Sub RefreshRandom()
    m_seed = Environment.TickCount
    UpdateRNG()
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

  Public Function MinkoDist(vec1 As Vf2d, vec2 As Vf2d, p As Single) As Single
    If p <= 0 Then Throw New ArgumentException(
      "Minkowski distance requires a positive order parameter (p > 0).", NameOf(p))

    Dim absDiffX = Abs(vec1.x - vec2.x)
    Dim absDiffY = Abs(vec1.y - vec2.y)
    ' Handle special cases for p=1 (Manhattan), p=2 (Euclidean) and p->inf (Chebyshev).
    Select Case p
      Case 1
        Return absDiffX + absDiffY
      Case 2
        Return Sqrt(absDiffX * absDiffX + absDiffY * absDiffY)
      Case Single.MaxValue
        Return Max(absDiffX, absDiffY)
      Case Else  ' General formula for Minkowski distance
        Return CSng((absDiffX ^ p + absDiffY ^ p) ^ (1 / p))
    End Select
  End Function

  Public Function MinkoDist(vec1 As Vi2d, vec2 As Vi2d, p As Single) As Integer
    If p <= 0 Then Throw New ArgumentException(
      "Minkowski distance requires a positive order parameter (p > 0).", NameOf(p))

    Dim absDiffX = Abs(vec1.x - vec2.x)
    Dim absDiffY = Abs(vec1.y - vec2.y)
    Dim res As Single
    ' Handle special cases for p=1 (Manhattan), p=2 (Euclidean) and p->inf (Chebyshev).
    Select Case p
      Case 1
        res = absDiffX + absDiffY
      Case 2
        res = Sqrt(absDiffX * absDiffX + absDiffY * absDiffY)
      Case Single.MaxValue
        res = Max(absDiffX, absDiffY)
      Case Else  ' General formula for Minkowski distance
        res = CSng((absDiffX ^ p + absDiffY ^ p) ^ (1 / p))
    End Select
    Return CInt(Fix(res))
  End Function

  Public Function Jaccard(rectA As RectF, rectB As RectF) As Single
    If rectA.IsEmpty OrElse rectB.IsEmpty Then Return 0F
    Dim overlapArea As Single

    With RectF.Intersect(rectA, rectB)
      overlapArea = If(.IsEmpty, 0F, .width * .height)
    End With

    Dim areaA As Single = rectA.width * rectA.height
    Dim areaB As Single = rectB.width * rectB.height

    Dim unionArea As Single = areaA + areaB - overlapArea
    Return If(unionArea <= 0F, 0F, overlapArea / unionArea)
  End Function

  Public Function Jaccard(rectA As RectI, rectB As RectI) As Single
    If rectA.IsEmpty OrElse rectB.IsEmpty Then Return 0F
    Dim overlapArea As Integer

    With RectI.Intersect(rectA, rectB)
      overlapArea = If(.IsEmpty, 0, .width * .height)
    End With

    Dim areaA As Integer = rectA.width * rectA.height
    Dim areaB As Integer = rectB.width * rectB.height

    Dim unionArea As Integer = areaA + areaB - overlapArea
    Return If(unionArea <= 0, 0F, CSng(overlapArea / unionArea))
  End Function

  Public Function JaccardDist(rectA As RectF, rectB As RectF) As Single
    Return 1.0F - Jaccard(rectA, rectB)
  End Function

  Public Function JaccardDist(rectA As RectI, rectB As RectI) As Single
    Return 1.0F - Jaccard(rectA, rectB)
  End Function

#Region "Basic scalar utilities"
  Public Function ClampF(value As Single, min As Single, max As Single) As Single
    Return MathF.Max(min, MathF.Min(max, value))
  End Function

  Public Function WrapF(value As Single, min As Single, max As Single) As Single
    Dim range = max - min
    If range = 0 Then Return min
    Dim v As Single = (value - min) Mod range
    If v < 0 Then v += range
    Return v + min
  End Function

  Public Function LerpF(left As Single, right As Single, t As Single) As Single
    Return left + (right - left) * t
  End Function

  Public Function CompareF(left As Single, right As Single, Optional orderOfMag As Integer = 6) As Boolean
    Return Abs(left - right) <= Pow(10.0F, -orderOfMag)
  End Function

  Public Function RepeatF(num As Single, length As Single) As Single
    Return ClampF(num - Floor(num / length) * length, 0, length)
  End Function

  Public Function PingPongF(num As Single, length As Single) As Single
    num = RepeatF(num, length * 2)
    Return length - Abs(num - length)
  End Function

  Public Function SmoothStepF(edge0 As Single, edge1 As Single, x As Single) As Single
    Dim t = ClampF((x - edge0) / (edge1 - edge0), 0F, 1.0F)
    Return t * t * (3.0F - 2.0F * t)
  End Function

  Public Function Repeat(num As Integer, length As Integer) As Integer
    Return Math.Clamp(num - num \ length * length, 0, length)
  End Function

  Public Function PingPong(num As Integer, length As Integer) As Integer
    num = Repeat(num, length * 2)
    Return length - Math.Abs(num - length)
  End Function

  Public Function SmoothStep(edge0 As Integer, edge1 As Integer, x As Integer) As Integer
    Dim t = Math.Clamp((x - edge0) \ (edge1 - edge0), 0, 1)
    Return t * t * (3 - 2 * t)
  End Function
#End Region

#Region "Angle helpers measured in radians"
  Public Function NormalizeAngle(rad As Single) As Single
    Dim r = rad Mod Tau
    If r <= -PI Then r += Tau
    If r > PI Then r -= Tau
    Return r
  End Function

  Public Function DeltaAngle(alpha As Single, beta As Single) As Single
    Return NormalizeAngle(beta - alpha)
  End Function
#End Region

  <Runtime.CompilerServices.Extension>
  Public Function MoveTowards(current As Single, target As Single, maxDelta As Single) As Single
    Dim diff = target - current
    If Abs(diff) <= maxDelta Then Return target
    Return current + Sign(diff) * maxDelta
  End Function

  <Runtime.CompilerServices.Extension>
  Public Function MoveTowards(current As Vf2d, target As Vf2d, maxDelta As Single) As Vf2d
    Dim toTarget = target - current
    Dim dist = toTarget.Mag()
    If dist <= maxDelta OrElse dist = 0 Then Return target
    Return current + toTarget * (maxDelta / dist)
  End Function

  <Runtime.CompilerServices.Extension>
  Public Function MoveTowards(current As Vi2d, target As Vi2d, maxDelta As Integer) As Vi2d
    Dim resF = MoveTowards(New Vf2d(current), New Vf2d(target), CSng(maxDelta))
    Return New Vi2d(CInt(Fix(resF.x)), CInt(Fix(resF.y)))
  End Function

  ' Line-segment intersection (returns True and intersection point if segments intersect)
  Public Function LineSegIntersect(a1 As Vf2d, a2 As Vf2d, b1 As Vf2d, b2 As Vf2d,
                                   <Runtime.InteropServices.Out> ByRef i As Vf2d) As Boolean
    Dim r = a2 - a1
    Dim s = b2 - b1
    Dim rxs = r.CrossF(s)
    Dim qp = b1 - a1
    Dim qpxr = qp.CrossF(r)
    If Abs(rxs) < 0.000001F Then
      i = New Vf2d(0, 0)
      Return False
    End If
    Dim t = qp.CrossF(s) / rxs
    Dim u = qpxr / rxs
    If t >= 0F AndAlso t <= 1.0F AndAlso u >= 0F AndAlso u <= 1.0F Then
      i = a1 + r * t
      Return True
    End If
    i = New Vf2d(0, 0)
    Return False
  End Function

#Region "Bezier/spline curve tools"
  Public Function QuadraticBezier(p0 As Vf2d, p1 As Vf2d, p2 As Vf2d, t As Single) As Vf2d
    Dim u = 1.0F - t
    Return New Vf2d(
      x:=p0.x * (u * u) + p1.x * (2.0F * u * t) + p2.x * (t * t),
      y:=p0.y * (u * u) + p1.y * (2.0F * u * t) + p2.y * (t * t)
    )
  End Function

  Public Function CubicBezier(p0 As Vf2d, p1 As Vf2d, p2 As Vf2d, p3 As Vf2d, t As Single) As Vf2d
    Dim u = 1.0F - t
    Dim u3 = u * u * u
    Dim t3 = t * t * t
    Return New Vf2d(
      x:=p0.x * u3 + p1.x * (3.0F * u * u * t) + p2.x * (3.0F * u * t * t) + p3.x * t3,
      y:=p0.y * u3 + p1.y * (3.0F * u * u * t) + p2.y * (3.0F * u * t * t) + p3.y * t3
    )
  End Function

  Public Function CatmullRom(p0 As Vf2d, p1 As Vf2d, p2 As Vf2d, p3 As Vf2d, t As Single,
                             Optional normalized As Boolean = False) As Vf2d
    Dim t2 As Single = t * t, t3 As Single = t2 * t
    Dim scale = If(normalized, 0.5F, 1.0F)

    Dim coeff0 = (-t + 2 * t2 - t3) * scale
    Dim coeff1 = (2 - 5 * t2 + 3 * t3) * scale
    Dim coeff2 = (t + 4 * t2 - 3 * t3) * scale
    Dim coeff3 = (-t2 + t3) * scale
    Return New Vf2d(
      x:=p0.x * coeff0 + p1.x * coeff1 + p2.x * coeff2 + p3.x * coeff3,
      y:=p0.y * coeff0 + p1.y * coeff1 + p2.y * coeff2 + p3.y * coeff3
    )
  End Function

  Public Function Hermite(p0 As Vf2d, t0 As Vf2d, p1 As Vf2d, t1 As Vf2d, t As Single) As Vf2d
    Dim t2 As Single = t * t, t3 As Single = t2 * t
    Dim h00 = 2.0F * t3 - 3.0F * t2 + 1.0F
    Dim h10 = t3 - 2.0F * t2 + t
    Dim h01 = -2.0F * t3 + 3.0F * t2
    Dim h11 = t3 - t2
    Return New Vf2d(
      x:=p0.x * h00 + t0.x * h10 + p1.x * h01 + t1.x * h11,
      y:=p0.y * h00 + t0.y * h10 + p1.y * h01 + t1.y * h11
    )
  End Function
#End Region
End Module