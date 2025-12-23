Option Strict On
Option Infer On

Public Structure RectI
  Public x As Integer, y As Integer, width As Integer, height As Integer

  Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer)
    Me.x = x
    Me.y = y
    Me.width = width
    Me.height = height
  End Sub

  Public Sub New(position As Vi2d, size As Vi2d)
    Me.New(position.x, position.y, size.x, size.y)
  End Sub

  Public ReadOnly Property Left As Integer
    Get
      Return x
    End Get
  End Property

  Public ReadOnly Property Right As Integer
    Get
      Return x + width
    End Get
  End Property

  Public ReadOnly Property Top As Integer
    Get
      Return y
    End Get
  End Property

  Public ReadOnly Property Bottom As Integer
    Get
      Return y + height
    End Get
  End Property

  Public ReadOnly Property Center As Vf2d
    Get
      Return New Vf2d(x + width \ 2, y + height \ 2)
    End Get
  End Property

  Public ReadOnly Property Location As Vi2d
    Get
      Return New Vi2d(x, y)
    End Get
  End Property

  Public ReadOnly Property Size As Vi2d
    Get
      Return New Vi2d(width, height)
    End Get
  End Property

#Region "MonoGame-style methods"
  Public Function Contains(x As Integer, y As Integer) As Boolean
    Return x >= Left AndAlso x < Right AndAlso y >= Top AndAlso y < Bottom
  End Function

  Public Function Contains(point As Vi2d) As Boolean
    Return Contains(point.x, point.y)
  End Function

  Public Function Contains(rect As RectI) As Boolean
    Return rect.Left >= Left AndAlso rect.Right <= Right AndAlso
      rect.Top >= Top AndAlso rect.Bottom <= Bottom
  End Function

  Public Function Intersects(rect As RectI) As Boolean
    Return Left < rect.Right AndAlso Right > rect.Left AndAlso
      Top < rect.Bottom AndAlso Bottom > rect.Top
  End Function

  Public Sub Offset(x As Integer, y As Integer)
    Me.x += x
    Me.y += y
  End Sub

  Public Sub Offset(point As Vi2d)
    Offset(point.x, point.y)
  End Sub

  Public Sub Inflate(dx As Integer, dy As Integer)
    x -= dx
    y -= dy
    width += dx * 2
    height += dy * 2
  End Sub
#End Region

#Region "Static methods"
  Public Shared Function Intersect(a As RectI, b As RectI) As RectI
    Dim left As Integer = Math.Max(a.Left, b.Left)
    Dim top As Integer = Math.Max(a.Top, b.Top)
    Dim right As Integer = Math.Min(a.Right, b.Right)
    Dim bottom As Integer = Math.Min(a.Bottom, b.Bottom)

    Return If(right < left OrElse bottom < top,
      New RectI, New RectI(left, top, right - left, bottom - top))
  End Function

  Public Shared Function Union(a As RectI, b As RectI) As RectI
    Dim left As Integer = Math.Min(a.Left, b.Left)
    Dim top As Integer = Math.Min(a.Top, b.Top)
    Dim right As Integer = Math.Max(a.Right, b.Right)
    Dim bottom As Integer = Math.Max(a.Bottom, b.Bottom)

    Return New RectI(left, top, right - left, bottom - top)
  End Function
#End Region

  Public ReadOnly Property IsEmpty As Boolean
    Get
      Return width <= 0 OrElse height <= 0
    End Get
  End Property

  Public Overrides Function ToString() As String
    Return $"{{ X:{x} Y:{y} Width:{width} Height:{height} }}"
  End Function
End Structure