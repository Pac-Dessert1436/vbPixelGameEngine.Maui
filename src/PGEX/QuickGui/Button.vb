Namespace PGEX.QuickGui
  ''' <summary>
  ''' Creates a Button Control - a clickable, labeled rectangle
  ''' </summary>
  Public Class Button
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

    Public Event Clicked As EventHandler(Of EventArgs)

    Public Overrides Sub Update(pge As PixelGameEngine)

      If m_state = State.Disabled OrElse Not Visible Then
        Return
      End If

      Pressed = False
      Released = False
      Dim elapsedTime = pge.GetElapsedTime()

      Dim vMouse = pge.GetMousePos()
      If m_state <> State.Click Then
        If vMouse.x >= Position.x AndAlso vMouse.x < Position.x + Size.x AndAlso
           vMouse.y >= Position.y AndAlso vMouse.y < Position.y + Size.y Then
          m_transition += elapsedTime * m_manager.HoverSpeedOn
          m_state = State.Hover

          Pressed = pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed
          If Pressed Then
            m_state = State.Click
            RaiseEvent Clicked(Me, EventArgs.Empty)
          End If

          Held = pge.GetMouse(PixelGameEngine.Mouse.Left).Held
        Else
          m_transition -= elapsedTime * m_manager.HoverSpeedOff
          m_state = State.Normal
        End If
      Else
        Held = pge.GetMouse(PixelGameEngine.Mouse.Left).Held
        Released = pge.GetMouse(PixelGameEngine.Mouse.Left).Released
        If Released Then
          m_state = State.Normal
          RaiseEvent Clicked(Me, EventArgs.Empty)
        End If
      End If

      m_transition = Math.Clamp(m_transition, 0.0F, 1.0F)

    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)

      If Not Visible Then
        Return
      End If

      Select Case m_state
        Case State.Disabled
          pge.FillRect(Position, Size, m_manager.DisableColor)
        Case State.Normal, State.Hover
          pge.FillRect(Position, Size, Pixel.Lerp(m_manager.NormalColor, m_manager.HoverColor, m_transition))
        Case State.Click
          pge.FillRect(Position, Size, m_manager.ClickColor)
      End Select

      pge.DrawRect(Position, Size - New Vf2d(1, 1), m_manager.BorderColor)
      Dim vText = pge.GetTextSizeProp(Text)
      pge.DrawStringProp(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)

    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)

      Throw New NotImplementedException

      'If Not Visible Then
      '  Return
      'End If

      'Select Case m_state
      '  Case State.Disabled
      '    pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.DisableColor)
      '  Case State.Normal, State.Hover
      '    pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), Pixel.Lerp(m_manager.NormalColor, m_manager.HoverColor, m_transition))
      '  Case State.Click
      '    pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.ClickColor)
      'End Select

      'pge.SetDecalMode(DecalMode.WIREFRAME)
      'pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.BorderColor)
      'pge.SetDecalMode(DecalMode.NORMAL)

      'Dim vText = pge.GetTextSizeProp(Text)
      'pge.DrawStringPropDecal(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)

    End Sub

  End Class

  Public Class ImageButton
    Inherits Button

    Public Sub New(manager As Manager, icon As Sprite, pos As Vf2d, size As Vf2d)
      MyBase.New(manager, String.Empty, pos, size)
      Me.Icon = icon
    End Sub

    Public Property Icon As Sprite 'Renderable

    Public Overrides Sub Update(pge As PixelGameEngine)
    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)
      MyBase.Draw(pge)
      pge.DrawSprite(Position + New Vi2d(4, 4), Icon) '.Sprite())
    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)
      Throw New NotImplementedException
      'MyBase.DrawDecal(pge)
      'pge.DrawDecal(Position + New Vi2d(4, 4), Icon.Decal())
    End Sub

  End Class

End Namespace