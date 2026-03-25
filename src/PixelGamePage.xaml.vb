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

    ' Create game instance - User should replace ExampleGame with their actual game class
    _game = New ExampleGame
    _pixelGameView = New PixelGameView(_game, 320, 180, 2)
    Content = _pixelGameView

    ' Start the game engine initialization
    _game.Start()
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
End Class

' Example game class - Users should inherit from PixelGameEngine
Public Class ExampleGame
  Inherits PixelGameEngine

  Private m_playerX As Single = 10.0F
  Private m_playerY As Single = 10.0F

  Public Sub New()
    Title = "Example Game"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    ' Initialize game resources here
    m_playerX = CSng(ScreenWidth / 2)
    m_playerY = CSng(ScreenHeight / 2)
    Return True
  End Function

  Protected Friend Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    ' Handle keyboard input
    If GetKey(Key.LEFT).Held Then
      m_playerX -= 100.0F * elapsedTime
    End If
    If GetKey(Key.RIGHT).Held Then
      m_playerX += 100.0F * elapsedTime
    End If
    If GetKey(Key.UP).Held Then
      m_playerY -= 100.0F * elapsedTime
    End If
    If GetKey(Key.DOWN).Held Then
      m_playerY += 100.0F * elapsedTime
    End If

    ' Clear screen and draw player
    Clear(Presets.DarkBlue)
    FillRect(CInt(Fix(m_playerX)), CInt(Fix(m_playerY)), 8, 8, Presets.Yellow)

    ' Display instructions
    DrawString(2, 2, "Arrow keys to move", Presets.White)
    DrawString(2, 12, $"X: {CInt(Fix(m_playerX))} Y: {CInt(Fix(m_playerY))}", Presets.Gray)

    Return True
  End Function

  Protected Friend Overrides Function OnUserDraw() As Boolean
    ' Optional: Additional rendering logic if needed
    ' Most rendering is done in OnUserUpdate for compatibility with the original olcPGE API
    Return True
  End Function

  Protected Overrides Function OnUserDestroy() As Boolean
    ' Cleanup resources
    Return True
  End Function
End Class
