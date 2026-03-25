Imports Microsoft.Maui.Controls

Partial Public Class MainPage
  Inherits ContentPage

  Public Sub New()
    Xaml.Extensions.LoadFromXaml(Me, GetType(MainPage))
  End Sub

  ' Start the game and navigate to PixelGamePage
  Private Async Sub OnStartGameClicked(sender As Object, e As EventArgs)
    Await Navigation.PushAsync(New PixelGamePage())
  End Sub
End Class