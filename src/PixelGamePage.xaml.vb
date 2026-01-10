Imports Microsoft.Maui.Controls
Imports SkiaSharp.Views.Maui.Controls
Imports SkiaSharp.Views.Maui
Imports GlobalKeyboardCapture.Maui.Core.Models

Public Class PixelGamePage
  Inherits ContentPage

  Public Sub New()
    Content = New PixelGameView(Pge, 320, 240, 2)
  End Sub
End Class

Public Class PixelGameView
  Inherits SKCanvasView

  Private WithEvents Timer As New Timers.Timer
  Private WithEvents TapRecog As New TapGestureRecognizer
  Private WithEvents PtrRecog As New PointerGestureRecognizer
  Private ReadOnly game As PixelGameEngine
  Private ReadOnly renderer As SkiaSharpRenderer
  Private mousePos As New Vi2d
  Private ReadOnly _lock As New Object
  Private Const FRAME_RATE As Integer = 60

  Public Sub New(game As PixelGameEngine, vpWidth As Integer, vpHeight As Integer, Optional scale As Integer = 1)
    Me.game = game
    renderer = New SkiaSharpRenderer(vpWidth, vpHeight, scale)
    game.Construct(vpWidth, vpHeight, scale, scale)
    game.SetRenderer(renderer)

    ' Add gesture recognizers
    GestureRecognizers.Add(TapRecog)
    GestureRecognizers.Add(PtrRecog)

    Timer.Interval = 1000 \ FRAME_RATE  ' Approx. 60 FPS
    Timer.Start()
  End Sub

  Protected Overrides Sub OnHandlerChanged()
    MyBase.OnHandlerChanged()
    ' View is now attached to the window
    If Handler IsNot Nothing Then Focus()
  End Sub

  Protected Overrides Sub OnPaintSurface(e As SKPaintSurfaceEventArgs)
    MyBase.OnPaintSurface(e)
    ' Render the game directly with the canvas
    renderer.Render(e.Surface.Canvas)
  End Sub

  Protected Sub OnKeyDown(e As KeyEventArgs)
    ' ToDo: Connect this logic to the lifecycle of `PixelGameEngine` class.
    game.SetKeyStateFromKey(e.Character.Value, True)
  End Sub

  Protected Sub OnKeyUp(e As KeyEventArgs)
    ' ToDo: Connect this logic to the lifecycle of `PixelGameEngine` class.
    game.SetKeyStateFromKey(e.Character.Value, False)
  End Sub

  Private Sub OnTapped(sender As Object, e As TappedEventArgs) Handles TapRecog.Tapped
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      mousePos.x = CInt(point.Value.X) \ renderer.Scale
      mousePos.y = CInt(point.Value.Y) \ renderer.Scale
      game.SetMousePosition(mousePos.x, mousePos.y)
      game.SetMouseButtonState(0, True)
      game.SetMouseButtonState(0, False)
    End If
  End Sub

  Private Sub OnPointerEntered(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerEntered
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      mousePos.x = CInt(point.Value.X) \ renderer.Scale
      mousePos.y = CInt(point.Value.Y) \ renderer.Scale
      game.SetMousePosition(mousePos.x, mousePos.y)
    End If
  End Sub

  Private Sub OnPointerMoved(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerMoved
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      mousePos.x = CInt(point.Value.X) \ renderer.Scale
      mousePos.y = CInt(point.Value.Y) \ renderer.Scale
      game.SetMousePosition(mousePos.x, mousePos.y)
    End If
  End Sub

  Private Sub OnPointerExited(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerExited
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      mousePos.x = CInt(point.Value.X) \ renderer.Scale
      mousePos.y = CInt(point.Value.Y) \ renderer.Scale
      game.SetMousePosition(mousePos.x, mousePos.y)
    End If
  End Sub

  Private Sub OnTimerElapsed(sender As Object, e As EventArgs) Handles Timer.Elapsed
    ' Update game state
    game.Update(1.0F / FRAME_RATE)

    ' Update renderer with pixel data from the game's draw target
    Dim drawTarget As Sprite = game.GetDrawTarget()
    If drawTarget IsNot Nothing Then
      Dim pixels = drawTarget.GetData()
      SyncLock _lock
        renderer.UpdatePixels(pixels)
      End SyncLock
    End If

    ' Redraw the view on the main thread
    Dispatcher.Dispatch(AddressOf InvalidateSurface)
  End Sub

  Protected Sub OnDisappearing()
    Timer.Stop()
    Timer.Dispose()
  End Sub
End Class