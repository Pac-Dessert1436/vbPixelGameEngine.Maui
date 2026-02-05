Namespace PGEX.QuickGui
  ''' <summary>
  ''' Creates a Button Control - a clickable, labeled rectangle
  ''' </summary>
  Public Class CheckBox
    Inherits Button

    Public Property Checked As Boolean = False

    Public Sub New(manager As Manager, text As String, check As Boolean, pos As Vf2d, size As Vf2d)
      MyBase.New(manager, text, pos, size)
      Checked = check
    End Sub

    Public Overrides Sub Update(pge As PixelGameEngine)
      If m_state = State.Disabled OrElse Not Visible Then Return
      MyBase.Update(pge)
      If Pressed Then
        Checked = Not Checked
      End If
    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)
      If Not Visible Then Return
      MyBase.Draw(pge)
      If Checked Then
        pge.DrawRect(Position + New Vf2d(2, 2), Size - New Vi2d(5, 5), m_manager.BorderColor)
      End If
    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)
      Throw New NotImplementedException
    End Sub

  End Class

  Public Class ImageCheckBox
    Inherits ImageButton

    Public Property Checked As Boolean = False

    Public Sub New(manager As Manager, icon As Sprite, check As Boolean, pos As Vf2d, size As Vf2d)
      MyBase.New(manager, icon, pos, size)
      Checked = check
    End Sub

    Public Overrides Sub Update(pge As PixelGameEngine)
      If m_state = State.Disabled OrElse Not Visible Then Return
      MyBase.Update(pge)
      If Pressed Then Checked = Not Checked
    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)
      MyBase.Draw(pge)
      If Checked Then
        pge.DrawRect(Position + New Vf2d(2, 2), Size - New Vi2d(5, 5), m_manager.BorderColor)
      End If
    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)
      Throw New NotImplementedException
    End Sub

  End Class
End Namespace