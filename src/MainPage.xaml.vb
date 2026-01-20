Imports Microsoft.Maui.Controls

Partial Public Class MainPage
  Inherits ContentPage

  Public Sub New()
    'InitializeComponent()
  End Sub

  Private Async Sub OnStartGameClicked(sender As Object, e As EventArgs)
    Await Navigation.PushAsync(New PixelGamePage())
  End Sub
End Class