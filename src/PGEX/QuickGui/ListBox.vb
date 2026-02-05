Namespace PGEX.QuickGui
  Public Class ListBox
    Inherits BaseControl

    Public Sub New(manager As Manager, list As List(Of String), pos As Vf2d, size As Vf2d)
      MyBase.New(manager)
      Me.List = list
      Group.CopyThemeFrom(m_manager)
      Position = pos
      size = size
      Slider = New Slider(Group,
                          New Vf2d(pos.x + size.x - m_manager.GrabRadius - 1, pos.y + m_manager.GrabRadius + 1),
                          New Vf2d(pos.x + size.x - m_manager.GrabRadius - 1, pos.y + size.y - m_manager.GrabRadius - 1),
                          0, list.Count, 0)
    End Sub

    ''' <summary>
    ''' Position of list
    ''' </summary>
    ''' <returns></returns>
    Public Property Position As Vf2d
    ''' <summary>
    ''' Size of list
    ''' </summary>
    ''' <returns></returns>
    Public Property Size As Vf2d
    ''' <summary>
    ''' Show a border?
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBorder As Boolean = True
    ''' <summary>
    ''' Show a background?
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBackground As Boolean = True

    Public Property Slider As Slider = Nothing
    Public Property Group As Manager
    Public Property VisibleItems As Integer = 0
    Public Property List As List(Of String)

    ''' <summary>
    ''' Item currently selected
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectedItem As Integer = 0
    Public Property PreviouslySelectedItem As Integer = 0
    ''' <summary>
    ''' Has selection changed?
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectionChanged As Boolean = False

    Public Overrides Sub Update(pge As PixelGameEngine)

      If m_state = State.Disabled OrElse Not Visible Then Return

      PreviouslySelectedItem = SelectedItem
      Dim vMouse = pge.GetMousePos() - Position + New Vi2d(2, 0)
      If pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed Then
        If vMouse.x >= 0 AndAlso vMouse.x < Size.x - (Group.GrabRadius * 2) AndAlso vMouse.y >= 0 AndAlso vMouse.y < Size.y Then
          SelectedItem = CInt(Slider.Value + vMouse.y / 10)
        End If
      End If

      SelectedItem = Math.Clamp(SelectedItem, 0, List.Count - 1)

      SelectionChanged = SelectedItem <> PreviouslySelectedItem

      Slider.Maximum = List.Count
      Group.Update(pge)

    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)

      If Not Visible Then Return

      If HasBackground Then
        pge.FillRect(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
      End If

      If HasBorder Then
        pge.DrawRect(Position, Size - New Vf2d(1, 1), m_manager.BorderColor)
      End If

      Dim idx0 As UInteger = CUInt(Slider.Value)
      Dim idx1 As UInteger = Math.Min(idx0 + CUInt((Size.y - 4) / 10), CUInt(List.Count))

      Dim vTextPos = Position + New Vf2d(2, 2)
      For idx As UInteger = idx0 To idx1 - 1UI
        If idx = SelectedItem Then
          pge.FillRect(vTextPos - New Vi2d(1, 1), New Vi2d(CInt(Size.x - Group.GrabRadius * 2), 10), Group.HoverColor)
        End If
        pge.DrawStringProp(vTextPos, List(CInt(idx)))
        vTextPos.y += 10
      Next

      Group.Draw(pge)

    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)

      Throw New NotImplementedException

      'If Not Visible Then Return

      'If HasBackground Then
      '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
      'End If

      'Dim idx0 As UInteger = CUInt(Slider.Value)
      'Dim idx1 As UInteger = Math.Min(idx0 + CUInt((Size.y - 4) / 10), CUInt(List.Count))

      'Dim vTextPos = Position + New Vf2d(2, 2)
      'For idx As UInteger = idx0 To idx1 - 1UL
      '  If idx = SelectedItem Then
      '    pge.FillRectDecal(vTextPos - New Vi2d(1, 1), New Vf2d(Size.x - Group.GrabRadius * 2.0F, 10.0F), Group.HoverColor)
      '  End If
      '  pge.DrawStringPropDecal(vTextPos, List(CInt(idx)))
      '  vTextPos.y += 10
      'Next

      'If HasBorder Then
      '  pge.SetDecalMode(DecalMode.WIREFRAME)
      '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.BorderColor)
      '  pge.SetDecalMode(DecalMode.NORMAL)
      'End If

      'Group.DrawDecal(pge)

    End Sub
  End Class
End Namespace