Imports Microsoft.Maui.Controls

Partial Public Class PixelGamePage
  Inherits ContentPage

  Private _pixelGameView As PixelGameView
  Private _game As PixelGameEngine

  Public Sub New()
    Xaml.Extensions.LoadFromXaml(Me, GetType(PixelGamePage))
  End Sub

  Protected Overrides Sub OnAppearing()
    MyBase.OnAppearing()

    _game = New ExampleGame
    _pixelGameView = New PixelGameView(_game, 320, 180, 2)
    Content = _pixelGameView

    ' Start the game engine initialization
    _game.Start()

    ' Request keyboard focus for the page
    Me.Focus()
  End Sub

  Protected Overrides Sub OnDisappearing()
    MyBase.OnDisappearing()

    ' Dispose the game view which handles game cleanup
    _pixelGameView?.Dispose()
    _pixelGameView = Nothing

    ' Dispose the game engine
    _game?.Dispose()
    _game = Nothing
  End Sub

  ' Handle keyboard events at page level for better focus management
  Private Sub OnPageKeyboard(sender As Object, e As KeyboardEventArgs) Handles Me.Keyboard
    ' Debug output for troubleshooting
    Debug.WriteLine($"Key pressed: {e.Key}")
  End Sub
End Class