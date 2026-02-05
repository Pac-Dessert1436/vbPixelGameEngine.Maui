Namespace PGEX.QuickGui

  ''' <summary>
  ''' Creates a Label control - it's just text!
  ''' </summary>
  Public Class Label
    Inherits BaseControl

    Public Sub New(manager As Manager, text As String, pos As Vf2d, size As Vf2d)
      MyBase.New(manager)
      Me.Text = text
      Position = pos
      Me.Size = size
    End Sub

    ''' <summary>
    ''' Position of button
    ''' </summary>
    ''' <returns></returns>
    Public Property Position As Vf2d

    ''' <summary>
    ''' Size of button
    ''' </summary>
    ''' <returns></returns>
    Public Property Size As Vf2d

    ''' <summary>
    ''' Text displayed on button
    ''' </summary>
    ''' <returns></returns>
    Public Property Text As String

    ''' <summary>
    ''' Show a border?
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBorder As Boolean = False

    ''' <summary>
    ''' Show a background?
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBackground As Boolean = False

    ''' <summary>
    ''' Where should the text be positioned?
    ''' </summary>
    ''' <returns></returns>
    Public Property Alignment As Alignment = Alignment.Center

    Public Overrides Sub Update(pge As PixelGameEngine)
      ' Implementation here
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
      '  Case Alignment.Center
      '    pge.DrawStringPropDecal(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)
      '  Case Alignment.Right
      '    pge.DrawStringPropDecal(New Vf2d(Position.x + Size.x - vText.x - 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
      'End Select

    End Sub

  End Class
End Namespace