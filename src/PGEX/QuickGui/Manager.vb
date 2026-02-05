Namespace PGEX.QuickGui
  ''' <summary>
  ''' A QuickGui.Manager acts as a convenient grouping of controls
  ''' </summary>
  Public Class Manager

    Private ReadOnly m_eraseControlsOnDestroy As Boolean
    Private ReadOnly m_controls As New List(Of BaseControl)

    ' This managers "Theme" can be set here
    ' Various element colors
    Public Property NormalColor As Pixel = Presets.DarkBlue
    Public Property HoverColor As Pixel = Presets.Blue
    Public Property ClickColor As Pixel = Presets.Cyan
    Public Property DisableColor As Pixel = Presets.DarkGrey
    Public Property BorderColor As Pixel = Presets.White
    Public Property TextColor As Pixel = Presets.White

    ''' <summary>
    ''' Speed to transition from Normal -> Hover
    ''' </summary>
    ''' <returns></returns>
    Public Property HoverSpeedOn As Single = 10.0!
    ''' <summary>
    ''' Speed to transition from Hover -> Normal
    ''' </summary>
    ''' <returns></returns>
    Public Property HoverSpeedOff As Single = 4.0!
    ''' <summary>
    ''' Size of grabe handle
    ''' </summary>
    ''' <returns></returns>
    Public Property GrabRadius As Single = 8.0!

    Public Sub CopyThemeFrom(manager As Manager)
      BorderColor = manager.BorderColor
      ClickColor = manager.ClickColor
      DisableColor = manager.DisableColor
      HoverColor = manager.HoverColor
      NormalColor = manager.NormalColor
      TextColor = manager.TextColor
      GrabRadius = manager.GrabRadius
      HoverSpeedOff = manager.HoverSpeedOff
      HoverSpeedOn = manager.HoverSpeedOn
    End Sub

    Public Sub New()
      m_eraseControlsOnDestroy = True
      m_controls = New List(Of BaseControl)()
    End Sub

    Public Sub New(cleanUpForMe As Boolean)
      m_eraseControlsOnDestroy = cleanUpForMe
      m_controls = New List(Of BaseControl)()
    End Sub

    Protected Overrides Sub Finalize()
      If m_eraseControlsOnDestroy Then
        'For Each p In m_controls
        '  p = Nothing
        'Next
        m_controls.Clear()
      End If
    End Sub

    ''' <summary>
    ''' Add a gui element derived from BaseControl to this manager
    ''' </summary>
    ''' <param name="control"></param>
    Public Sub AddControl(control As BaseControl)
      m_controls.Add(control)
    End Sub

    ''' <summary>
    ''' Updates all controls this manager operates
    ''' </summary>
    ''' <param name="pge"></param>
    Public Sub Update(pge As PixelGameEngine)
      For Each p In m_controls
        p.Update(pge)
      Next
    End Sub

    ''' <summary>
    ''' Draws as "sprite" all controls this manager operates
    ''' </summary>
    ''' <param name="pge"></param>
    Public Sub Draw(pge As PixelGameEngine)
      For Each p In m_controls
        p.Draw(pge)
      Next
    End Sub

    ''' <summary>
    ''' Draws as "decal" all controls this manager operates
    ''' </summary>
    ''' <param name="pge"></param>
    Public Sub DrawDecal(pge As PixelGameEngine)
      For Each p In m_controls
        p.DrawDecal(pge)
      Next
    End Sub

  End Class
End Namespace