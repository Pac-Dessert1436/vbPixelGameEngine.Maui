Imports Microsoft.Maui.Controls
Imports SkiaSharp
Imports SkiaSharp.Views.Maui
Imports SkiaSharp.Views.Maui.Controls

Namespace Platforms.Android
  Public Class MauiPixelGameView
    Inherits SKCanvasView

    Private WithEvents Timer As New Timers.Timer
    Private WithEvents TapRecog As New TapGestureRecognizer
    Private WithEvents PtrRecog As New PointerGestureRecognizer
    Private ReadOnly game As PixelGameEngine
    Private ReadOnly renderer As SkiaSharpRenderer

    Public Sub New(game As PixelGameEngine, vpWidth As Integer, vpHeight As Integer, Optional scale As Integer = 1)
      Me.game = game
      renderer = New SkiaSharpRenderer(vpWidth, vpHeight, scale)
      game.Construct(vpWidth, vpHeight, scale, scale)
      game.SetRenderer(renderer)

      Timer.Interval = 16  ' Approx. 60 FPS
      Timer.Start()

      ' Add gesture recognizers
      GestureRecognizers.Add(TapRecog)
      GestureRecognizers.Add(PtrRecog)
    End Sub

    Protected Overrides Sub OnPaintSurface(e As SKPaintSurfaceEventArgs)
      MyBase.OnPaintSurface(e)
      ' Clear the canvas
      e.Surface.Canvas.Clear(SKColors.Black)
      ' Render the game directly with the canvas
      renderer.Render(e.Surface.Canvas)
    End Sub

    Private Sub OnTapped(sender As Object, e As TappedEventArgs) Handles TapRecog.Tapped
      Dim point = e.GetPosition(Me)
      Static mousePos As New Vi2d
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
      Static mousePos As New Vi2d
      If point.HasValue Then
        mousePos.x = CInt(point.Value.X) \ renderer.Scale
        mousePos.y = CInt(point.Value.Y) \ renderer.Scale
        game.SetMousePosition(mousePos.x, mousePos.y)
      End If
    End Sub

    Private Sub OnPointerMoved(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerMoved
      Dim point = e.GetPosition(Me)
      Static mousePos As New Vi2d
      If point.HasValue Then
        mousePos.x = CInt(point.Value.X) \ renderer.Scale
        mousePos.y = CInt(point.Value.Y) \ renderer.Scale
        game.SetMousePosition(mousePos.x, mousePos.y)
      End If
    End Sub

    Private Sub OnPointerExited(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerExited
      Dim point = e.GetPosition(Me)
      Static mousePos As New Vi2d
      If point.HasValue Then
        mousePos.x = CInt(point.Value.X) \ renderer.Scale
        mousePos.y = CInt(point.Value.Y) \ renderer.Scale
        game.SetMousePosition(mousePos.x, mousePos.y)
      End If
    End Sub

    Private Sub OnTimerElapsed(sender As Object, e As EventArgs) Handles Timer.Elapsed
      ' Update game state
      game.Update(0.016F) ' ~60 FPS
      ' Update renderer with pixel data from the game's draw target
      Dim drawTarget As Sprite = game.GetDrawTarget()
      If drawTarget IsNot Nothing Then
        renderer.UpdatePixels(drawTarget.GetData())
      End If
      ' Redraw the view
      InvalidateSurface()
    End Sub
  End Class
End Namespace