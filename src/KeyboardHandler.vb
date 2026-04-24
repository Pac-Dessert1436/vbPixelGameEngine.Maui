Imports Microsoft.Maui.Controls

''' <summary>
''' Cross-platform keyboard handler for MAUI applications
''' </summary>
Public Class KeyboardHandler
  Implements IDisposable

  Private ReadOnly _game As PixelGameEngine
  Private _isDisposed As Boolean = False
  Private _currentPage As Page

  Public Sub New(game As PixelGameEngine)
    _game = game
  End Sub

  ''' <summary>
  ''' Register keyboard event handlers for the specified page
  ''' </summary>
  Public Sub RegisterKeyboardEvents(page As Page)
    If page Is Nothing Then Exit Sub

    _currentPage = page
    AddHandler page.Focused, AddressOf OnPageFocused
    AddHandler page.Unfocused, AddressOf OnPageUnfocused

    RegisterMauiKeyboardEvents(page)
  End Sub

  ''' <summary>
  ''' Unregister keyboard event handlers
  ''' </summary>
  Public Sub UnregisterKeyboardEvents(page As Page)
    If page Is Nothing Then Exit Sub

    RemoveHandler page.Focused, AddressOf OnPageFocused
    RemoveHandler page.Unfocused, AddressOf OnPageUnfocused

    UnregisterMauiKeyboardEvents(page)
  End Sub

  Private Sub OnPageFocused(sender As Object, e As FocusEventArgs)
    Debug.WriteLine("Page focused - keyboard input enabled")
  End Sub

  Private Sub OnPageUnfocused(sender As Object, e As FocusEventArgs)
    Debug.WriteLine("Page unfocused - keyboard input disabled")
  End Sub

#Region "MAUI Keyboard Events"
  Private Sub RegisterMauiKeyboardEvents(page As Page)
    Try
      ' Register keyboard press event for the page
      AddHandler page.Keyboard, AddressOf OnKeyboardKeyPressed
    Catch ex As Exception
      Debug.WriteLine($"Failed to register MAUI keyboard events: {ex.Message}")
    End Try
  End Sub

  Private Sub UnregisterMauiKeyboardEvents(page As Page)
    Try
      If page IsNot Nothing Then RemoveHandler page.Keyboard, AddressOf OnKeyboardKeyPressed
    Catch ex As Exception
      Debug.WriteLine($"Failed to unregister MAUI keyboard events: {ex.Message}")
    End Try
  End Sub

  Private Sub OnKeyboardKeyPressed(sender As Object, e As KeyboardEventArgs)
    If _game Is Nothing Then Exit Sub
    Try
      Dim keyName = GetKeyNameFromKey(e.Key)
      If Not String.IsNullOrEmpty(keyName) Then
        _game.SetKeyStateFromKey(keyName, True)
      End If
    Catch ex As Exception
      Debug.WriteLine($"Error processing keyboard input: {ex.Message}")
    End Try
  End Sub

  Private Shared Function GetKeyNameFromKey(key As KeyboardKey) As String
    Select Case key
      Case KeyboardKey.A : Return "A"
      Case KeyboardKey.B : Return "B"
      Case KeyboardKey.C : Return "C"
      Case KeyboardKey.D : Return "D"
      Case KeyboardKey.E : Return "E"
      Case KeyboardKey.F : Return "F"
      Case KeyboardKey.G : Return "G"
      Case KeyboardKey.H : Return "H"
      Case KeyboardKey.I : Return "I"
      Case KeyboardKey.J : Return "J"
      Case KeyboardKey.K : Return "K"
      Case KeyboardKey.L : Return "L"
      Case KeyboardKey.M : Return "M"
      Case KeyboardKey.N : Return "N"
      Case KeyboardKey.O : Return "O"
      Case KeyboardKey.P : Return "P"
      Case KeyboardKey.Q : Return "Q"
      Case KeyboardKey.R : Return "R"
      Case KeyboardKey.S : Return "S"
      Case KeyboardKey.T : Return "T"
      Case KeyboardKey.U : Return "U"
      Case KeyboardKey.V : Return "V"
      Case KeyboardKey.W : Return "W"
      Case KeyboardKey.X : Return "X"
      Case KeyboardKey.Y : Return "Y"
      Case KeyboardKey.Z : Return "Z"
      Case KeyboardKey.D0 : Return "0"
      Case KeyboardKey.D1 : Return "1"
      Case KeyboardKey.D2 : Return "2"
      Case KeyboardKey.D3 : Return "3"
      Case KeyboardKey.D4 : Return "4"
      Case KeyboardKey.D5 : Return "5"
      Case KeyboardKey.D6 : Return "6"
      Case KeyboardKey.D7 : Return "7"
      Case KeyboardKey.D8 : Return "8"
      Case KeyboardKey.D9 : Return "9"
      Case KeyboardKey.F1 : Return "F1"
      Case KeyboardKey.F2 : Return "F2"
      Case KeyboardKey.F3 : Return "F3"
      Case KeyboardKey.F4 : Return "F4"
      Case KeyboardKey.F5 : Return "F5"
      Case KeyboardKey.F6 : Return "F6"
      Case KeyboardKey.F7 : Return "F7"
      Case KeyboardKey.F8 : Return "F8"
      Case KeyboardKey.F9 : Return "F9"
      Case KeyboardKey.F10 : Return "F10"
      Case KeyboardKey.F11 : Return "F11"
      Case KeyboardKey.F12 : Return "F12"
      Case KeyboardKey.Up : Return "UP"
      Case KeyboardKey.Down : Return "DOWN"
      Case KeyboardKey.Left : Return "LEFT"
      Case KeyboardKey.Right : Return "RIGHT"
      Case KeyboardKey.Space : Return "SPACE"
      Case KeyboardKey.Tab : Return "TAB"
      Case KeyboardKey.Enter : Return "ENTER"
      Case KeyboardKey.Escape : Return "ESCAPE"
      Case KeyboardKey.Shift : Return "SHIFT"
      Case KeyboardKey.CtrlLeft, KeyboardKey.CtrlRight : Return "CTRL"
      Case KeyboardKey.AltLeft, KeyboardKey.AltRight : Return "ALT"
      Case KeyboardKey.Backspace : Return "BACK"
      Case KeyboardKey.Delete : Return "DEL"
      Case KeyboardKey.Home : Return "HOME"
      Case KeyboardKey.[End] : Return "END"
      Case KeyboardKey.PageUp : Return "PGUP"
      Case KeyboardKey.PageDown : Return "PGDN"
      Case KeyboardKey.CapsLock : Return "CAPSLOCK"
      Case KeyboardKey.Insert : Return "INS"
      Case KeyboardKey.Pause : Return "PAUSE"
      Case KeyboardKey.ScrollLock : Return "SCROLL"
      Case KeyboardKey.Numpad0 : Return "0"
      Case KeyboardKey.Numpad1 : Return "1"
      Case KeyboardKey.Numpad2 : Return "2"
      Case KeyboardKey.Numpad3 : Return "3"
      Case KeyboardKey.Numpad4 : Return "4"
      Case KeyboardKey.Numpad5 : Return "5"
      Case KeyboardKey.Numpad6 : Return "6"
      Case KeyboardKey.Numpad7 : Return "7"
      Case KeyboardKey.Numpad8 : Return "8"
      Case KeyboardKey.Numpad9 : Return "9"
      Case KeyboardKey.NumpadMultiply : Return "NP_MUL"
      Case KeyboardKey.NumpadDivide : Return "NP_DIV"
      Case KeyboardKey.NumpadAdd : Return "NP_ADD"
      Case KeyboardKey.NumpadSubtract : Return "NP_SUB"
      Case KeyboardKey.NumpadDecimal : Return "NP_DECIMAL"
      Case KeyboardKey.Period : Return "."
      Case KeyboardKey.Equals : Return "="
      Case KeyboardKey.Comma : Return ","
      Case KeyboardKey.Minus : Return "-"
      Case KeyboardKey.Semicolon : Return ";"
      Case KeyboardKey.Slash : Return "/"
      Case KeyboardKey.Quote : Return "'"
      Case KeyboardKey.BracketLeft : Return "["
      Case KeyboardKey.Backslash : Return "\"
      Case KeyboardKey.BracketRight : Return "]"
      Case Else : Return Nothing
    End Select
  End Function
#End Region

  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not _isDisposed Then
      If disposing Then
        UnregisterKeyboardEvents(_currentPage)
      End If
      _isDisposed = True
    End If
  End Sub
End Class