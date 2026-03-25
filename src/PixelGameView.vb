Imports Microsoft.Maui.Controls
Imports SkiaSharp.Views.Maui.Controls
Imports SkiaSharp.Views.Maui
Imports Microsoft.Maui.Dispatching

''' <summary>
''' Cross-platform game view for rendering and input handling
''' </summary>
Public Class PixelGameView
  Inherits SKCanvasView
  Implements IDisposable

  Private WithEvents Timer As New Timers.Timer
  Private WithEvents TapRecog As New TapGestureRecognizer
  Private WithEvents PtrRecog As New PointerGestureRecognizer
  Private ReadOnly _game As PixelGameEngine
  Private ReadOnly _renderer As SkiaSharpRenderer
  Private ReadOnly _lock As New Object()
  Private Const FRAME_RATE As Integer = 60
  Private _isRunning As Boolean = False
  Private _keyboardHandler As KeyboardHandler
  Private _isDisposed As Boolean

  Public Sub New(game As PixelGameEngine, vpWidth As Integer, vpHeight As Integer, Optional scale As Integer = 1)
    Me._game = game
    _renderer = New SkiaSharpRenderer(vpWidth, vpHeight, scale)

    ' Initialize game & check for errors
    If game.Construct(vpWidth, vpHeight, scale, scale).IsRCodeFail() Then
      ArgumentOutOfRangeException.ThrowIfZero(vpWidth, NameOf(vpWidth))
      ArgumentOutOfRangeException.ThrowIfZero(vpHeight, NameOf(vpHeight))
      ArgumentOutOfRangeException.ThrowIfZero(scale, NameOf(scale))
    End If

    ' Setup gesture recognizers
    GestureRecognizers.Add(TapRecog)
    GestureRecognizers.Add(PtrRecog)

    ' Configure game loop timer
    Timer.Interval = 1000 \ FRAME_RATE

    ' Initialize keyboard handler
    _keyboardHandler = New KeyboardHandler(game)
  End Sub

  Protected Overrides Sub OnHandlerChanged()
    MyBase.OnHandlerChanged()

    If Handler IsNot Nothing Then
      ' Get the parent page
      Dim parentPage = GetParentPage()

      If parentPage IsNot Nothing Then
        ' Register keyboard events
        _keyboardHandler?.RegisterKeyboardEvents(parentPage)

        ' Request focus to enable keyboard input
        Me.Focus()
      End If

      ' Start the game loop
      _isRunning = True
      Timer.Start()
    Else
      ' Clean up when handler is removed
      _isRunning = False
      Timer.Stop()

      ' Unregister keyboard events
      _keyboardHandler?.UnregisterKeyboardEvents(Nothing)
    End If
  End Sub

  Private Function GetParentPage() As Page
    If Me.Parent Is Nothing Then Return Nothing

    Dim element = Me.Parent
    While element IsNot Nothing AndAlso Not TypeOf element Is Page
      element = element.Parent
    End While

    Return TryCast(element, Page)
  End Function

  Protected Overrides Sub OnPaintSurface(e As SKPaintSurfaceEventArgs)
    MyBase.OnPaintSurface(e)
    ' Render the game using SkiaSharp canvas
    _renderer.Render(e.Surface.Canvas)
  End Sub

  ' Touch/Mouse pointer events (cross-platform)
  Private Sub OnTapped(sender As Object, e As TappedEventArgs) Handles TapRecog.Tapped
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      Dim gameX = CInt(Fix(point.Value.X)) \ _renderer.Scale
      Dim gameY = CInt(Fix(point.Value.Y)) \ _renderer.Scale
      _game.SetMousePosition(gameX, gameY)
      ' Simulate mouse click
      _game.SetMouseButtonState(0, True)
      _game.SetMouseButtonState(0, False)
    End If
  End Sub

  Private Sub OnPointerEntered(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerEntered
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      Dim gameX = CInt(Fix(point.Value.X)) \ _renderer.Scale
      Dim gameY = CInt(Fix(point.Value.Y)) \ _renderer.Scale
      _game.SetMousePosition(gameX, gameY)
    End If
  End Sub

  Private Sub OnPointerMoved(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerMoved
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      Dim gameX = CInt(Fix(point.Value.X)) \ _renderer.Scale
      Dim gameY = CInt(Fix(point.Value.Y)) \ _renderer.Scale
      _game.SetMousePosition(gameX, gameY)
    End If
  End Sub

  Private Sub OnPointerExited(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerExited
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      Dim gameX = CInt(Fix(point.Value.X)) \ _renderer.Scale
      Dim gameY = CInt(Fix(point.Value.Y)) \ _renderer.Scale
      _game.SetMousePosition(gameX, gameY)
    End If
  End Sub

  Private Sub OnTimerElapsed(sender As Object, e As EventArgs) Handles Timer.Elapsed
    If Not _isRunning Then Return

    ' Calculate elapsed time for this frame
    Dim elapsedTime As Single = 1.0F / FRAME_RATE

    ' Update keyboard and mouse states
    _game.UpdateKeyStates(elapsedTime)
    _game.UpdateMouseStates(elapsedTime)

    ' Update game state
    If _game.OnUserUpdate(elapsedTime) Then
      ' Render and update display
      If _game.OnUserDraw() Then
        Dim drawTarget As Sprite = _game.GetDrawTarget()
        If drawTarget IsNot Nothing Then
          Dim pixels = drawTarget.GetData()
          SyncLock _lock
            _renderer.UpdatePixels(pixels)
          End SyncLock
        End If
      End If
      ' Trigger repaint on UI thread
      Dispatcher.DispatchAsync(Sub() InvalidateSurface())
    Else
      ' Game wants to exit
      Application.Current.Quit()
    End If
  End Sub

  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not _isDisposed Then
      If disposing Then
        ' Dispose managed state
        _isRunning = False
        Timer.Stop()
        Timer.Dispose()
        _keyboardHandler?.Dispose()
        _renderer?.Dispose()
      End If

      ' Free unmanaged resources
      _isDisposed = True
    End If
  End Sub

  Public Sub Dispose() Implements IDisposable.Dispose
    ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    Dispose(disposing:=True)
    GC.SuppressFinalize(Me)
  End Sub
End Class