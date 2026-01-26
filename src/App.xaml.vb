Imports Microsoft.Maui.Controls

Partial Public Class App
  Inherits Application

  Public Sub New()
    InitializeComponent()
    MainPage = New MainPage()
  End Sub

  Protected Overrides Sub OnStart()
    ' Handle when your app starts
  End Sub

  Protected Overrides Sub OnSleep()
    ' Handle when your app sleeps
  End Sub

  Protected Overrides Sub OnResume()
    ' Handle when your app resumes
  End Sub
End Class
