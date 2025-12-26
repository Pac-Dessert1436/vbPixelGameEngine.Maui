Public Structure Vf2d

  Public x As Single
  Public y As Single

  Public Sub New(x As Single, y As Single)
    Me.x = x
    Me.y = y
  End Sub

  Public Sub New(vec As Vf2d)
    x = vec.x
    y = vec.y
  End Sub

  Public Function Mag() As Single
    Return MathF.Sqrt(x * x + y * y)
  End Function

  Public Function Mag2() As Single
    Return x * x + y * y
  End Function

  Public Function Norm() As Vf2d
    Dim m = Mag()
    Dim r = If(m <> 0, 1 / m, 0)
    Return New Vf2d(x * r, y * r)
  End Function

  Public Function Perp() As Vf2d
    Return New Vf2d(-y, x)
  End Function

  Public Function Floor() As Vf2d
    Return New Vf2d(MathF.Floor(x), MathF.Floor(y))
  End Function

  Public Function Ceil() As Vf2d
    Return New Vf2d(MathF.Ceiling(x), MathF.Ceiling(y))
  End Function

  Public Function Max(vec As Vf2d) As Vf2d
    Return New Vf2d(MathF.Max(x, vec.x), MathF.Max(y, vec.y))
  End Function

  Public Function Min(vec As Vf2d) As Vf2d
    Return New Vf2d(MathF.Min(x, vec.x), MathF.Min(y, vec.y))
  End Function

  Public Function Cart() As Vf2d
    Return New Vf2d(MathF.Cos(y) * x, MathF.Sin(y) * x)
  End Function

  Public Function Polar() As Vf2d
    Return New Vf2d(Mag(), MathF.Atan2(y, x))
  End Function

  Public Function Clamp(vec1 As Vf2d, vec2 As Vf2d) As Vf2d
    Return Max(vec1).Min(vec2)
  End Function

  Public Function Lerp(vec As Vf2d, t As Double) As Vf2d
    Return Me * CSng(Fix(1.0F - t)) + (vec * CSng(Fix(t)))
  End Function

  Public Function Dot(other As Vf2d) As Integer
    Return CInt(Fix(x * other.x + y * other.y))
  End Function

  Public Function DotF(other As Vf2d) As Single
    Return x * other.x + y * other.y
  End Function

  Public Function Cross(other As Vf2d) As Integer
    Return CInt(Fix(x * other.y - y * other.x))
  End Function

  Public Function CrossF(other As Vf2d) As Single
    Return x * other.y - y * other.x
  End Function

  Public Shared Function Dist(vec1 As Vf2d, vec2 As Vf2d) As Single
    Return (vec1 - vec2).Mag()
  End Function

  Public Shared Function Dist2(vec1 As Vf2d, vec2 As Vf2d) As Single
    Return (vec1 - vec2).Mag2()
  End Function

  Public Shared Function TaxiDist(vec1 As Vf2d, vec2 As Vf2d) As Single
    Return MathF.Abs(vec1.x - vec2.x) + MathF.Abs(vec1.y - vec2.y)
  End Function

  Public Shared Function ChebDist(vec1 As Vf2d, vec2 As Vf2d) As Single
    Return MathF.Max(MathF.Abs(vec1.x - vec2.x), MathF.Abs(vec1.y - vec2.y))
  End Function

  Public Shared Function Angle(vec1 As Vf2d, vec2 As Vf2d) As Single
    Return MathF.Atan2(vec1.y - vec2.y, vec1.x - vec2.x)
  End Function

  Public Function Rotate(radians As Single) As Vf2d
    Dim x = Me.x * MathF.Cos(radians) - Me.y * MathF.Sin(radians)
    Dim y = Me.x * MathF.Sin(radians) + Me.y * MathF.Cos(radians)
    Return New Vf2d(x, y)
  End Function

  Public Function Reflect(normal As Vf2d) As Vf2d
    ' Reflected vector = Original vector - 2 * DotProduct * Normal
    Dim x = Me.x - 2 * DotF(normal) * normal.x
    Dim y = Me.y - 2 * DotF(normal) * normal.y
    Return New Vf2d(x, y)
  End Function

  Public Function LineSegProj(a As Vf2d, b As Vf2d) As Vf2d
    Dim ab = b - a
    Dim ab2 = ab.Mag2()
    If ab2 = 0F Then Return a
    Dim t = (Me - a).DotF(ab) / ab2
    t = ClampF(t, 0F, 1.0F)
    Return a + ab * t
  End Function

  Public Function DistToLineSeg(a As Vf2d, b As Vf2d) As Single
    Return Dist(Me, LineSegProj(a, b))
  End Function

  Public Shared Operator +(left As Vf2d, right As Vf2d) As Vf2d
    Return New Vf2d(left.x + right.x, left.y + right.y)
  End Operator

  Public Shared Operator +(left As Vf2d, right As Single) As Vf2d
    Return New Vf2d(left.x + right, left.y + right)
  End Operator

  Public Shared Operator +(left As Single, right As Vf2d) As Vf2d
    Return New Vf2d(right.x + left, right.y + left)
  End Operator

  Public Shared Operator +(left As Vf2d, right As Vi2d) As Vf2d
    Return New Vf2d(left.x + right.x, left.y + right.y)
  End Operator

  Public Shared Operator -(left As Vf2d, right As Vf2d) As Vf2d
    Return New Vf2d(left.x - right.x, left.y - right.y)
  End Operator

  Public Shared Operator -(left As Vf2d, right As Vi2d) As Vf2d
    Return New Vf2d(left.x - right.x, left.y - right.y)
  End Operator

  Public Shared Operator *(left As Vf2d, right As Single) As Vf2d
    Return New Vf2d(left.x * right, left.y * right)
  End Operator

  Public Shared Operator *(left As Single, right As Vf2d) As Vf2d
    Return New Vf2d(right.x * left, right.y * left)
  End Operator

  Public Shared Operator *(left As Vf2d, right As Vf2d) As Vf2d
    Return New Vf2d(left.x * right.x, left.y * right.y)
  End Operator

  Public Shared Operator /(left As Vf2d, right As Single) As Vf2d
    Return New Vf2d(left.x / right, left.y / right)
  End Operator

  Public Shared Operator /(left As Vf2d, right As Vf2d) As Vf2d
    Return New Vf2d(left.x / right.x, left.y / right.y)
  End Operator

  Public Shared Operator +(vec As Vf2d) As Vf2d
    Return New Vf2d(+vec.x, +vec.y)
  End Operator

  Public Shared Operator -(vec As Vf2d) As Vf2d
    Return New Vf2d(-vec.x, -vec.y)
  End Operator

  Public Shared Operator =(left As Vf2d, right As Vf2d) As Boolean
    Return left.x = right.x AndAlso left.y = right.y
  End Operator

  Public Shared Operator <>(left As Vf2d, right As Vf2d) As Boolean
    Return left.x <> right.x OrElse left.y <> right.y
  End Operator

  Public Function Str() As String
    Return $"({x},{y})"
  End Function

  Public Overrides Function ToString() As String
    Return Str()
  End Function

  Public Shared Widening Operator CType(vec As Vf2d) As Vi2d
    Return New Vi2d(CInt(Fix(vec.x)), CInt(Fix(vec.y)))
  End Operator

  Public Overrides Function Equals(obj As Object) As Boolean
    Return Me = DirectCast(obj, Vf2d)
  End Function

  Public Overrides Function GetHashCode() As Integer
    Return HashCode.Combine(x, y)
  End Function
End Structure