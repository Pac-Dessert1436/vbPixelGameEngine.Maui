Option Strict On
Option Infer On

Public Class SpriteSheet
  Private ReadOnly sheet As Sprite, frameW As Integer, frameH As Integer

  Public ReadOnly Property Rows As Integer
  Public ReadOnly Property Columns As Integer
  Public Property CurrFrame As Sprite

  Private ReadOnly Property AnimFrames As New Dictionary(Of String, List(Of Sprite))
  Private ReadOnly Property AllFrameIndices As (rowIdx As Integer, colIdx As Integer)()

  Private Shared _pauseAllAnim As Boolean
  Public Shared Property PauseAllAnimations As Boolean
    Private Get
      Return _pauseAllAnim
    End Get
    Set(value As Boolean)
      _pauseAllAnim = value
    End Set
  End Property

  Public Sub New(sprite As Sprite, frameScale As Vi2d)
    sheet = sprite
    frameW = frameScale.x
    frameH = frameScale.y

    Columns = sheet.Width \ frameW
    Rows = sheet.Height \ frameH

    ReDim AllFrameIndices(Rows * Columns - 1)
    Dim i As Integer = 0
    For row As Integer = 0 To Rows - 1
      For col As Integer = 0 To Columns - 1
        AllFrameIndices(i) = (row, col)
        i += 1
      Next col
    Next row
  End Sub

  Public Sub New(imgPath As String, frameScale As Vi2d)
    Me.New(New Sprite(imgPath), frameScale)
  End Sub

  Default Public ReadOnly Property Frame(rowIdx As Integer, colIdx As Integer) As Sprite
    Get
      ArgumentNullException.ThrowIfNull(Me)
      If rowIdx >= Rows OrElse colIdx >= Columns Then
        Throw New ArgumentException("Row index or column index is out of range.")
      End If
      Dim output As New Sprite(frameW, frameH)
      For i As Integer = 0 To frameW - 1
        For j As Integer = 0 To frameH - 1
          Dim xPos As Integer = colIdx * frameW + i
          Dim yPos As Integer = rowIdx * frameH + j
          output.SetPixel(i, j, sheet.GetPixel(xPos, yPos))
        Next j
      Next i
      Return output
    End Get
  End Property

  Public Function CreateTileMap(start As (row As Integer, col As Integer),
      [end] As (row As Integer, col As Integer)) As List(Of Sprite)

    Dim outputTileMap As New List(Of Sprite)

    Dim tileMapStartIdx = Array.IndexOf(AllFrameIndices, (start.row, start.col))
    Dim tileMapEndIdx = Array.IndexOf(AllFrameIndices, ([end].row, [end].col))

    For idx As Integer = tileMapStartIdx To tileMapEndIdx
      Dim row = AllFrameIndices(idx).rowIdx, col = AllFrameIndices(idx).colIdx
      outputTileMap.Add(Frame(row, col))
    Next idx
    Return outputTileMap
  End Function

  Public Sub DefineAnimation(name As String, start As (row As Integer, col As Integer),
                             [end] As (row As Integer, col As Integer))
    AnimFrames(name) = CreateTileMap(start, [end])
  End Sub

  Public Sub PlayAnimation(name As String, frameDuration As Single,
                           Optional isLooping As Boolean = True,
                           Optional isAnimPaused As Boolean = False)
    Static framePointer As IEnumerator(Of Sprite), prevName As String
    If name <> prevName Then
      If framePointer IsNot Nothing Then framePointer.Reset()
      framePointer = AnimFrames(name).GetEnumerator()
      prevName = name
    End If

    Static frameTimer As Single = 0F
    If AnimFrames(name).Count = 0 Then
      Throw New InvalidOperationException("Animation frames cannot be empty.")
    End If
    CurrFrame = framePointer.Current
    If isAnimPaused OrElse PauseAllAnimations Then Exit Sub
    frameTimer += Pge.GetElapsedTime()
    If frameTimer < frameDuration Then Exit Sub

    If framePointer.MoveNext() Then
      frameTimer = 0
    ElseIf isLooping Then
      framePointer.Reset()
      framePointer.MoveNext()  ' Move to the first frame.
    End If
  End Sub
End Class