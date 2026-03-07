Imports Microsoft.Maui.Controls

#If WINDOWS10_0_19041_0_OR_GREATER Then
Imports System.Runtime.InteropServices
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

    AddHandler page.Focused, AddressOf OnPageFocused
    AddHandler page.Unfocused, AddressOf OnPageUnfocused

#If WINDOWS10_0_19041_0_OR_GREATER Then
    RegisterWindowsKeyboardEvents(page)
#End If
  End Sub

  ''' <summary>
  ''' Unregister keyboard event handlers
  ''' </summary>
  Public Sub UnregisterKeyboardEvents(page As Page)
    If page Is Nothing Then Return

    RemoveHandler page.Focused, AddressOf OnPageFocused
    RemoveHandler page.Unfocused, AddressOf OnPageUnfocused

#If WINDOWS10_0_19041_0_OR_GREATER Then
    UnregisterWindowsKeyboardEvents()
#End If
  End Sub

  Private Sub OnPageFocused(sender As Object, e As FocusEventArgs)
    ' TODO: Continue the game when page is focused
    Debug.WriteLine("Page focused - keyboard input enabled")
  End Sub

  Private Sub OnPageUnfocused(sender As Object, e As FocusEventArgs)
    ' TODO: Suspend the game when page is unfocused
    Debug.WriteLine("Page unfocused - keyboard input disabled")
  End Sub

#Region "Platform-specific implementation"
#If WINDOWS10_0_19041_0_OR_GREATER Then
  Private _windowsKeyListener As WindowsKeyListener

  Private Sub RegisterWindowsKeyboardEvents(page As Page)
    Try
      _windowsKeyListener = New WindowsKeyListener(_game)
      _windowsKeyListener.Register(page)
    Catch ex As Exception
      Debug.WriteLine($"Failed to register Windows keyboard events: {ex.Message}")
    End Try
  End Sub

  Private Sub UnregisterWindowsKeyboardEvents()
    Try
      If _windowsKeyListener IsNot Nothing Then
        _windowsKeyListener.Unregister()
        _windowsKeyListener = Nothing
      End If
    Catch ex As Exception
      Debug.WriteLine($"Failed to unregister Windows keyboard events: {ex.Message}")
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
      If disposing Then UnregisterKeyboardEvents(Nothing)
      _isDisposed = True
    End If
  End Sub
End Class

#Region "Platform-specific keyboard listeners"
#If WINDOWS10_0_19041_0_OR_GREATER Then
''' <summary>
''' Windows-specific keyboard listener using Win32 API
''' </summary>
Public Class WindowsKeyListener
  Private ReadOnly _game As PixelGameEngine
  Private _keyboardTimer As System.Threading.Timer

  Private Const VK_SPACE As Integer = &H20
  Private Const VK_ESCAPE As Integer = &H1B
  Private Const VK_RETURN As Integer = &HD
  Private Const VK_SHIFT As Integer = &H10
  Private Const VK_CONTROL As Integer = &H11
  Private Const VK_MENU As Integer = &H12
  Private Const VK_LEFT As Integer = &H25
  Private Const VK_UP As Integer = &H26
  Private Const VK_RIGHT As Integer = &H27
  Private Const VK_DOWN As Integer = &H28
  Private Const VK_F1 As Integer = &H70

  <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)>
  Private Shared Function GetAsyncKeyState(vKey As Integer) As Short
  End Function

  Public Sub New(game As PixelGameEngine)
    _game = game
  End Sub

  Public Sub Register(page As Page)
    _keyboardTimer = New System.Threading.Timer(
      Sub(state) PollKeyboardState(),
      Nothing,
      TimeSpan.Zero,
      TimeSpan.FromMilliseconds(16))
  End Sub

  Public Sub Unregister()
    If _keyboardTimer IsNot Nothing Then
      _keyboardTimer.Dispose()
      _keyboardTimer = Nothing
    End If
  End Sub

  Private Sub PollKeyboardState()
    If _game Is Nothing Then Return

    Try
      For i As Integer = 0 To 25
        Dim vk As Integer = &H41 + i
        Dim isDown = (GetAsyncKeyState(vk) And &H8000) <> 0
        _game.SetKeyStateFromKey(ChrW(&H41 + i).ToString(), isDown)
      Next

      For i As Integer = 0 To 9
        Dim vk As Integer = &H30 + i
        Dim isDown = (GetAsyncKeyState(vk) And &H8000) <> 0
        _game.SetKeyStateFromKey(i.ToString(), isDown)
      Next

      _game?.SetKeyStateFromKey("SPACE", (GetAsyncKeyState(VK_SPACE) And &H8000) <> 0)
      _game?.SetKeyStateFromKey("ESCAPE", (GetAsyncKeyState(VK_ESCAPE) And &H8000) <> 0)
      _game?.SetKeyStateFromKey("ENTER", (GetAsyncKeyState(VK_RETURN) And &H8000) <> 0)

      Dim isShiftDown = (GetAsyncKeyState(VK_SHIFT) And &H8000) <> 0
      _game?.SetKeyStateFromKey("SHIFT", isShiftDown)

      Dim isCtrlDown = (GetAsyncKeyState(VK_CONTROL) And &H8000) <> 0
      _game?.SetKeyStateFromKey("CTRL", isCtrlDown)

      _game?.SetKeyStateFromKey("UP", (GetAsyncKeyState(VK_UP) And &H8000) <> 0)
      _game?.SetKeyStateFromKey("DOWN", (GetAsyncKeyState(VK_DOWN) And &H8000) <> 0)
      _game?.SetKeyStateFromKey("LEFT", (GetAsyncKeyState(VK_LEFT) And &H8000) <> 0)
      _game?.SetKeyStateFromKey("RIGHT", (GetAsyncKeyState(VK_RIGHT) And &H8000) <> 0)

      For i As Integer = 0 To 11
        Dim vk As Integer = VK_F1 + i
        Dim isDown = (GetAsyncKeyState(vk) And &H8000) <> 0
        _game?.SetKeyStateFromKey($"F{i + 1}", isDown)
      Next
    Catch
    End Try
  End Sub
End Class
#End If
#End Region