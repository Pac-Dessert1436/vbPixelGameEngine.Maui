Public Structure Vi2d

  ' See comments below re: Option Strict; probably fine, but wanted to note that
  ' before doing so code was "working" due to automatic type conversion; but now
  ' it is strict... so there could be differences?

  Public x As Integer
  Public y As Integer

  Public Sub New(x As Integer, y As Integer)
    Me.x = x
    Me.y = y
  End Sub

  Public Sub New(vec As Vi2d)
    x = vec.x
    y = vec.y
  End Sub

  Public Function Mag() As Integer
    Return CInt(Fix(MathF.Sqrt(x * x + y * y)))
  End Function

  Public Function Mag2() As Integer
    Return x * x + y * y
  End Function

  Public Function Norm() As Vi2d
    Dim r = CInt(1 / Mag())
    Return New Vi2d(x * r, y * r)
  End Function

  Public Function Perp() As Vi2d
    Return New Vi2d(-y, x)
  End Function

  Public Function Floor() As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Floor(x)), CInt(MathF.Floor(y)))
  End Function

  Public Function Ceil() As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Ceiling(x)), CInt(MathF.Ceiling(y)))
  End Function

  Public Function Max(vec As Vi2d) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Max(x, vec.x)), CInt(MathF.Max(y, vec.y)))
  End Function

  Public Function Min(vec As Vi2d) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Min(x, vec.x)), CInt(MathF.Min(y, vec.y)))
  End Function

  Public Function Cart() As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Cos(y) * x), CInt(MathF.Sin(y) * x))
  End Function

  Public Function Polar() As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(Mag(), CInt(MathF.Atan2(y, x)))
  End Function

  Public Function Clamp(vec1 As Vi2d, vec2 As Vi2d) As Vi2d
    Return Max(vec1).Min(vec2)
  End Function

  Public Function Lerp(vec As Vi2d, t As Double) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return Me * CInt(Fix(1.0F - t)) + (vec * CInt(Fix(t)))
  End Function

  Public Function Dot(other As Vi2d) As Integer
    Return x * other.x + y * other.y
  End Function

  Public Function Cross(other As Vi2d) As Integer
    Return x * other.y - y * other.x
  End Function

  Public Shared Function Dist(vec1 As Vi2d, vec2 As Vi2d) As Integer
    Return (vec1 - vec2).Mag()
  End Function

  Public Shared Function Dist2(vec1 As Vi2d, vec2 As Vi2d) As Integer
    Return (vec1 - vec2).Mag2()
  End Function

  Public Shared Function TaxiDist(vec1 As Vi2d, vec2 As Vi2d) As Integer
    Return Math.Abs(vec1.x - vec2.x) + Math.Abs(vec1.y - vec2.y)
  End Function

  Public Shared Function ChebDist(vec1 As Vi2d, vec2 As Vi2d) As Integer
    Return Math.Max(Math.Abs(vec1.x - vec2.x), Math.Abs(vec1.y - vec2.y))
  End Function

  Public Shared Function Angle(vec1 As Vi2d, vec2 As Vi2d) As Single
    Return MathF.Atan2(vec1.y - vec2.y, vec1.x - vec2.x)
  End Function

  Public Function Rotate(radians As Single) As Vi2d
    Dim x = CInt(Fix(Me.x * MathF.Cos(radians) - Me.y * MathF.Sin(radians)))
    Dim y = CInt(Fix(Me.x * MathF.Sin(radians) + Me.y * MathF.Cos(radians)))
    Return New Vi2d(x, y)
  End Function

  Public Function Reflect(normal As Vi2d, Optional precise As Boolean = False) As Vi2d
    If precise Then
      Dim resF = New Vf2d(x, y).Reflect(normal)
      Return New Vi2d(CInt(Fix(resF.x)), CInt(Fix(resF.y)))
    Else
      Dim x = Me.x - 2 * Dot(normal) * normal.x
      Dim y = Me.y - 2 * Dot(normal) * normal.y
      Return New Vi2d(Fix(x), Fix(y))
    End If
  End Function

  Public Function LineSegProj(a As Vi2d, b As Vi2d) As Vi2d
    Dim ab = b - a
    Dim ab2 = ab.Mag2()
    If ab2 = 0F Then Return a
    Dim t = CSng((Me - a).Dot(ab) / ab2)
    t = ClampF(t, 0F, 1.0F)
    Return a + New Vi2d(CInt(Fix(ab.x * t)), CInt(Fix(ab.y * t)))
  End Function

  Public Function DistToLineSeg(a As Vi2d, b As Vi2d) As Integer
    Return Fix(Dist(Me, LineSegProj(a, b)))
  End Function

  Public Shared Operator +(left As Vi2d, right As Vi2d) As Vi2d
    Return New Vi2d(left.x + right.x, left.y + right.y)
  End Operator

  Public Shared Operator +(left As Vi2d, right As Vf2d) As Vi2d
    Return New Vi2d(left.x + CInt(Fix(right.x)), left.y + CInt(Fix(right.y)))
  End Operator

  Public Shared Operator -(left As Vi2d, right As Vi2d) As Vi2d
    Return New Vi2d(left.x - right.x, left.y - right.y)
  End Operator

  Public Shared Operator -(left As Vi2d, right As Vf2d) As Vi2d
    Return New Vi2d(left.x - CInt(right.x), left.y - CInt(right.y))
  End Operator

  Public Shared Operator *(left As Vi2d, right As Integer) As Vi2d
    Return New Vi2d(left.x * right, left.y * right)
  End Operator

  Public Shared Operator *(left As Vi2d, right As Vi2d) As Vi2d
    Return New Vi2d(left.x * right.x, left.y * right.y)
  End Operator

  Public Shared Operator /(left As Vi2d, right As Integer) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(Fix(left.x / right)), CInt(Fix(left.y / right)))
  End Operator

  Public Shared Operator /(left As Vi2d, right As Vi2d) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(Fix(left.x / right.x)), CInt(Fix(left.y / right.y)))
  End Operator

  'Public Shared Operator +=(left As Vi2d, right As Vi2d) As Vi2d
  '  left.x += right.x
  '  left.y += right.y
  '  Return left
  'End Operator

  'Public Shared Operator -=(left As Vi2d, right As Vi2d) As Vi2d
  '  left.x -= right.x
  '  left.y -= right.y
  '  Return left
  'End Operator

  'Public Shared Operator *=(left As Vi2d, right As Integer) As Vi2d
  '  left.x *= right
  '  left.y *= right
  '  Return left
  'End Operator

  'Public Shared Operator /=(left As Vi2d, right As Integer) As Vi2d
  '  left.x /= right
  '  left.y /= right
  '  Return left
  'End Operator

  'Public Shared Operator *=(left As Vi2d, right As Vi2d) As Vi2d
  '  left.x *= right.x
  '  left.y *= right.y
  '  Return left
  'End Operator

  'Public Shared Operator /=(left As Vi2d, right As Vi2d) As Vi2d
  '  left.x /= right.x
  '  left.y /= right.y
  '  Return left
  'End Operator

  Public Shared Operator +(vec As Vi2d) As Vi2d
    Return New Vi2d(+vec.x, +vec.y)
  End Operator

  Public Shared Operator -(vec As Vi2d) As Vi2d
    Return New Vi2d(-vec.x, -vec.y)
  End Operator

  Public Shared Operator =(left As Vi2d, right As Vi2d) As Boolean
    Return left.x = right.x AndAlso left.y = right.y
  End Operator

  Public Shared Operator <>(left As Vi2d, right As Vi2d) As Boolean
    Return left.x <> right.x OrElse left.y <> right.y
  End Operator

  Public Function Str() As String
    Return $"({x},{y})"
  End Function

  Public Overrides Function ToString() As String
    Return Str()
  End Function

  'Public Shared Widening Operator CType(v As Vi2d) As Vi2d
  '  Return New Vi2d(v.x, v.y)
  'End Operator

  Public Shared Widening Operator CType(vec As Vi2d) As Vf2d
    Return New Vf2d(vec.x, vec.y)
  End Operator

  'Public Shared Widening Operator CType(v As Vi2d) As v2d_generic(Of Double)
  '  Return New v2d_generic(Of Double)(v.x, v.y)
  'End Operator

  Public Overrides Function Equals(obj As Object) As Boolean
    Return Me = DirectCast(obj, Vi2d)
  End Function

  Public Overrides Function GetHashCode() As Integer
    Return HashCode.Combine(x, y)
  End Function

End Structure