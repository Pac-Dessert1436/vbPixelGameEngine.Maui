Imports Microsoft.Maui.Controls

Partial Public Class PixelGamePage
  Inherits ContentPage

  Private _pixelGameView As PixelGameView

  Public Sub New()
    Xaml.Extensions.LoadFromXaml(Me, GetType(PixelGamePage))
  End Sub

  Protected Overrides Sub OnAppearing()
    MyBase.OnAppearing()

    ' Create game view with example game instance
    ' User should replace Example with their actual game class
    _pixelGameView = New PixelGameView(Pge, 320, 180, 2)
    Content = _pixelGameView
  End Sub

  Protected Overrides Sub OnDisappearing()
    MyBase.OnDisappearing()
    _pixelGameView?.Dispose()
  End Sub
End Class