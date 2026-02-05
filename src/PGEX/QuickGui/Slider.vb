Namespace PGEX.QuickGui
  ''' <summary>
  ''' Creates a Slider Control - a grabbable handle that slides between two locations
  ''' </summary>
  Public Class Slider
    Inherits BaseControl

    Public Sub New(manager As Manager, posMin As Vf2d, posMax As Vf2d, valMin As Single, valMax As Single, value As Single)
      MyBase.New(manager)
      ' Initialization code here
      Me.MinimumPosition = posMin
      Me.MaximumPosition = posMax
      Me.Minimum = valMin
      Me.Maximum = valMax
      Me.Value = value
    End Sub

    ''' <summary>
    ''' Minimum value
    ''' </summary>
    ''' <returns></returns>
    Public Property Minimum As Single = -100.0F
    ''' <summary>
    ''' Maximum value
    ''' </summary>
    ''' <returns></returns>
    Public Property Maximum As Single = 100.0F
    ''' <summary>
    ''' Current value
    ''' </summary>
    ''' <returns></returns>
    Public Property Value As Single = 0.0F

    ''' <summary>
    ''' Location of minimum/start
    ''' </summary>
    ''' <returns></returns>
    Public Property MinimumPosition As Vf2d
    ''' <summary>
    ''' Location of maximum/end
    ''' </summary>
    ''' <returns></returns>
    Public Property MaximumPosition As Vf2d

    Public Overrides Sub Update(pge As PixelGameEngine)

      If m_state = State.Disabled OrElse Not Visible Then
        Return
      End If

      Dim fElapsedTime As Single = pge.GetElapsedTime()

      Dim vMouse = pge.GetMousePos()
      Held = False
      If m_state = State.Click Then
        Dim d = MaximumPosition - MinimumPosition
        Dim u As Single = d.Dot(vMouse - MinimumPosition) / d.Mag2()
        Value = u * (Maximum - Minimum) + Minimum
        Held = True
      Else
        Dim vSliderPos = MinimumPosition + (MaximumPosition - MinimumPosition) * ((Value - Minimum) / (Maximum - Minimum))
        If (vMouse - vSliderPos).Mag2() <= CInt(m_manager.GrabRadius) * CInt(m_manager.GrabRadius) Then
          m_transition += fElapsedTime * m_manager.HoverSpeedOn
          m_state = State.Hover
          If pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed Then
            m_state = State.Click
            Pressed = True
          End If
        Else
          m_state = State.Normal
        End If
      End If

      If pge.GetMouse(PixelGameEngine.Mouse.Left).Released Then
        m_state = State.Normal
        Released = True
      End If

      If m_state = State.Normal Then
        m_transition -= fElapsedTime * m_manager.HoverSpeedOff
        m_state = State.Normal
        Held = False
      End If

      Value = Math.Clamp(Value, Minimum, Maximum)
      m_transition = Math.Clamp(m_transition, 0.0F, 1.0F)

    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)

      If Not Visible Then
        Return
      End If

      pge.DrawLine(MinimumPosition, MaximumPosition, m_manager.BorderColor)
      Dim vSliderPos = MinimumPosition + (MaximumPosition - MinimumPosition) * ((Value - Minimum) / (Maximum - Minimum))

      Select Case m_state
        Case State.Disabled
          pge.FillCircle(vSliderPos, CInt(m_manager.GrabRadius), m_manager.DisableColor)
        Case State.Normal, State.Hover
          pge.FillCircle(vSliderPos, CInt(m_manager.GrabRadius), Pixel.Lerp(m_manager.NormalColor, m_manager.HoverColor, m_transition))
        Case State.Click
          pge.FillCircle(vSliderPos, CInt(m_manager.GrabRadius), m_manager.ClickColor)
      End Select

      pge.DrawCircle(vSliderPos, CInt(m_manager.GrabRadius), m_manager.BorderColor)

    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)

      Throw New NotImplementedException

      'If Not Visible Then
      '  Return
      'End If

      'pge.DrawLineDecal(MinimumPosition, MaximumPosition, m_manager.BorderColor)
      'Dim vSliderPos = MinimumPosition + (MaximumPosition - MinimumPosition) * ((Value - Minimum) / (Maximum - Minimum))

      'Select Case m_state
      '  Case State.Disabled
      '    pge.FillRectDecal(vSliderPos - New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius), New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius) * 2.0F, m_manager.DisableColor)
      '  Case State.Normal, State.Hover
      '    pge.FillRectDecal(vSliderPos - New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius), New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius) * 2.0F, Pixel.Lerp(m_manager.NormalColor, m_manager.HoverColor, m_transition))
      '  Case State.Click
      '    pge.FillRectDecal(vSliderPos - New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius), New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius) * 2.0F, m_manager.ClickColor)
      'End Select

      'pge.SetDecalMode(DecalMode.WIREFRAME)
      'pge.FillRectDecal(vSliderPos - New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius), New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius) * 2.0F, m_manager.BorderColor)
      'pge.SetDecalMode(DecalMode.NORMAL)

    End Sub

  End Class
End Namespace