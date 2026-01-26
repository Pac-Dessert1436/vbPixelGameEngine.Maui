Imports CommunityToolkit.Maui.Core.Platform
Imports Microsoft.Maui.Controls

Partial Public Class MainPage
  Inherits ContentPage

  'Private ReadOnly _keyHandlerService As New KeyHandlerService()
  Private Shared _globalKeyHandler As Object

  Public Sub New()
    Xaml.Extensions.LoadFromXaml(Me, [GetType]())
  End Sub

  ' Start the game and navigate to PixelGamePage
  Private Async Sub OnStartGameClicked(sender As Object, e As EventArgs)
    Await Navigation.PushAsync(New PixelGamePage())
  End Sub

  ' Register global keyboard handler for desktop platforms
  Public Shared Sub RegisterGlobalKeyboardHandler()
    ' Initialize platform-specific keyboard handler
#If ANDROID29_0_OR_GREATER Then
    ' Android uses touch/virtual keyboard, handled by PixelGameView
#ElseIf WINDOWS10_0_19041_0_OR_GREATER Then
    ' Windows global keyboard capture
    'Try
    '  ' Note: GlobalKeyboardCapture.Maui is no longer used
    '  _globalKeyHandler = New ?????   ' KEY HANDLER REQUIRED
    'Catch ex As Exception
    '  ' Fallback to MAUI keyboard events
    'End Try
#End If
  End Sub

  ' Cleanup global keyboard handler
  Public Shared Sub UnregisterGlobalKeyboardHandler()
    If _globalKeyHandler IsNot Nothing Then
#If WINDOWS10_0_19041_0_OR_GREATER Then
      TryCast(_globalKeyHandler, IDisposable)?.Dispose()
#End If
      _globalKeyHandler = Nothing
    End If
  End Sub
End Class
