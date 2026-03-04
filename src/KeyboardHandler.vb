Imports Microsoft.Maui.Controls
Imports Microsoft.Maui.ApplicationModel

' Platform-specific imports
#If WINDOWS10_0_19041_0_OR_GREATER Then
' Platform-specific imports for Windows
Imports System.Windows.Input
#End If

''' <summary>
''' Cross-platform keyboard handler for MAUI applications
''' </summary>
Public Class KeyboardHandler
  Implements IDisposable

  Private ReadOnly _game As PixelGameEngine
  Private _isDisposed As Boolean = False

  Public Sub New(game As PixelGameEngine)
    _game = game
  End Sub

  ''' <summary>
  ''' Register keyboard event handlers for the specified page
  ''' </summary>
  Public Sub RegisterKeyboardEvents(page As Page)
    If page Is Nothing Then Return

    ' Register for keyboard events
    AddHandler page.Focused, AddressOf OnPageFocused
    AddHandler page.Unfocused, AddressOf OnPageUnfocused

    ' Platform-specific registration
#If WINDOWS10_0_19041_0_OR_GREATER Then
    RegisterWindowsKeyboardEvents(page)
#End If
  End Sub

  ''' <summary>
  ''' Unregister keyboard event handlers
  ''' </summary>
  Public Sub UnregisterKeyboardEvents(page As Page)
    If page Is Nothing Then Return

    ' Unregister keyboard events
    RemoveHandler page.Focused, AddressOf OnPageFocused
    RemoveHandler page.Unfocused, AddressOf OnPageUnfocused

    ' Platform-specific unregistration
#If WINDOWS10_0_19041_0_OR_GREATER Then
    UnregisterWindowsKeyboardEvents()
#End If
  End Sub

  Private Sub OnPageFocused(sender As Object, e As FocusEventArgs)
    System.Diagnostics.Debug.WriteLine("Page focused - keyboard input enabled")
  End Sub

  Private Sub OnPageUnfocused(sender As Object, e As FocusEventArgs)
    System.Diagnostics.Debug.WriteLine("Page unfocused - keyboard input disabled")
  End Sub

#Region "Platform-specific implementation"
#If WINDOWS10_0_19041_0_OR_GREATER Then
  Private _windowsKeyListener As WindowsKeyListener
  
  Private Sub RegisterWindowsKeyboardEvents(page As Page)
    Try
      _windowsKeyListener = New WindowsKeyListener(_game)
      _windowsKeyListener.Register(page)
    Catch ex As Exception
      System.Diagnostics.Debug.WriteLine($"Failed to register Windows keyboard events: {ex.Message}")
    End Try
  End Sub
  
  Private Sub UnregisterWindowsKeyboardEvents()
    Try
      If _windowsKeyListener IsNot Nothing Then
        _windowsKeyListener.Unregister()
        _windowsKeyListener = Nothing
      End If
    Catch ex As Exception
      System.Diagnostics.Debug.WriteLine($"Failed to unregister Windows keyboard events: {ex.Message}")
    End Try
  End Sub
#End If
#End Region

  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not _isDisposed Then
      If disposing Then
        ' Dispose managed resources
        UnregisterKeyboardEvents(Nothing)
      End If

      ' Free unmanaged resources
      _isDisposed = True
    End If
  End Sub
End Class

#Region "Platform-specific keyboard listeners"
#If WINDOWS10_0_19041_0_OR_GREATER Then
''' <summary>
''' Windows-specific keyboard listener using WinUI3
''' </summary>
Public Class WindowsKeyListener
  Private ReadOnly _game As PixelGameEngine
  Private _parentPage As Page
  Private _keyboardTimer As Timers.Timer
  
  Public Sub New(game As PixelGameEngine)
    _game = game
  End Sub
  
  Public Sub Register(page As Page)
    _parentPage = page
    
    ' For MAUI on Windows, we'll use a timer to poll keyboard state
    ' This is not ideal but will work for basic input
    _keyboardTimer = New Timers.Timer(16) ' ~60 FPS polling
    AddHandler _keyboardTimer.Elapsed, AddressOf PollKeyboardState
    _keyboardTimer.Start()
  End Sub
  
  Public Sub Unregister()
    ' Clean up keyboard polling
    If _keyboardTimer IsNot Nothing Then
      _keyboardTimer.Stop()
      _keyboardTimer.Dispose()
      _keyboardTimer = Nothing
    End If
    _parentPage = Nothing
  End Sub
  
  Private Sub PollKeyboardState(sender As Object, e As EventArgs)
    ' ' Check for common keys
    ' For key = Key.A To Key.Z
    '   If Keyboard.IsKeyDown(key) Then
    '     _game?.SetKeyStateFromKey(key.ToString(), True)
    '   Else
    '     _game?.SetKeyStateFromKey(key.ToString(), False)
    '   End If
    ' Next
    
    ' ' Check for spacebar
    ' If Keyboard.IsKeyDown(Key.Space) Then
    '   _game?.SetKeyStateFromKey("SPACE", True)
    ' Else
    '   _game?.SetKeyStateFromKey("SPACE", False)
    ' End If
    
    ' ' Check for escape
    ' If Keyboard.IsKeyDown(Key.Escape) Then
    '   _game?.SetKeyStateFromKey("ESCAPE", True)
    ' Else
    '   _game?.SetKeyStateFromKey("ESCAPE", False)
    ' End If
    
    ' ' Check for enter
    ' If Keyboard.IsKeyDown(Key.Enter) Then
    '   _game?.SetKeyStateFromKey("ENTER", True)
    ' Else
    '   _game?.SetKeyStateFromKey("ENTER", False)
    ' End If
    
    ' ' Check for shift
    ' If Keyboard.IsKeyDown(Key.LeftShift) OrElse
    '    Keyboard.IsKeyDown(Key.RightShift) Then
    '   _game?.SetKeyStateFromKey("SHIFT", True)
    ' Else
    '   _game?.SetKeyStateFromKey("SHIFT", False)
    ' End If
    
    ' ' Check for control
    ' If Keyboard.IsKeyDown(Key.LeftCtrl) OrElse
    '    Keyboard.IsKeyDown(Key.RightCtrl) Then
    '   _game?.SetKeyStateFromKey("CTRL", True)
    ' Else
    '   _game?.SetKeyStateFromKey("CTRL", False)
    ' End If
    
    ' ' Check for arrow keys
    ' If Keyboard.IsKeyDown(Key.Up) Then
    '   _game?.SetKeyStateFromKey("UP", True)
    ' Else
    '   _game?.SetKeyStateFromKey("UP", False)
    ' End If
    
    ' If Keyboard.IsKeyDown(Key.Down) Then
    '   _game?.SetKeyStateFromKey("DOWN", True)
    ' Else
    '   _game?.SetKeyStateFromKey("DOWN", False)
    ' End If
    
    ' If Keyboard.IsKeyDown(Key.Left) Then
    '   _game?.SetKeyStateFromKey("LEFT", True)
    ' Else
    '   _game?.SetKeyStateFromKey("LEFT", False)
    ' End If
    
    ' If Keyboard.IsKeyDown(Key.Right) Then
    '   _game?.SetKeyStateFromKey("RIGHT", True)
    ' Else
    '   _game?.SetKeyStateFromKey("RIGHT", False)
    ' End If
    
    ' ' Check for function keys
    ' For i As Integer = 1 To 12
    '   Dim fKey = Key.F1 + i - 1
    '   If Keyboard.IsKeyDown(fKey) Then
    '     _game?.SetKeyStateFromKey($"F{i}", True)
    '   Else
    '     _game?.SetKeyStateFromKey($"F{i}", False)
    '   End If
    ' Next
    
    ' ' Check for number keys
    ' For i As Integer = 0 To 9
    '   Dim numKey = Key.D0 + i
    '   If Keyboard.IsKeyDown(numKey) Then
    '     _game?.SetKeyStateFromKey(i.ToString(), True)
    '   Else
    '     _game?.SetKeyStateFromKey(i.ToString(), False)
    '   End If
    ' Next
  End Sub
End Class
#End If
#End Region