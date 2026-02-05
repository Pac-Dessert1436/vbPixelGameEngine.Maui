Namespace PGEX.QuickGui
  Public Enum State
    Disabled
    Normal
    Hover
    Click
  End Enum

  Public Enum Alignment
    Left
    Center
    Right
  End Enum

  ''' <summary>
  ''' Virtual base class for all controls
  ''' </summary>
  Public MustInherit Class BaseControl

    ''' <summary>
    ''' Controls are related to a manager, where the theme resides and control groups can be implemented
    ''' </summary>
    Protected m_manager As Manager ' Switches
    ''' <summary>
    ''' All controls exist in one of four states
    ''' </summary>
    Protected m_state As State = State.Normal

    ''' <summary>
    ''' To add a "swish" to things, controls can fade between states
    ''' </summary>
    Protected m_transition As Single = 0.0!

    Public Sub New(manager As Manager)
      m_manager = manager
      m_manager.AddControl(Me)
    End Sub

    ''' <summary>
    ''' Switches the control on/off
    ''' </summary>
    ''' <param name="enabled"></param>
    Public Overridable Sub Enable(enabled As Boolean)
      m_state = If(enabled, State.Normal, State.Disabled)
    End Sub

    ''' <summary>
    ''' Sets whether or not the control is interactive/displayed
    ''' </summary>
    ''' <returns></returns>
    Public Property Visible As Boolean = True

    ''' <summary>
    ''' True on single frame control begins being manipulated
    ''' </summary>
    ''' <returns></returns>
    Public Property Pressed As Boolean

    ''' <summary>
    ''' True on all frames control is under user manipulation
    ''' </summary>
    ''' <returns></returns>
    Public Property Held As Boolean

    ''' <summary>
    ''' True on single frame control ceases being manipulated
    ''' </summary>
    ''' <returns></returns>
    Public Property Released As Boolean

    ''' <summary>
    ''' Updates the controls behavior
    ''' </summary>
    ''' <param name="pge"></param>
    Public Overridable Sub Update(pge As PixelGameEngine)
    End Sub
    ''' <summary>
    ''' Draws the control using "sprite" based CPU operations
    ''' </summary>
    ''' <param name="pge"></param>
    Public MustOverride Sub Draw(pge As PixelGameEngine)
    ''' <summary>
    ''' Draws the control using "decal" based GPU operations
    ''' </summary>
    ''' <param name="pge"></param>
    Public MustOverride Sub DrawDecal(pge As PixelGameEngine)

  End Class
End Namespace