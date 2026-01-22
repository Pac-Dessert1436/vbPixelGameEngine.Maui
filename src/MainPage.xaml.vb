Imports Microsoft.Maui.Controls

Partial Public Class MainPage
  Inherits ContentPage

  Public Sub New()
    Xaml.Extensions.LoadFromXaml(Me, [GetType]())
  End Sub

  ' Note: Just a placeholder for now - subject to change
  Private Async Sub OnStartGameClicked(sender As Object, e As EventArgs)
    Await Navigation.PushAsync(New PixelGamePage)
  End Sub

  ' Note: More methods will be added here as needed
End Class