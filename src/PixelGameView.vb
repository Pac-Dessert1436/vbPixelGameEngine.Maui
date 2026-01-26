Imports Microsoft.Maui.Controls
Imports SkiaSharp.Views.Maui.Controls
Imports SkiaSharp.Views.Maui
Imports CommunityToolkit.Maui.Core.Platform

''' <summary>
''' Cross-platform game view for rendering and input handling
''' </summary>
Public Class PixelGameView
  Inherits SKCanvasView

  Private WithEvents Timer As New Timers.Timer
  Private WithEvents TapRecog As New TapGestureRecognizer
  Private WithEvents PtrRecog As New PointerGestureRecognizer
  Private ReadOnly _game As PixelGameEngine
  Private ReadOnly _renderer As SkiaSharpRenderer
  Private ReadOnly _lock As New Object()
  Private Const FRAME_RATE As Integer = 60
  Private _isRunning As Boolean = False

  Public Sub New(game As PixelGameEngine, vpWidth As Integer, vpHeight As Integer, Optional scale As Integer = 1)
    Me._game = game
    _renderer = New SkiaSharpRenderer(vpWidth, vpHeight, scale)

    ' Initialize game
    If game.Construct(vpWidth, vpHeight, scale, scale) <> PixelGameEngine.RCode.Ok Then
      Throw New Exception("Failed to construct game engine")
    End If
    game.SetRenderer(_renderer)

    ' Setup gesture recognizers
    GestureRecognizers.Add(TapRecog)
    GestureRecognizers.Add(PtrRecog)

    ' Configure game loop timer
    Timer.Interval = 1000 \ FRAME_RATE
  End Sub

  Protected Overrides Sub OnHandlerChanged()
    MyBase.OnHandlerChanged()

    ' View is attached to window
    If Handler IsNot Nothing Then
      ' Request focus to receive keyboard events (desktop platforms)
      Focus()
      _isRunning = True
      Timer.Start()
      RegisterPlatformInput()
    Else
      ' View is detached from window
      _isRunning = False
      Timer.Stop()
      UnregisterPlatformInput()
    End If
  End Sub

  Protected Overrides Sub OnPaintSurface(e As SKPaintSurfaceEventArgs)
    MyBase.OnPaintSurface(e)
    ' Render the game using SkiaSharp canvas
    _renderer.Render(e.Surface.Canvas)
  End Sub

  Private Sub RegisterPlatformInput()
#If WINDOWS10_0_19041_0_OR_GREATER Then
    ' Windows: Register keyboard and mouse events
    'RegisterWindowsInput()
#ElseIf ANDROID29_0_OR_GREATER Then
    ' Android: Input handled via gesture recognizers (already added)
#End If
  End Sub

  Private Sub UnregisterPlatformInput()
#If WINDOWS10_0_19041_0_OR_GREATER Then
    'UnregisterWindowsInput()
#End If
  End Sub

#Region "FixMe: This part of code cannot be compiled for lack of KeyboardEventArgs"
  ' ToDo: Make full use of input handlers & events with MAUI Community Toolkit.

#If WINDOWS10_0_19041_0_OR_GREATER Then
  ' Windows-specific input handling
  'Private _windowsKeyHandler As ?????  ' KEY HANDLER NEEDED

  'Private Sub RegisterWindowsInput()
  '  Try
  '    _windowsKeyHandler = New ?????  ' KEY HANDLER NEEDED
  '    AddHandler _windowsKeyHandler.KeyDown, AddressOf OnGlobalKeyDown
  '    AddHandler _windowsKeyHandler.KeyUp, AddressOf OnGlobalKeyUp
  '  Catch ex As Exception
  '    ' Global keyboard capture may not be available, fall back to MAUI events
  '  End Try
  'End Sub

  'Private Sub UnregisterWindowsInput()
  '  If _windowsKeyHandler IsNot Nothing Then
  '    RemoveHandler _windowsKeyHandler.KeyDown, AddressOf OnGlobalKeyDown
  '    RemoveHandler _windowsKeyHandler.KeyUp, AddressOf OnGlobalKeyUp
  '    _windowsKeyHandler?.Dispose()
  '    _windowsKeyHandler = Nothing
  '  End If
  'End Sub
  '
  'Private Sub OnGlobalKeyDown(sender As Object, e As ?????)  ' EVENT ARGUMENTS NEEDED
  '  _game?.SetKeyStateFromKey(e.Key.ToString(), True)
  'End Sub

  'Private Sub OnGlobalKeyUp(sender As Object, e As ?????)  ' EVENT ARGUMENTS NEEDED
  '  _game?.SetKeyStateFromKey(e.Key.ToString(), False)
  'End Sub
#End If

  ' MAUI keyboard events (works on all platforms but requires focus)
  'Protected Overrides Sub OnKeyDown(e As KeyboardEventArgs)
  '  MyBase.OnKeyDown(e)
  '  _game?.SetKeyStateFromKey(e.Key.ToString(), True)
  'End Sub

  'Protected Overrides Sub OnKeyUp(e As KeyboardEventArgs)
  '  MyBase.OnKeyUp(e)
  '  _game?.SetKeyStateFromKey(e.Key.ToString(), False)
  'End Sub
#End Region

  ' Touch/Mouse pointer events (cross-platform)
  Private Sub OnTapped(sender As Object, e As TappedEventArgs) Handles TapRecog.Tapped
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      Dim gameX = CInt(Fix(point.Value.X)) \ _renderer.Scale
      Dim gameY = CInt(Fix(point.Value.Y)) \ _renderer.Scale
      _game.SetMousePosition(gameX, gameY)
      ' Simulate mouse click
      _game.SetMouseButtonState(0, True)
      _game.SetMouseButtonState(0, False)
    End If
  End Sub

  Private Sub OnPointerEntered(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerEntered
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      Dim gameX = CInt(Fix(point.Value.X)) \ _renderer.Scale
      Dim gameY = CInt(Fix(point.Value.Y)) \ _renderer.Scale
      _game.SetMousePosition(gameX, gameY)
    End If
  End Sub

  Private Sub OnPointerMoved(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerMoved
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      Dim gameX = CInt(Fix(point.Value.X)) \ _renderer.Scale
      Dim gameY = CInt(Fix(point.Value.Y)) \ _renderer.Scale
      _game.SetMousePosition(gameX, gameY)
    End If
  End Sub

  Private Sub OnPointerExited(sender As Object, e As PointerEventArgs) Handles PtrRecog.PointerExited
    Dim point = e.GetPosition(Me)
    If point.HasValue Then
      Dim gameX = CInt(Fix(point.Value.X)) \ _renderer.Scale
      Dim gameY = CInt(Fix(point.Value.Y)) \ _renderer.Scale
      _game.SetMousePosition(gameX, gameY)
    End If
  End Sub

  Private Sub OnTimerElapsed(sender As Object, e As EventArgs) Handles Timer.Elapsed
    If Not _isRunning Then Return

    ' Update game state
    If _game.OnUserUpdate(1.0F / FRAME_RATE) Then
      ' Render and update display
      If _game.OnUserRender() Then
        Dim drawTarget As Sprite = _game.GetDrawTarget()
        If drawTarget IsNot Nothing Then
          Dim pixels = drawTarget.GetData()
          SyncLock _lock
            _renderer.UpdatePixels(pixels)
          End SyncLock
        End If
      End If
      ' Trigger repaint on UI thread
      Dispatcher.Dispatch(AddressOf InvalidateSurface)
    Else
      ' Game wants to exit
      Application.Current.Quit()
    End If
  End Sub

  Public Sub Dispose()
    _isRunning = False
    Timer.Stop()
    Timer.Dispose()
    UnregisterPlatformInput()
    _renderer?.Dispose()
  End Sub
End Class
