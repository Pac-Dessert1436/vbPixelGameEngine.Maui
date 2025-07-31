Imports Microsoft.Maui.Controls

Public Class MauiPixelGameView
  Inherits GraphicsView

  Private WithEvents Timer As New Timers.Timer
  Private WithEvents TapRecog As New TapGestureRecognizer
  Private WithEvents PtrRecog As New PointerGestureRecognizer
  Private ReadOnly game As PixelGameEngine

  Public Sub New(game As PixelGameEngine, vpWidth As Integer, vpHeight As Integer)
    Me.game = game
    game.Construct(vpWidth, vpHeight, fullScreen:=True)
    Timer.Interval = 16  ' Approx. 60 FPS
    Timer.Start()
  End Sub

  Private Sub OnTapped(sender As Object, e As TappedEventArgs) Handles TapRecog.Tapped
    Dim point = e.GetPosition(Me)
    Static mousePos As New Vi2d
    If point.HasValue Then
      mousePos.x = CInt(point.Value.X)
      mousePos.y = CInt(point.Value.Y)
    End If
    ' ToDo: Handle the updated mouse position, or rather the updated pointer position on
    '       Android devices or something.
  End Sub

  Private Sub OnPointerEntered(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerEntered
    Dim point = e.GetPosition(Me)
    Static mousePos As New Vi2d
    If point.HasValue Then
      mousePos.x = CInt(point.Value.X)
      mousePos.y = CInt(point.Value.Y)
    End If
  End Sub

  Private Sub OnPointerMoved(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerMoved
    Dim point = e.GetPosition(Me)
    Static mousePos As New Vi2d
    If point.HasValue Then
      mousePos.x = CInt(point.Value.X)
      mousePos.y = CInt(point.Value.Y)
    End If
    ' ToDo: Handle the updated mouse position, or rather the updated pointer position on
    '       Android devices or something.
  End Sub

  Private Sub OnPointerExited(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerExited
    Dim point = e.GetPosition(Me)
    Static mousePos As New Vi2d
    If point.HasValue Then
      mousePos.x = CInt(point.Value.X)
      mousePos.y = CInt(point.Value.Y)
    End If
    ' ToDo: Handle the updated mouse position, or rather the updated pointer position on
    '       Android devices or something.
  End Sub

  Private Sub OnTimerElapsed(sender As Object, e As EventArgs) Handles Timer.Elapsed
    ' ToDo: The current game state will be updated here.
    Invalidate()
  End Sub
End Class
