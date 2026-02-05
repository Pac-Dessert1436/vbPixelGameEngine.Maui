Imports VbPixelGameEngine.Maui.PGEX.QuickGui

Public Class TextBox
  Inherits Label

  Private m_textEdit As Boolean

  Public Sub New(manager As Manager, text As String, pos As Vf2d, size As Vf2d)
    MyBase.New(manager, text, pos, size)
    Alignment = Alignment.Left
    HasBorder = True
    HasBackground = False
  End Sub

  Public Overrides Sub Update(pge As PixelGameEngine)

    If m_state = State.Disabled OrElse Not Visible Then Return

    Pressed = False
    Released = False

    Dim vMouse = pge.GetMousePos()

    If vMouse.x >= Position.x AndAlso vMouse.x < Position.x + Size.x AndAlso
        vMouse.y >= Position.y AndAlso vMouse.y < Position.y + Size.y Then

      Pressed = pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed
      Released = pge.GetMouse(PixelGameEngine.Mouse.Left).Released

      If Pressed AndAlso pge.IsTextEntryEnabled() AndAlso Not m_textEdit Then
        pge.TextEntryEnable(False)
      End If

      If Pressed AndAlso Not pge.IsTextEntryEnabled() AndAlso Not m_textEdit Then
        pge.TextEntryEnable(True, Text)
        m_textEdit = True
      End If

      Held = pge.GetMouse(PixelGameEngine.Mouse.Left).Held

    Else
      Pressed = pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed
      Released = pge.GetMouse(PixelGameEngine.Mouse.Left).Released
      Held = pge.GetMouse(PixelGameEngine.Mouse.Left).Held

      If Pressed AndAlso m_textEdit Then
        Text = pge.TextEntryGetString()
        pge.TextEntryEnable(False)
        m_textEdit = False
      End If
    End If

    If m_textEdit AndAlso pge.IsTextEntryEnabled() Then
      Text = pge.TextEntryGetString()
    End If

  End Sub

  Public Overrides Sub Draw(pge As PixelGameEngine)

    If Not Visible Then Return

    If HasBackground Then
      pge.FillRect(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
    End If

    If HasBorder Then
      pge.DrawRect(Position, Size - New Vf2d(1, 1), m_manager.BorderColor)
    End If

    Dim vText = pge.GetTextSizeProp(Text)
    Select Case Alignment
      Case Alignment.Left
        pge.DrawStringProp(New Vf2d(Position.x + 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
      Case Alignment.Center
        pge.DrawStringProp(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)
      Case Alignment.Right
        pge.DrawStringProp(New Vf2d(Position.x + Size.x - vText.x - 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
    End Select
  End Sub

  Public Overrides Sub DrawDecal(pge As PixelGameEngine)

    Throw New NotImplementedException

    'If Not Visible Then Return

    'If HasBackground Then
    '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
    'End If

    'If HasBorder Then
    '  pge.SetDecalMode(DecalMode.WIREFRAME)
    '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.BorderColor)
    '  pge.SetDecalMode(DecalMode.NORMAL)
    'End If

    'Dim vText = pge.GetTextSizeProp(Text)
    'Select Case Alignment
    '  Case Alignment.Left
    '    pge.DrawStringPropDecal(New Vf2d(Position.x + 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
    '  Case Alignment.Centre
    '    pge.DrawStringPropDecal(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)
    '  Case Alignment.Right
    '    pge.DrawStringPropDecal(New Vf2d(Position.x + Size.x - vText.x - 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
    'End Select

  End Sub

End Class