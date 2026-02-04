Option Compare Text
Imports System.Threading

Public Module Singleton

  Public Property AtomActive As Boolean
  Public Property MapKeys As New Dictionary(Of Integer, Integer)
  Public Property Pge As PixelGameEngine

End Module

Public MustInherit Class PixelGameEngine
  Implements IDisposable

  Private m_renderer As SkiaSharpRenderer

  Public Sub SetRenderer(renderer As SkiaSharpRenderer)
    m_renderer = renderer
  End Sub

  Public Delegate Function PixelModeDelegate(x As Integer, y As Integer, p1 As Pixel, p2 As Pixel) As Pixel
  Private m_funcPixelMode As PixelModeDelegate

  Protected Property Title As String

  ' Removed for MAUI - SkiaSharp rendering used instead

  Private m_pixelMode As Pixel.Mode
  Private m_blendFactor As Single = 1.0F

  Private ReadOnly m_lastElapsed As Single
  Private m_screenWidth As Integer
  Private m_screenHeight As Integer
  Private m_pixelWidth As Integer
  Private m_pixelHeight As Integer
  Private m_fullScreen As Boolean
  Private m_enableVSYNC As Boolean
  Private m_pixelX As Single
  Private m_pixelY As Single
  Private m_defaultDrawTarget As Sprite
  Private m_drawTarget As Sprite

  'Private Shared m_shutdown As Boolean
  Private m_windowWidth As Integer
  Private m_windowHeight As Integer
  Private ReadOnly m_hasInputFocus As Boolean
  Private ReadOnly m_hasMouseFocus As Boolean
  Private ReadOnly m_frameTimer As Single = 1.0F
  Private ReadOnly m_frameCount As Integer
  Private ReadOnly m_totalFrameCount As Integer
  'Private m_totalFrames As Integer

  Private ReadOnly m_keyNewState(255) As Boolean
  Private ReadOnly m_keyOldState(255) As Boolean
  Private ReadOnly m_keyboardState(255) As HwButton

  Private ReadOnly m_mouseNewState(4) As Boolean
  Private ReadOnly m_mouseOldState(4) As Boolean
  Private ReadOnly m_mouseState(4) As HwButton
  Private m_mousePosXcache As Integer
  Private m_mousePosYcache As Integer
  'Private m_mousePosX As Integer
  'Private m_mousePosY As Integer
  Private m_mousePos As Vi2d

  ' MAUI-specific mouse control methods
  Public Sub SetMousePosition(x As Integer, y As Integer)
    m_mousePos.x = x
    m_mousePos.y = y
    m_mousePosXcache = x
    m_mousePosYcache = y
  End Sub

  Public Sub SetMouseButtonState(button As Integer, pressed As Boolean)
    If button >= 0 AndAlso button < m_mouseNewState.Length Then
      m_mouseNewState(button) = pressed
    End If
  End Sub

  ' MAUI-specific keyboard mapping
  Public Sub SetKeyStateFromKey(keyStr As String, pressed As Boolean)
    Dim keyIdx As Integer = -1
    Dim upperKey = keyStr.ToUpper()

    ' Map MAUI key strings to our internal Key enum
    Select Case upperKey
      Case "A" : keyIdx = Key.A
      Case "B" : keyIdx = Key.B
      Case "C" : keyIdx = Key.C
      Case "D" : keyIdx = Key.D
      Case "E" : keyIdx = Key.E
      Case "F" : keyIdx = Key.F
      Case "G" : keyIdx = Key.G
      Case "H" : keyIdx = Key.H
      Case "I" : keyIdx = Key.I
      Case "J" : keyIdx = Key.J
      Case "K" : keyIdx = Key.K
      Case "L" : keyIdx = Key.L
      Case "M" : keyIdx = Key.M
      Case "N" : keyIdx = Key.N
      Case "O" : keyIdx = Key.O
      Case "P" : keyIdx = Key.P
      Case "Q" : keyIdx = Key.Q
      Case "R" : keyIdx = Key.R
      Case "S" : keyIdx = Key.S
      Case "T" : keyIdx = Key.T
      Case "U" : keyIdx = Key.U
      Case "V" : keyIdx = Key.V
      Case "W" : keyIdx = Key.W
      Case "X" : keyIdx = Key.X
      Case "Y" : keyIdx = Key.Y
      Case "Z" : keyIdx = Key.Z
      Case "0" : keyIdx = Key.K0
      Case "1" : keyIdx = Key.K1
      Case "2" : keyIdx = Key.K2
      Case "3" : keyIdx = Key.K3
      Case "4" : keyIdx = Key.K4
      Case "5" : keyIdx = Key.K5
      Case "6" : keyIdx = Key.K6
      Case "7" : keyIdx = Key.K7
      Case "8" : keyIdx = Key.K8
      Case "9" : keyIdx = Key.K9
      Case "F1" : keyIdx = Key.F1
      Case "F2" : keyIdx = Key.F2
      Case "F3" : keyIdx = Key.F3
      Case "F4" : keyIdx = Key.F4
      Case "F5" : keyIdx = Key.F5
      Case "F6" : keyIdx = Key.F6
      Case "F7" : keyIdx = Key.F7
      Case "F8" : keyIdx = Key.F8
      Case "F9" : keyIdx = Key.F9
      Case "F10" : keyIdx = Key.F10
      Case "F11" : keyIdx = Key.F11
      Case "F12" : keyIdx = Key.F12
      Case "UP" : keyIdx = Key.UP
      Case "DOWN" : keyIdx = Key.DOWN
      Case "LEFT" : keyIdx = Key.LEFT
      Case "RIGHT" : keyIdx = Key.RIGHT
      Case "SPACE" : keyIdx = Key.SPACE
      Case "TAB" : keyIdx = Key.TAB
      Case "SHIFT", "LEFTSHIFT", "RIGHTSHIFT" : keyIdx = Key.SHIFT
      Case "CTRL", "LEFTCTRL", "RIGHTCTRL" : keyIdx = Key.CTRL
      Case "ALT", "LEFTALT", "RIGHTALT" : keyIdx = Key.ALT
      Case "INSERT", "INS" : keyIdx = Key.INS
      Case "DELETE", "DEL" : keyIdx = Key.DEL
      Case "HOME" : keyIdx = Key.HOME
      Case "END" : keyIdx = Key.[END]
      Case "PAGEUP", "PGUP" : keyIdx = Key.PGUP
      Case "PAGEDOWN", "PGDN" : keyIdx = Key.PGDN
      Case "BACK", "BACKSPACE" : keyIdx = Key.BACK
      Case "ESC", "ESCAPE" : keyIdx = Key.ESCAPE
      Case "RETURN", "ENTER" : keyIdx = Key.ENTER
      Case "PAUSE", "BREAK" : keyIdx = Key.PAUSE
      Case "SCROLL", "SCROLLLOCK" : keyIdx = Key.SCROLL
      Case "NUMPAD0" : keyIdx = Key.NP0
      Case "NUMPAD1" : keyIdx = Key.NP1
      Case "NUMPAD2" : keyIdx = Key.NP2
      Case "NUMPAD3" : keyIdx = Key.NP3
      Case "NUMPAD4" : keyIdx = Key.NP4
      Case "NUMPAD5" : keyIdx = Key.NP5
      Case "NUMPAD6" : keyIdx = Key.NP6
      Case "NUMPAD7" : keyIdx = Key.NP7
      Case "NUMPAD8" : keyIdx = Key.NP8
      Case "NUMPAD9" : keyIdx = Key.NP9
      Case "MULTIPLY", "NUMPADMULTIPLY" : keyIdx = Key.NP_MUL
      Case "DIVIDE", "NUMPADDIVIDE" : keyIdx = Key.NP_DIV
      Case "ADD", "NUMPADADD" : keyIdx = Key.NP_ADD
      Case "SUBTRACT", "NUMPADSUBTRACT" : keyIdx = Key.NP_SUB
      Case "DECIMAL", "NUMPADDECIMAL", "NUMPADPERIOD" : keyIdx = Key.NP_DECIMAL
      Case "." : keyIdx = Key.PERIOD
      Case "=" : keyIdx = Key.EQUALS
      Case "," : keyIdx = Key.COMMA
      Case "-" : keyIdx = Key.MINUS
      Case ";" : keyIdx = Key.OEM_1
      Case "/" : keyIdx = Key.OEM_2
      Case "`" : keyIdx = Key.OEM_3
      Case "[" : keyIdx = Key.OEM_4
      Case "\" : keyIdx = Key.OEM_5
      Case "]" : keyIdx = Key.OEM_6
      Case "'" : keyIdx = Key.OEM_7
      Case "\" : keyIdx = Key.OEM_8
      Case "CAPSLOCK" : keyIdx = Key.CAPS_LOCK
    End Select

    If keyIdx >= 0 AndAlso keyIdx < m_keyNewState.Length Then
      m_keyNewState(keyIdx) = pressed
    End If
  End Sub

  Private ReadOnly m_mouseWheelDelta As Integer
  Private m_mouseWheelDeltaCache As Integer
  Private m_viewX As Integer
  Private m_viewY As Integer
  Private m_viewW As Integer
  Private m_viewH As Integer

  ' Removed for MAUI - SkiaSharp rendering used instead

  Private m_subPixelOffsetX As Single
  Private m_subPixelOffsetY As Single

  Public Enum Mouse
    Left
    Right
    Middle
  End Enum

  Public Structure HwButton
    Public Pressed As Boolean ' Set once during the frame the event occurs
    Public Released As Boolean ' Set once during the frame the event occurs
    Public Held As Boolean ' Set true for all frames between pressed and released events
    Public ElapsedTime As Single
  End Structure

  Public Enum RCode
    Ok
    Fail
    NoFile
  End Enum

  Public Enum Key
    NONE
    A
    B
    C
    D
    E
    F
    G
    H
    I
    J
    K
    L
    M
    N
    O
    P
    Q
    R
    S
    T
    U
    V
    W
    X
    Y
    Z
    K0
    K1
    K2
    K3
    K4
    K5
    K6
    K7
    K8
    K9
    F1
    F2
    F3
    F4
    F5
    F6
    F7
    F8
    F9
    F10
    F11
    F12
    UP
    DOWN
    LEFT
    RIGHT
    SPACE
    TAB
    SHIFT
    CTRL
    ALT
    INS
    DEL
    HOME
    [END]
    PGUP
    PGDN
    BACK
    ESCAPE
    [RETURN]
    ENTER
    PAUSE
    SCROLL
    NP0
    NP1
    NP2
    NP3
    NP4
    NP5
    NP6
    NP7
    NP8
    NP9
    NP_MUL
    NP_DIV
    NP_ADD
    NP_SUB
    NP_DECIMAL
    PERIOD
    EQUALS
    COMMA
    MINUS
    OEM_1
    OEM_2
    OEM_3
    OEM_4
    OEM_5
    OEM_6
    OEM_7
    OEM_8
    CAPS_LOCK
    ENUM_END
  End Enum

  Private ReadOnly m_mouseButtons As Byte = 5
  Private ReadOnly m_defaultAlpha As Byte = &HFF
  Private ReadOnly m_defaultPixel As Integer = m_defaultAlpha << 24
  Private ReadOnly m_tabSizeInSpaces As Byte = 4
  Private Const OLC_MAX_VERTS = 128

  Private ReadOnly m_fontSprite As New Dictionary(Of BuiltinFont, Sprite)
  Private m_spacing(95) As Byte
  Private ReadOnly m_fontSpacing As Vi2d()
  Private m_KeyboardMap As List(Of Tuple(Of Key, String, String))

  Protected Friend Sub New()
    Title = "Undefined"
    Pge = Me
  End Sub

  ' Removed for MAUI - Window management handled by MAUI

  Public Function GetScreenSize() As (w As Integer, h As Integer)
    ' For MAUI, return default screen size
    ' Can be overridden to get actual device screen size
    Return (1920, 1080)
  End Function

  Public ReadOnly Property IsFullScreen As Boolean
    Get
      Return m_fullScreen
    End Get
  End Property

  Public Sub ToggleFullScreen()
    ' Toggle full screen state for MAUI
    m_fullScreen = Not m_fullScreen
  End Sub

  Public Sub DecreasePixelSize()
    If m_pixelWidth > 1 AndAlso m_pixelHeight > 1 Then
      m_pixelWidth -= 1
      m_pixelHeight -= 1
      m_windowWidth = m_screenWidth * m_pixelWidth
      m_windowHeight = m_screenHeight * m_pixelHeight
      ' MAUI: Pixel size change handled by renderer scale
    End If
  End Sub

  Public Sub IncreasePixelSize()
    ' Possibly limit to a maximum?
    m_pixelWidth += 1
    m_pixelHeight += 1
    m_windowWidth = m_screenWidth * m_pixelWidth
    m_windowHeight = m_screenHeight * m_pixelHeight
    ' MAUI: Pixel size change handled by renderer scale
  End Sub

  Public Function CapsLock() As Boolean
    ' MAUI: Caps lock state - platform-specific implementation needed
    Return False
  End Function

  Public Function NumLock() As Boolean
    ' MAUI: Num lock state - platform-specific implementation needed
    Return False
  End Function

  Public Overloads Function Construct(screenW As Integer, screenH As Integer, Optional fullScreen As Boolean = False, Optional vsync As Boolean = False) As RCode
    Return Construct(screenW, screenH, 1, 1, fullScreen, vsync)
  End Function

  Public Overloads Function Construct(screenW As Integer, screenH As Integer, pixelW As Integer, pixelH As Integer, Optional fullScreen As Boolean = False, Optional vsync As Boolean = False) As RCode

    m_screenWidth = screenW
    m_screenHeight = screenH
    m_pixelWidth = pixelW
    m_pixelHeight = pixelH
    m_fullScreen = fullScreen
    m_enableVSYNC = vsync

    m_pixelX = 2.0F / m_screenWidth
    m_pixelY = 2.0F / m_screenHeight

    If m_pixelWidth = 0 OrElse m_pixelHeight = 0 OrElse m_screenWidth = 0 OrElse m_screenHeight = 0 Then
      Return RCode.Fail
    End If

    ' Load the default font sheet
    Pge_ConstructFontSheet()

    ' Create a sprite that represents the primary drawing target
    m_defaultDrawTarget = New Sprite(m_screenWidth, m_screenHeight)
    SetDrawTarget(Nothing)

    Return RCode.Ok

  End Function

  Protected Sub SetScreenSize(w As Integer, h As Integer)

    m_defaultDrawTarget = Nothing
    m_screenWidth = w
    m_screenHeight = h
    m_defaultDrawTarget = New Sprite(m_screenWidth, m_screenHeight)
    SetDrawTarget(Nothing)

    ' MAUI: SkiaSharpRenderer handles rendering
    Pge_UpdateViewport()

  End Sub

  <DebuggerNonUserCode, DebuggerStepThrough>
  Public Function Start() As RCode
    ' MAUI: Game loop is handled by PixelGameView, this is just for initialization
    Return RCode.Ok
  End Function

  Public Sub SetDrawTarget(target As Sprite)
    If target IsNot Nothing Then
      m_drawTarget = target
    Else
      m_drawTarget = m_defaultDrawTarget
    End If
  End Sub

  Friend ReadOnly Property GetDrawTarget() As Sprite
    Get
      Return m_drawTarget
    End Get
  End Property

  Private Protected ReadOnly Property GetDrawTargetWidth() As Integer
    Get
      If m_drawTarget IsNot Nothing Then
        Return m_drawTarget.Width
      Else
        Return 0
      End If
    End Get
  End Property

  Private Protected ReadOnly Property GetDrawTargetHeight() As Integer
    Get
      If m_drawTarget IsNot Nothing Then
        Return m_drawTarget.Height
      Else
        Return 0
      End If
    End Get
  End Property

  Protected ReadOnly Property IsFocused() As Boolean
    Get
      Return m_hasInputFocus
    End Get
  End Property

  Public ReadOnly Property GetKey(k As Key) As HwButton
    Get
      Return m_keyboardState(k)
    End Get
  End Property

  Public ReadOnly Property GetMouse(b As Integer) As HwButton
    Get
      Return m_mouseState(b)
    End Get
  End Property

  Protected ReadOnly Property GetMouseX() As Integer
    Get
      Return m_mousePos.x
    End Get
  End Property

  Protected ReadOnly Property GetMouseY() As Integer
    Get
      Return m_mousePos.y
    End Get
  End Property

  Friend ReadOnly Property GetMousePos() As Vi2d
    Get
      Return m_mousePos
    End Get
  End Property

  Protected ReadOnly Property GetMouseWheel() As Integer
    Get
      Return m_mouseWheelDelta
    End Get
  End Property

  Public ReadOnly Property ScreenWidth As Integer
    Get
      Return m_screenWidth
    End Get
  End Property

  Public ReadOnly Property ScreenHeight As Integer
    Get
      Return m_screenHeight
    End Get
  End Property

  Friend Function GetElapsedTime() As Single
    Return m_lastElapsed
  End Function

  Protected Function GetPixel(x As Double, y As Double) As Pixel
    Return GetPixel(CInt(x), CInt(y))
  End Function

  Protected Function GetPixel(x As Integer, y As Integer) As Pixel
    Return m_drawTarget.GetPixel(x, y)
  End Function

  Protected Function Draw(pos As Vi2d) As Boolean
    Return Draw(pos.x, pos.y, Presets.White)
  End Function

  Protected Function Draw(pos As Vi2d, p As Pixel) As Boolean
    Return Draw(pos.x, pos.y, p)
  End Function

  Protected Overridable Function Draw(x As Double, y As Double) As Boolean
    Return Draw(x, y, Presets.White)
  End Function

  Protected Overridable Function Draw(x As Integer, y As Integer) As Boolean
    Return Draw(x, y, Presets.White)
  End Function

  Public Function Draw(x As Double, y As Double, p As Pixel) As Boolean
    Return Draw(CInt(x), CInt(y), p)
  End Function

  Public Function Draw(x As Integer, y As Integer, p As Pixel) As Boolean

    If m_drawTarget Is Nothing Then
      Return False
    End If

    If m_pixelMode = Pixel.Mode.Normal Then
      Return m_drawTarget.SetPixel(x, y, p)
    End If

    If m_pixelMode = Pixel.Mode.Mask Then
      If p.A = 255 Then
        Return m_drawTarget.SetPixel(x, y, p)
      End If
    End If

    If m_pixelMode = Pixel.Mode.Alpha Then
      Dim d = m_drawTarget.GetPixel(x, y)
      Dim a = p.A / 255.0F * m_blendFactor
      Dim c = 1.0F - a
      Dim r = a * p.R + c * d.R
      Dim g = a * p.G + c * d.G
      Dim b = a * p.B + c * d.B
      Return m_drawTarget.SetPixel(x, y, New Pixel(CByte(r), CByte(g), CByte(b)))
    End If

    If m_pixelMode = Pixel.Mode.Custom Then
      Return m_drawTarget.SetPixel(x, y, m_funcPixelMode(x, y, p, m_drawTarget.GetPixel(x, y)))
    End If

    Return False

  End Function

  Protected Sub SetSubPixelOffset(ox As Single, oy As Single)
    m_subPixelOffsetX = ox * m_pixelX
    m_subPixelOffsetY = oy * m_pixelY
  End Sub

  Protected Sub DrawLine(pos1 As Vi2d, pos2 As Vi2d)
    DrawLine(pos1.x, pos1.y, pos2.x, pos2.y, Presets.White, &HFFFFFFFFUI)
  End Sub

  Protected Sub DrawLine(pos1 As Vi2d, pos2 As Vi2d, p As Pixel, Optional pattern As UInteger = &HFFFFFFFFUI)
    DrawLine(pos1.x, pos1.y, pos2.x, pos2.y, p, pattern)
  End Sub

  Friend Sub DrawLine(pos1 As Vf2d, pos2 As Vi2d, p As Pixel, Optional pattern As UInteger = &HFFFFFFFFUI)
    DrawLine(pos1.x, pos1.y, pos2.x, pos2.y, p, pattern)
  End Sub

  Protected Sub DrawLine(x1 As Double, y1 As Double, x2 As Double, y2 As Double)
    DrawLine(x1, y1, x2, y2, Presets.White, &HFFFFFFFFUI)
  End Sub

  Protected Sub DrawLine(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer)
    DrawLine(x1, y1, x2, y2, Presets.White, &HFFFFFFFFUI)
  End Sub

  Public Sub DrawLine(x1 As Double, y1 As Double, x2 As Double, y2 As Double, p As Pixel, Optional pattern As UInteger = &HFFFFFFFFUI)
    DrawLine(CInt(x1), CInt(y1), CInt(x2), CInt(y2), p, pattern)
  End Sub

  Public Sub DrawLine(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, p As Pixel, Optional pattern As UInteger = &HFFFFFFFFUI)

    Dim dx = x2 - x1
    Dim dy = y2 - y1

    Dim rol = New Func(Of Boolean)(Function()
                                     pattern = (pattern << 1) Or (pattern >> 31)
                                     Return CInt(pattern And 1) <> 0
                                   End Function)

    ' straight line idea by gurkanctn
    If dx = 0 Then ' Line is vertical
      If y2 < y1 Then Swap(y1, y2)
      For y = y1 To y2
        If rol() Then Draw(x1, y, p)
      Next
      Return
    End If

    If dy = 0 Then ' Line is horizontal
      If x2 < x1 Then Swap(x1, x2)
      For x = x1 To x2
        If rol() Then Draw(x, y1, p)
      Next
      Return
    End If

    ' Line is Funk-aye
    Dim dx1 = MathF.Abs(dx) : Dim dy1 = MathF.Abs(dy)
    Dim px = 2 * dy1 - dx1 : Dim py = 2 * dx1 - dy1

    If dy1 <= dx1 Then

      Dim x, y As Integer
      Dim xe As Integer

      If dx >= 0 Then
        x = x1 : y = y1 : xe = x2
      Else
        x = x2 : y = y2 : xe = x1
      End If
      If rol() Then Draw(x, y, p)

      For i = 0 To xe - x
        x += 1
        If px < 0 Then
          px += 2 * dy1
        Else
          If (dx < 0 AndAlso dy < 0) OrElse (dx > 0 AndAlso dy > 0) Then
            y += 1
          Else
            y -= 1
          End If
          px += 2 * (dy1 - dx1)
        End If
        If rol() Then Draw(x, y, p)
      Next

    Else

      Dim x, y As Integer
      Dim ye As Integer

      If dy >= 0 Then
        x = x1 : y = y1 : ye = y2
      Else
        x = x2 : y = y2 : ye = y1
      End If
      If rol() Then Draw(x, y, p)

      For i = 0 To ye - y
        y += 1
        If py <= 0 Then
          py += 2 * dx1
        Else
          If (dx < 0 AndAlso dy < 0) OrElse (dx > 0 AndAlso dy > 0) Then
            x += 1
          Else
            x -= 1
          End If
          py += 2 * (dx1 - dy1)
        End If
        If rol() Then Draw(x, y, p)
      Next

    End If

  End Sub

  Protected Sub DrawCircle(pos As Vi2d, radius As Integer)
    DrawCircle(pos.x, pos.y, radius, Presets.White, &HFF)
  End Sub

  Protected Sub DrawCircle(pos As Vi2d, radius As Integer, p As Pixel, Optional mask As Byte = &HFF)
    DrawCircle(pos.x, pos.y, radius, p, mask)
  End Sub

  Public Sub DrawCircle(pos As Vf2d, radius As Integer, p As Pixel, Optional mask As Byte = &HFF)
    DrawCircle(pos.x, pos.y, radius, p, mask)
  End Sub

  Public Sub DrawCircle(pos As Vf2d, radius As Single, p As Pixel, Optional mask As Byte = &HFF)
    DrawCircle(pos.x, pos.y, radius, p, mask)
  End Sub

  Protected Sub DrawCircle(x As Double, y As Double, radius As Double)
    DrawCircle(x, y, radius, Presets.White, &HFF)
  End Sub

  Protected Sub DrawCircle(x As Integer, y As Integer, radius As Integer)
    DrawCircle(x, y, radius, Presets.White, &HFF)
  End Sub

  Public Sub DrawCircle(x As Double, y As Double, radius As Double, p As Pixel, Optional mask As Byte = &HFF)
    DrawCircle(CInt(x), CInt(y), CInt(radius), p, mask)
  End Sub

  Public Sub DrawCircle(x As Integer, y As Integer, radius As Integer, p As Pixel, Optional mask As Byte = &HFF)

    Dim x0 = 0
    Dim y0 = radius
    Dim d = 3 - 2 * radius
    If radius = 0 Then Return

    While y0 >= x0 ' only formulate 1/8 of circle
      If (mask And &H1) <> 0 Then Draw(x + x0, y - y0, p)
      If (mask And &H2) <> 0 Then Draw(x + y0, y - x0, p)
      If (mask And &H4) <> 0 Then Draw(x + y0, y + x0, p)
      If (mask And &H8) <> 0 Then Draw(x + x0, y + y0, p)
      If (mask And &H10) <> 0 Then Draw(x - x0, y + y0, p)
      If (mask And &H20) <> 0 Then Draw(x - y0, y + x0, p)
      If (mask And &H40) <> 0 Then Draw(x - y0, y - x0, p)
      If (mask And &H80) <> 0 Then Draw(x - x0, y - y0, p)
      If d < 0 Then
        d += 4 * x0 + 6 : x0 += 1
      Else
        d += 4 * (x0 - y0) + 10 : x0 += 1 : y0 -= 1
      End If
    End While

  End Sub

  Public Sub FloodFill(x As Integer, y As Integer, c As Pixel)

    If x < 0 Then x = 0
    If x > ScreenWidth - 1 Then x = ScreenWidth - 1
    If y < 0 Then y = 0
    If y > ScreenHeight - 1 Then y = ScreenHeight - 1

    Dim p = New Vi2d(x, y)
    Dim stk As New Stack()
    stk.Push(p)
    Dim replacementColor = GetPixel(p.x, p.y)
    Do While stk.Count <> 0
      p = CType(stk.Pop(), Vi2d)
      Dim testColor = GetPixel(p.x, p.y)
      If testColor = replacementColor Then
        Draw(p.x, p.y, c)
        If p.x - 1 > -1 Then stk.Push(New Vi2d(p.x - 1, p.y))
        If p.x + 1 < ScreenWidth Then stk.Push(New Vi2d(p.x + 1, p.y))
        If p.y - 1 > -1 Then stk.Push(New Vi2d(p.x, p.y - 1))
        If p.y + 1 < ScreenHeight Then stk.Push(New Vi2d(p.x, p.y + 1))
      End If
    Loop
  End Sub

  Protected Sub FillCircle(pos As Vi2d, radius As Integer)
    FillCircle(pos.x, pos.y, radius, Presets.White)
  End Sub

  Protected Sub FillCircle(pos As Vi2d, radius As Integer, p As Pixel)
    FillCircle(pos.x, pos.y, radius, p)
  End Sub

  Friend Sub FillCircle(pos As Vf2d, radius As Integer, p As Pixel)
    FillCircle(pos.x, pos.y, radius, p)
  End Sub

  Public Sub FillCircle(x As Double, y As Double, radius As Double)
    FillCircle(x, y, radius, Presets.White)
  End Sub

  Public Sub FillCircle(x As Integer, y As Integer, radius As Integer)
    FillCircle(x, y, radius, Presets.White)
  End Sub

  Public Sub FillCircle(x As Double, y As Double, radius As Double, p As Pixel)
    FillCircle(CInt(x), CInt(y), CInt(radius), p)
  End Sub

  Public Sub FillCircle(x As Integer, y As Integer, radius As Integer, p As Pixel)

    Dim x0 = 0
    Dim y0 = radius
    Dim d = 3 - 2 * radius
    If radius = 0 Then Return

    Dim drawLine = Sub(sx As Integer, ex As Integer, ny As Integer)
                     For i = sx To ex
                       Draw(i, ny, p)
                     Next
                   End Sub

    While y0 >= x0
      drawLine(x - x0, x + x0, y - y0)
      drawLine(x - y0, x + y0, y - x0)
      drawLine(x - x0, x + x0, y + y0)
      drawLine(x - y0, x + y0, y + x0)
      If d < 0 Then
        d += 4 * x0 + 6 : x0 += 1
      Else
        d += 4 * (x0 - y0) + 10 : x0 += 1 : y0 -= 1
      End If
    End While

  End Sub

  Protected Sub DrawRect(pos As Vi2d, size As Vi2d)
    DrawRect(pos.x, pos.y, size.x, size.y, Presets.White)
  End Sub

  Protected Sub DrawRect(pos As Vi2d, size As Vi2d, p As Pixel)
    DrawRect(pos.x, pos.y, size.x, size.y, p)
  End Sub

  Protected Sub DrawRect(x As Double, y As Double, w As Double, h As Double)
    DrawRect(x, y, w, h, Presets.White)
  End Sub

  Protected Sub DrawRect(x As Integer, y As Integer, w As Integer, h As Integer)
    DrawRect(x, y, w, h, Presets.White)
  End Sub

  Public Sub DrawRect(x As Double, y As Double, w As Double, h As Double, p As Pixel)
    DrawRect(CInt(x), CInt(y), CInt(w), CInt(h), p)
  End Sub

  Public Sub DrawRect(pos As Vf2d, size As Vf2d, p As Pixel)
    DrawRect(CInt(pos.x), CInt(pos.y), CInt(size.x), CInt(size.y), p)
  End Sub

  Public Sub DrawRect(x As Integer, y As Integer, w As Integer, h As Integer, p As Pixel)
    DrawLine(x, y, x + w, y, p)
    DrawLine(x + w, y, x + w, y + h, p)
    DrawLine(x + w, y + h, x, y + h, p)
    DrawLine(x, y + h, x, y, p)
  End Sub

  Protected Shared Function QBColor(index As Double) As Pixel
    Return QBColor(CInt(index))
  End Function

  Protected Shared Function QBColor(index As Integer) As Pixel
    Select Case index Mod 16
      Case 0 : Return Presets.Black
      Case 1 : Return Presets.DarkBlue
      Case 2 : Return Presets.DarkGreen
      Case 3 : Return Presets.DarkCyan
      Case 4 : Return Presets.DarkRed
      Case 5 : Return Presets.DarkMagenta
      Case 6 : Return Presets.Brown
      Case 7 : Return Presets.DarkGrey
      Case 8 : Return Presets.Gray
      Case 9 : Return Presets.Blue
      Case 10 : Return Presets.Green
      Case 11 : Return Presets.Cyan
      Case 12 : Return Presets.Red
      Case 13 : Return Presets.Magenta
      Case 14 : Return Presets.Yellow
      Case 15 : Return Presets.White
      Case Else
        Return Presets.Black
    End Select
  End Function

  Public Sub Clear()
    Clear(Presets.Black)
  End Sub

  Public Sub Clear(p As Pixel)
    Dim pixels = GetDrawTargetWidth() * GetDrawTargetHeight()
    Dim m() = GetDrawTarget().GetData()
    For i = 0 To pixels - 1
      m(i) = p
    Next
#If PGE_DBG_OVERDRAW Then
    Sprite.nOverdrawCount += pixels
#End If
  End Sub

  Protected Sub FillRect(pos As Vi2d, size As Vi2d)
    FillRect(pos.x, pos.y, size.x, size.y, Presets.White)
  End Sub

  Protected Sub FillRect(pos As Vi2d, size As Vi2d, p As Pixel)
    FillRect(pos.x, pos.y, size.x, size.y, p)
  End Sub

  Protected Sub FillRect(x As Double, y As Double, w As Double, h As Double)
    FillRect(x, y, w, h, Presets.White)
  End Sub

  Protected Sub FillRect(x As Integer, y As Integer, w As Integer, h As Integer)
    FillRect(x, y, w, h, Presets.White)
  End Sub

  Public Sub FillRect(pos As Vf2d, size As Vf2d, p As Pixel)
    FillRect(CInt(pos.x), CInt(pos.y), CInt(size.x), CInt(size.y), p)
  End Sub

  Public Sub FillRect(x As Double, y As Double, w As Double, h As Double, p As Pixel)
    FillRect(CInt(x), CInt(y), CInt(w), CInt(h), p)
  End Sub

  Public Sub FillRect(x As Integer, y As Integer, w As Integer, h As Integer, p As Pixel)

    Dim x2 = x + w
    Dim y2 = y + h

    If x < 0 Then x = 0
    If x >= GetDrawTargetWidth() Then x = GetDrawTargetWidth()
    If y < 0 Then y = 0
    If y >= GetDrawTargetHeight() Then y = GetDrawTargetHeight()

    If x2 < 0 Then x2 = 0
    If x2 >= GetDrawTargetWidth() Then x2 = GetDrawTargetWidth()
    If y2 < 0 Then y2 = 0
    If y2 >= GetDrawTargetHeight() Then y2 = GetDrawTargetHeight()

    For i = x To x2 - 1
      For j = y To y2 - 1
        Draw(i, j, p)
      Next
    Next

  End Sub

  Protected Sub DrawTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d)
    DrawTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, Presets.White)
  End Sub

  Protected Sub DrawTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d, p As Pixel)
    DrawTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, p)
  End Sub

  Protected Sub DrawTriangle(x1 As Double, y1 As Double, x2 As Double, y2 As Double, x3 As Double, y3 As Double)
    DrawTriangle(x1, y1, x2, y2, x3, y3, Presets.White)
  End Sub

  Protected Sub DrawTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer)
    DrawTriangle(x1, y1, x2, y2, x3, y3, Presets.White)
  End Sub

  Public Sub DrawTriangle(x1 As Double, y1 As Double, x2 As Double, y2 As Double, x3 As Double, y3 As Double, p As Pixel)
    DrawTriangle(CInt(x1), CInt(y1), CInt(x2), CInt(y2), CInt(x3), CInt(y3), p)
  End Sub

  Public Sub DrawTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer, p As Pixel)
    DrawLine(x1, y1, x2, y2, p)
    DrawLine(x2, y2, x3, y3, p)
    DrawLine(x3, y3, x1, y1, p)
  End Sub

  Protected Sub FillTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d)
    FillTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, Presets.White)
  End Sub

  Protected Sub FillTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d, p As Pixel)
    FillTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, p)
  End Sub

  Protected Sub FillTriangle(x1 As Double, y1 As Double, x2 As Double, y2 As Double, x3 As Double, y3 As Double)
    FillTriangle(x1, y1, x2, y2, x3, y3, Presets.White)
  End Sub

  Protected Sub FillTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer)
    FillTriangle(x1, y1, x2, y2, x3, y3, Presets.White)
  End Sub

  Public Sub FillTriangle(x1 As Double, y1 As Double, x2 As Double, y2 As Double, x3 As Double, y3 As Double, p As Pixel)
    FillTriangle(CInt(x1), CInt(y1), CInt(x2), CInt(y2), CInt(x3), CInt(y3), p)
  End Sub

  Public Sub FillTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer, p As Pixel)

    Dim drawline = Sub(sx As Integer, ex As Integer, ny As Integer)
                     For i = sx To ex
                       Draw(i, ny, p)
                     Next
                   End Sub

    Dim t1x, t2x, y, minx, maxx, t1xp, t2xp As Integer
    Dim changed1, changed2 As Boolean
    Dim signx1, signx2, dx1, dy1, dx2, dy2 As Integer
    Dim e1, e2 As Integer
    ' Sort vertices
    If y1 > y2 Then Swap(y1, y2) : Swap(x1, x2)
    If y1 > y3 Then Swap(y1, y3) : Swap(x1, x3)
    If y2 > y3 Then Swap(y2, y3) : Swap(x2, x3)

    t1x = x1 : t2x = x1 : y = y1 ' Starting points
    dx1 = x2 - x1
    If dx1 < 0 Then
      dx1 = -dx1
      signx1 = -1
    Else
      signx1 = 1
    End If
    dy1 = y2 - y1

    dx2 = x3 - x1
    If dx2 < 0 Then
      dx2 = -dx2
      signx2 = -1
    Else
      signx2 = 1
    End If
    dy2 = y3 - y1

    If dy1 > dx1 Then ' swap values
      Swap(dx1, dy1) : changed1 = True
    End If
    If dy2 > dx2 Then ' swap values
      Swap(dy2, dx2) : changed2 = True
    End If

    e2 = dx2 >> 1
    ' Flat top, just process the second half
    If y1 = y2 Then GoTo nextx
    e1 = dx1 >> 1

    For i = 0 To dx1 - 1
      t1xp = 0 : t2xp = 0

      If t1x < t2x Then
        minx = t1x : maxx = t2x
      Else
        minx = t2x : maxx = t1x
      End If

      ' process first line until y value is about to change
      While i < dx1
        'i += 1
        e1 += dy1
        While e1 >= dx1
          e1 -= dx1
          If changed1 Then
            t1xp = signx1
          Else
            GoTo next1
          End If
        End While
        If changed1 Then Exit While
        t1x += signx1
      End While
      ' Move line
next1:
      ' Process second line until y value is about to change
      While True
        e2 += dy2
        While e2 >= dx2
          e2 -= dx2
          If changed2 Then
            t2xp = signx2 ' t2x += signx2
          Else
            GoTo next2
          End If
        End While
        If changed2 Then Exit While
        t2x += signx2
      End While
next2:
      If minx > t1x Then minx = t1x
      If minx > t2x Then minx = t2x
      If maxx < t1x Then maxx = t1x
      If maxx < t2x Then maxx = t2x
      drawline(minx, maxx, y)    ' Draw line from min to max points found on the y
      ' Now increase y
      If Not changed1 Then t1x += signx1
      t1x += t1xp
      If Not changed2 Then t2x += signx2
      t2x += t2xp
      y += 1
      If y = y2 Then Exit For
    Next
nextx:
    ' Second half
    dx1 = x3 - x2
    If dx1 < 0 Then
      dx1 = -dx1 : signx1 = -1
    Else
      signx1 = 1
    End If
    dy1 = y3 - y2 : t1x = x2

    If dy1 > dx1 Then ' swap values
      Swap(dy1, dx1) : changed1 = True
    Else
      changed1 = False
    End If

    e1 = dx1 >> 1

    For i = 0 To dx1
      t1xp = 0
      t2xp = 0
      If t1x < t2x Then
        minx = t1x : maxx = t2x
      Else
        minx = t2x : maxx = t1x
      End If
      ' process first line until y value is about to change
      While i < dx1
        e1 += dy1
        While e1 >= dx1
          e1 -= dx1
          If changed1 Then t1xp = signx1 : Exit While ' t1x += signx1
          GoTo next3
        End While
        If changed1 Then Exit While
        t1x += signx1
        If i < dx1 Then i += 1
      End While
next3:
      ' process second line until y value is about to change
      While t2x <> x3
        e2 += dy2
        While e2 >= dx2
          e2 -= dx2
          If changed2 Then
            t2xp = signx2
          Else
            GoTo next4
          End If
        End While
        If changed2 Then Exit While
        t2x += signx2
      End While
next4:

      If minx > t1x Then minx = t1x
      If minx > t2x Then minx = t2x
      If maxx < t1x Then maxx = t1x
      If maxx < t2x Then maxx = t2x
      drawline(minx, maxx, y)
      If Not changed1 Then t1x += signx1
      t1x += t1xp
      If Not changed2 Then t2x += signx2
      t2x += t2xp
      y += 1
      If y > y3 Then Return

    Next

  End Sub

  Protected Sub DrawSprite(pos As Vi2d, sprite As Sprite, Optional scale As Integer = 1)
    DrawSprite(pos.x, pos.y, sprite, scale)
  End Sub

  Friend Sub DrawSprite(pos As Vf2d, sprite As Sprite, Optional scale As Integer = 1)
    DrawSprite(pos.x, pos.y, sprite, scale)
  End Sub

  Public Sub DrawSprite(x As Double, y As Double, sprite As Sprite, Optional scale As Integer = 1)
    DrawSprite(CInt(x), CInt(y), sprite, scale)
  End Sub

  Public Sub DrawSprite(x As Integer, y As Integer, sprite As Sprite, Optional scale As Integer = 1)

    If sprite Is Nothing Then Return

    If scale > 1 Then
      For i = 0 To sprite.Width - 1
        For j = 0 To sprite.Height - 1
          For iIs = 0 To scale - 1
            For js = 0 To scale - 1
              Dim dx = x + (i * scale) + iIs
              Dim dy = y + (j * scale) + js
              If dx < 0 OrElse dy < 0 OrElse dx > ScreenWidth - 1 OrElse dy > ScreenHeight - 1 Then Continue For
              Draw(dx, dy, sprite.GetPixel(i, j))
            Next
          Next
        Next
      Next
    Else
      For i = 0 To sprite.Width - 1
        For j = 0 To sprite.Height - 1
          Dim dx = x + i, dy = y + j
          If dx < 0 OrElse dy < 0 OrElse dx > ScreenWidth - 1 OrElse dy > ScreenHeight - 1 Then Continue For
          Draw(dx, dy, sprite.GetPixel(i, j))
        Next
      Next
    End If

  End Sub

  Protected Sub DrawPartialSprite(pos As Vi2d, sprite As Sprite, sourcepos As Vi2d, size As Vi2d, Optional scale As Integer = 1)
    DrawPartialSprite(pos.x, pos.y, sprite, sourcepos.x, sourcepos.y, size.x, size.y, scale)
  End Sub

  Protected Sub DrawPartialSprite(x As Double, y As Double, sprite As Sprite, ox As Double, oy As Double, w As Double, h As Double, Optional scale As Integer = 1)
    DrawPartialSprite(CInt(x), CInt(y), sprite, CInt(ox), CInt(oy), CInt(w), CInt(h), scale)
  End Sub

  Protected Sub DrawPartialSprite(x As Integer, y As Integer, sprite As Sprite, ox As Integer, oy As Integer, w As Integer, h As Integer, Optional scale As Integer = 1)

    If sprite Is Nothing Then Return
    
    If scale > 1 Then
      For i = 0 To w - 1
        For j = 0 To h - 1
          For iIs = 0 To scale - 1
            For js = 0 To scale - 1
              Draw(x + (i * scale) + iIs, y + (j * scale) + js, sprite.GetPixel(i + ox, j + oy))
            Next
          Next
        Next
      Next
    Else
      For i = 0 To w - 1
        For j = 0 To h - 1
          Draw(x + i, y + j, sprite.GetPixel(i + ox, j + oy))
        Next
      Next
    End If

  End Sub

  Public Function GetTextSize(text As String) As Vi2d
    Dim size = New Vi2d(0, 1)
    Dim pos = New Vi2d(0, 1)
    For Each c In text
      If c = vbLf Then
        pos.y += 1
        pos.x = 0
      ElseIf c = vbTab Then
        pos.x += m_tabSizeInSpaces
      Else
        pos.x += 1
      End If
      size.x = Math.Max(size.x, pos.x)
      size.y = Math.Max(size.y, pos.y)
    Next
    Return size * 8
  End Function

  Public Sub DrawString(pos As Vi2d, text As String)
    DrawString(pos.x, pos.y, text, Presets.White, 1)
  End Sub

  Public Sub DrawString(pos As Vi2d, text As String, col As Pixel, Optional scale As Integer = 1)
    DrawString(pos.x, pos.y, text, col, scale)
  End Sub

  Public Sub DrawString(x As Double, y As Double, text As String)
    DrawString(x, y, text, Presets.White, 1)
  End Sub

  Public Sub DrawString(x As Integer, y As Integer, text As String)
    DrawString(x, y, text, Presets.White, 1)
  End Sub

  Public Sub DrawString(x As Double, y As Double, text As String, col As Pixel, Optional scale As Integer = 1, Optional font As BuiltinFont = 0)
    DrawString(CInt(x), CInt(y), text, col, scale, font)
  End Sub

  Public Sub DrawString(x As Integer, y As Integer, text As String, col As Pixel, Optional scale As Integer = 1, Optional font As BuiltinFont = 0)

    Dim sx = 0
    Dim sy = 0
    Dim m = m_pixelMode

    'If m <> Pixel.Custom Then
    If col.A <> 255 Then
      SetPixelMode(Pixel.Mode.Alpha)
    Else
      SetPixelMode(Pixel.Mode.Mask)
    End If
    'End If

    Dim fontSpr As Sprite
    If Not m_fontSprite.TryGetValue(font, fontSpr) Then Exit Sub
    For Each c In text
      If c = vbLf Then
        sx = 0
        sy += 8 * scale
      Else
        Dim ox = (Asc(c) - 32) Mod 16
        Dim oy = (Asc(c) - 32) \ 16
        If scale > 1 Then
          For i = 0 To 7
            For j = 0 To 7
              If fontSpr.GetPixel(i + ox * 8, j + oy * 8).R > 0 Then
                For iIs = 0 To scale - 1
                  For js = 0 To scale - 1
                    Draw(x + sx + (i * scale) + iIs, y + sy + (j * scale) + js, col)
                  Next
                Next
              End If
            Next
          Next
        Else
          For i = 0 To 7
            For j = 0 To 7
              If fontSpr.GetPixel(i + ox * 8, j + oy * 8).R > 0 Then
                Draw(x + sx + i, y + sy + j, col)
              End If
            Next
          Next
        End If
        sx += 8 * scale
      End If

    Next

    SetPixelMode(m)

  End Sub

  Friend Function GetTextSizeProp(text As String) As Vi2d
    Dim size = New Vi2d(0, 1)
    Dim pos = New Vi2d(0, 1)
    For Each c In text
      If c = vbLf Then
        pos.y += 1
        pos.x = 0
      ElseIf c = vbTab Then
        pos.x += m_tabSizeInSpaces * 8
      Else
        pos.x += m_fontSpacing(AscW(c) - 32).y
      End If
      size.x = Math.Max(size.x, pos.x)
      size.y = Math.Max(size.y, pos.y)
    Next
    size.y *= 8
    Return size
  End Function

  Friend Sub DrawStringProp(pos As Vi2d, text As String)
    DrawStringProp(pos, text, Presets.White, 1)
  End Sub

  Friend Sub DrawStringProp(pos As Vi2d, text As String, col As Pixel, Optional scale As Integer = 1)
    DrawStringProp(pos.x, pos.y, text, col, scale)
  End Sub

  Friend Sub DrawStringProp(x As Integer, y As Integer, text As String, col As Pixel, Optional scale As Integer = 1, Optional font As BuiltinFont = 0)

    Dim sx = 0
    Dim sy = 0
    Dim m = m_pixelMode

    'If m <> Pixel.Custom Then
    If col.A <> 255 Then
      SetPixelMode(Pixel.Mode.Alpha)
    Else
      SetPixelMode(Pixel.Mode.Mask)
    End If
    'End If
    
    Dim fontSpr As Sprite
    If Not m_fontSprite.TryGetValue(font, fontSpr) Then Exit Sub
    For Each c In text
      If c = vbLf Then
        sx = 0
        sy += 8 * scale
      ElseIf c = vbTab Then
        sx += 8 * m_tabSizeInSpaces * scale
      Else
        Dim ch = Asc(c) - 32
        Dim ox = ch Mod 16
        Dim oy = ch \ 16
        If scale > 1 Then
          For i = 0 To m_fontSpacing(ch).y - 1 '7
            For j = 0 To 7
              If fontSpr.GetPixel(i + ox * 8 + m_fontSpacing(ch).x, j + oy * 8).R > 0 Then
                For iIs = 0 To scale - 1
                  For js = 0 To scale - 1
                    Draw(x + sx + (i * scale) + iIs, y + sy + (j * scale) + js, col)
                  Next
                Next
              End If
            Next
          Next
        Else
          For i = 0 To m_fontSpacing(ch).y - 1 '7
            For j = 0 To 7
              If fontSpr.GetPixel(i + ox * 8 + m_fontSpacing(ch).x, j + oy * 8).R > 0 Then
                Draw(x + sx + i, y + sy + j, col)
              End If
            Next
          Next
        End If
        sx += m_fontSpacing(ch).y * scale
      End If

    Next

    SetPixelMode(m)

  End Sub

  Public Sub SetPixelMode(m As Pixel.Mode)
    m_pixelMode = m
  End Sub

  Protected ReadOnly Property GetPixelMode() As Pixel.Mode
    Get
      Return m_pixelMode
    End Get
  End Property

  Public Sub SetPixelMode(pixelMode As PixelModeDelegate)
    m_funcPixelMode = pixelMode
    m_pixelMode = Pixel.Mode.Custom
  End Sub

  Protected Sub SetPixelBlend(blend As Single)
    m_blendFactor = blend
    If m_blendFactor < 0.0F Then m_blendFactor = 0.0F
    If m_blendFactor > 1.0F Then m_blendFactor = 1.0F
  End Sub

  Public Sub TextEntryEnable(enable As Boolean, Optional text As String = "")
    If enable Then
      m_textEntryCursor = text.Length
      m_textEntryString = text
      m_textEntryEnable = True
    Else
      m_textEntryEnable = False
    End If
  End Sub

  Public Function TextEntryGetString() As String
    Return m_textEntryString
  End Function

  Public Function TextEntryGetCursor() As Integer
    Return m_textEntryCursor
  End Function

  Public Function IsTextEntryEnabled() As Boolean
    Return m_textEntryEnable
  End Function

  Private m_textEntryCursor As Integer
  Private m_textEntryEnable As Boolean
  Private m_textEntryString As String

  Sub UpdateTextEntry()

    ' Check for typed characters
    For Each key In m_KeyboardMap
      If GetKey(key.Item1).Pressed Then
        m_textEntryString = m_textEntryString.Insert(m_textEntryCursor, If(GetKey(PixelGameEngine.Key.SHIFT).Held, key.Item3, key.Item2))
        m_textEntryCursor += 1
      End If
    Next

    ' Check for command characters
    If GetKey(Key.LEFT).Pressed Then
      m_textEntryCursor = Math.Max(0, m_textEntryCursor - 1)
    End If
    If GetKey(Key.RIGHT).Pressed Then
      m_textEntryCursor = Math.Min(m_textEntryString.Length, m_textEntryCursor + 1)
    End If
    If GetKey(Key.BACK).Pressed AndAlso m_textEntryCursor > 0 Then
      m_textEntryString = m_textEntryString.Remove(m_textEntryCursor - 1, 1)
      m_textEntryCursor = Math.Max(0, m_textEntryCursor - 1)
    End If
    If GetKey(Key.DEL).Pressed AndAlso m_textEntryCursor < m_textEntryString.Length Then
      m_textEntryString = m_textEntryString.Remove(m_textEntryCursor, 1)
    End If

    'If GetKey(PixelGameEngine.Key.UP).Pressed Then
    '  If m_commandHistory.Any() Then
    '    If m_commandHistoryIt <> m_commandHistory.GetEnumerator().Current Then
    '      m_commandHistoryIt = m_ommandHistoryIt.Previous()
    '    End If
    '    m_textEntryCursor = m_commandHistoryIt.Value.Length
    '    m_textEntryString = m_commandHistoryIt.Value
    '  End If
    'End If

    'If GetKey(PixelGameEngine.Key.DOWN).Pressed Then
    '  If m_commandHistory.Any() Then
    '    If m_commandHistoryIt <> m_commandHistory.GetEnumerator().End Then
    '      m_commandHistoryIt = m_commandHistoryIt.Next()
    '      If m_commandHistoryIt <> m_commandHistory.GetEnumerator().End Then
    '        m_textEntryCursor = m_commandHistoryIt.Value.Length
    '        m_textEntryString = m_commandHistoryIt.Value
    '      Else
    '        m_textEntryCursor = 0
    '        m_textEntryString = ""
    '      End If
    '    End If
    '  End If
    'End If

    If GetKey(Key.ENTER).Pressed Then
      'If m_consoleShow Then
      '  Console.WriteLine(">" & m_textEntryString)
      '  If OnConsoleCommand(m_textEntryString) Then
      '    m_commandHistory.Add(m_textEntryString)
      '    m_commandHistoryIt = m_commandHistory.GetEnumerator().End
      '  End If
      '  m_textEntryString = ""
      '  m_textEntryCursor = 0
      'Else
      OnTextEntryComplete(m_textEntryString)
      TextEntryEnable(False)
      'End If
    End If

  End Sub

  ''' <summary>
  ''' Called when a text entry is confirmed with "enter" key
  ''' </summary>
  ''' <param name="text"></param>
  Public Overridable Sub OnTextEntryComplete(text As String)
  End Sub

  Protected MustOverride Function OnUserCreate() As Boolean
  Protected Friend MustOverride Function OnUserUpdate(elapsedTime As Single) As Boolean
  Protected Friend MustOverride Function OnUserRender() As Boolean
  Protected MustOverride Function OnUserDestroy() As Boolean

  Private _isDisposed As Boolean = False
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not _isDisposed Then
      Try
        If disposing Then
          ' TODO: dispose managed state (managed objects)
          Debug.WriteLineIf(Not OnUserDestroy(), "[ENGINE WARNING] Some resources failed to dispose.")
        End If

        ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
        ' TODO: set large fields to null
      Finally
        _isDisposed = True
      End Try
    End If
  End Sub

  Protected Overrides Sub Finalize()
    Dispose(disposing:=False)
    MyBase.Finalize()
  End Sub

  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(disposing:=True)
    GC.SuppressFinalize(Me)
  End Sub

  Private Sub Pge_UpdateViewport()

    Dim ww = m_screenWidth * m_pixelWidth
    Dim wh = m_screenHeight * m_pixelHeight
    Dim wasp = CSng(ww / wh)

    m_viewW = m_windowWidth
    m_viewH = CInt(Fix(m_viewW / wasp))

    If m_viewH > m_windowHeight Then
      m_viewH = m_windowHeight
      m_viewW = CInt(Fix(m_viewH * wasp))
    End If

    m_viewX = (m_windowWidth - m_viewW) \ 2
    m_viewY = (m_windowHeight - m_viewH) \ 2

  End Sub

  Private Sub Pge_UpdateWindowSize(x As Integer, y As Integer)
    m_windowWidth = x
    m_windowHeight = y
    Pge_UpdateViewport()
  End Sub

  Private Sub Pge_UpdateMouseWheel(delta As Integer)
    m_mouseWheelDeltaCache += delta
  End Sub

  Public Property ShowEngineName As Boolean = True
  Public Property ShowIPS As Boolean = True

  Private Sub Pge_UpdateMouse(x As Integer, y As Integer)

    ' Mouse coords come in screen space
    ' But leave in pixel space

    ' Full Screen mode may have a weird viewport we must clamp to
    x -= m_viewX
    y -= m_viewY

    m_mousePosXcache = CInt(Fix(x / (m_windowWidth - (m_viewX * 2)) * m_screenWidth))
    m_mousePosYcache = CInt(Fix(y / (m_windowHeight - (m_viewY * 2)) * m_screenHeight))

    If m_mousePosXcache >= m_screenWidth Then m_mousePosXcache = m_screenWidth - 1
    If m_mousePosYcache >= m_screenHeight Then m_mousePosYcache = m_screenHeight - 1

    If m_mousePosXcache < 0 Then m_mousePosXcache = 0
    If m_mousePosYcache < 0 Then m_mousePosYcache = 0

  End Sub

  Private Sub EngineThread()
    ' MAUI/SkiaSharp implementation
    ' Main game loop is now managed by PixelGameView via Timer
    ' This function only handles initialization

    ' Create font sprite sheet for text rendering
    Pge_ConstructFontSheet()

    ' Create user resources
    If Not OnUserCreate() Then AtomActive = False
  End Sub

  Private Sub Pge_ConstructFontSheet()

    ' Note: Pixel font data has been moved to the GetFontSheet() extension method.
    For Each font As BuiltinFont In [Enum].GetValues(Of BuiltinFont)()
      Dim data = font.GetFontSheet()
      m_fontSprite(font) = New Sprite(128, 48)

      Dim px = 0, py = 0
      For b = 0 To 1023 Step 4
        Dim sym1 = AscW(data(b + 0)) - 48
        Dim sym2 = AscW(data(b + 1)) - 48
        Dim sym3 = AscW(data(b + 2)) - 48
        Dim sym4 = AscW(data(b + 3)) - 48
        Dim r = sym1 << 18 Or sym2 << 12 Or sym3 << 6 Or sym4

        For i = 0 To 23
          Dim k = If((r And (1 << i)) <> 0, 255, 0)
          m_fontSprite(font).SetPixel(px, py, New Pixel(k, k, k, k))
          If Interlocked.Increment(py) = 48 Then
            px += 1 : py = 0
          End If
        Next
      Next
    Next

    m_spacing = {
      &H3, &H25, &H16, &H8, &H7, &H8, &H8, &H4, &H15, &H15, &H8, &H7, &H15, &H7, &H24, &H8,
      &H8, &H17, &H8, &H8, &H8, &H8, &H8, &H8, &H8, &H8, &H24, &H15, &H6, &H7, &H16, &H17,
      &H8, &H8, &H8, &H8, &H8, &H8, &H8, &H8, &H8, &H17, &H8, &H8, &H17, &H8, &H8, &H8,
      &H8, &H8, &H8, &H8, &H17, &H8, &H8, &H8, &H8, &H17, &H8, &H15, &H8, &H15, &H8, &H8,
      &H24, &H18, &H17, &H17, &H17, &H17, &H17, &H17, &H17, &H33, &H17, &H17, &H33, &H18, &H17, &H17,
      &H17, &H17, &H17, &H17, &H7, &H17, &H17, &H18, &H18, &H17, &H17, &H7, &H33, &H7, &H8, &H0}

    Dim offset = 0
    For Each c In m_spacing
      m_fontSpacing(offset) = New Vi2d(c >> 4, c And 15)
      offset += 1
    Next
    
    m_KeyboardMap = New List(Of Tuple(Of Key, String, String)) From {
      Tuple.Create(Key.A, "a", "A"), Tuple.Create(Key.B, "b", "B"), Tuple.Create(Key.C, "c", "C"), Tuple.Create(Key.D, "d", "D"), Tuple.Create(Key.E, "e", "E"),
      Tuple.Create(Key.F, "f", "F"), Tuple.Create(Key.G, "g", "G"), Tuple.Create(Key.H, "h", "H"), Tuple.Create(Key.I, "i", "I"), Tuple.Create(Key.J, "j", "J"),
      Tuple.Create(Key.K, "k", "K"), Tuple.Create(Key.L, "l", "L"), Tuple.Create(Key.M, "m", "M"), Tuple.Create(Key.N, "n", "N"), Tuple.Create(Key.O, "o", "O"),
      Tuple.Create(Key.P, "p", "P"), Tuple.Create(Key.Q, "q", "Q"), Tuple.Create(Key.R, "r", "R"), Tuple.Create(Key.S, "s", "S"), Tuple.Create(Key.T, "t", "T"),
      Tuple.Create(Key.U, "u", "U"), Tuple.Create(Key.V, "v", "V"), Tuple.Create(Key.W, "w", "W"), Tuple.Create(Key.X, "x", "X"), Tuple.Create(Key.Y, "y", "Y"),
      Tuple.Create(Key.Z, "z", "Z"),
      Tuple.Create(Key.K0, "0", ")"), Tuple.Create(Key.K1, "1", "!"), Tuple.Create(Key.K2, "2", """"), Tuple.Create(Key.K3, "3", "#"), Tuple.Create(Key.K4, "4", "$"),
      Tuple.Create(Key.K5, "5", "%"), Tuple.Create(Key.K6, "6", "^"), Tuple.Create(Key.K7, "7", "&"), Tuple.Create(Key.K8, "8", "*"), Tuple.Create(Key.K9, "9", "("),
      Tuple.Create(Key.NP0, "0", "0"), Tuple.Create(Key.NP1, "1", "1"), Tuple.Create(Key.NP2, "2", "2"), Tuple.Create(Key.NP3, "3", "3"), Tuple.Create(Key.NP4, "4", "4"),
      Tuple.Create(Key.NP5, "5", "5"), Tuple.Create(Key.NP6, "6", "6"), Tuple.Create(Key.NP7, "7", "7"), Tuple.Create(Key.NP8, "8", "8"), Tuple.Create(Key.NP9, "9", "9"),
      Tuple.Create(Key.NP_MUL, "*", "*"), Tuple.Create(Key.NP_DIV, "/", "/"), Tuple.Create(Key.NP_ADD, "+", "+"), Tuple.Create(Key.NP_SUB, "-", "-"), Tuple.Create(Key.NP_DECIMAL, ".", "."),
      Tuple.Create(Key.PERIOD, ".", ">"), Tuple.Create(Key.EQUALS, "=", "+"), Tuple.Create(Key.COMMA, ",", "<"), Tuple.Create(Key.MINUS, "-", "_"), Tuple.Create(Key.SPACE, " ", " "),
      Tuple.Create(Key.OEM_1, ";", ":"), Tuple.Create(Key.OEM_2, "/", "?"), Tuple.Create(Key.OEM_3, "'", "@"), Tuple.Create(Key.OEM_4, "[", "{"),
      Tuple.Create(Key.OEM_5, "\", "|"), Tuple.Create(Key.OEM_6, "]", "}"), Tuple.Create(Key.OEM_7, "#", "~")}

  End Sub
#Region "Additional"

  Private Shared Function GET_WHEEL_DELTA_WPARAM(wParam As IntPtr) As Integer
    Dim l = wParam.ToInt64
    Dim v = CInt(If(l > Integer.MaxValue, l - UInteger.MaxValue, l))
    Return CShort(v >> 16)
  End Function

  Public Shared Sub Swap(Of T)(ByRef left As T, ByRef right As T)
    Dim temp = left
    left = right
    right = temp
  End Sub

#End Region

#Region "C++'isms"

  ' Since a lot of the olcPGE examples use `rand`,
  ' including similar functionality here to reduce
  ' constantly trying to translate C++ code to VB
  ' for this common scenario.

  Private ReadOnly m_random As New Random
  Protected Const RAND_MAX As Integer = 2147483647

  Protected ReadOnly Property Coin As Integer
    Get
      Return If(Rnd >= 0.5, 1, 0)
    End Get
  End Property

  Protected ReadOnly Property Rnd As Double
    Get
      Return m_random.NextDouble
    End Get
  End Property

  ' Provide for something *similar* to C++.
  Public ReadOnly Property Rand As Integer
    Get
      Return CInt(Fix(m_random.NextDouble * RAND_MAX))
    End Get
  End Property

#End Region

#Region "CGE"

  ' I've added these so that compatibility with CGE can be retained.

  Private Shared Function ConsoleColor2PixelColor(c As Color) As Pixel
    Select Case c
      Case Color.FgBlack : Return Presets.Black
      Case Color.FgDarkBlue : Return Presets.DarkBlue
      Case Color.FgDarkGreen : Return Presets.DarkGreen
      Case Color.FgDarkCyan : Return Presets.DarkCyan
      Case Color.FgDarkRed : Return Presets.DarkRed
      Case Color.FgDarkMagenta : Return Presets.DarkMagenta
      Case Color.FgDarkYellow : Return Presets.DarkYellow
      Case Color.FgGray : Return Presets.Gray
      Case Color.FgDarkGray : Return Presets.DarkGrey
      Case Color.FgBlue : Return Presets.Blue
      Case Color.FgGreen : Return Presets.Green
      Case Color.FgCyan : Return Presets.Cyan
      Case Color.FgRed : Return Presets.Red
      Case Color.FgMagenta : Return Presets.Magenta
      Case Color.FgYellow : Return Presets.Yellow
      Case Color.FgWhite : Return Presets.White
      Case Color.BgBlack : Return Presets.Black
      Case Color.BgDarkBlue : Return Presets.DarkBlue
      Case Color.BgDarkGreen : Return Presets.DarkGreen
      Case Color.BgDarkCyan : Return Presets.DarkCyan
      Case Color.BgDarkRed : Return Presets.DarkRed
      Case Color.BgDarkMagenta : Return Presets.DarkMagenta
      Case Color.BgDarkYellow : Return Presets.DarkYellow
      Case Color.BgGray : Return Presets.Gray
      Case Color.BgDarkGray : Return Presets.DarkGrey
      Case Color.BgBlue : Return Presets.Blue
      Case Color.BgGreen : Return Presets.Green
      Case Color.BgCyan : Return Presets.Cyan
      Case Color.BgRed : Return Presets.Red
      Case Color.BgMagenta : Return Presets.Magenta
      Case Color.BgYellow : Return Presets.Yellow
      Case Color.BgWhite : Return Presets.White
      Case Else
        Return Presets.White
    End Select
  End Function

  Public Function ConstructConsole(w As Integer, h As Integer, pw As Integer, ph As Integer) As RCode
    Return Construct(w, h, pw, ph)
  End Function

  Protected Sub Fill(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    Dim w = x2 - x1 + 1
    Dim h = y2 - y1 + 1
    FillRect(x1, y1, w, h, ConsoleColor2PixelColor(c))
  End Sub

  Protected Sub FillCircle(x As Single, y As Single, radius As Single, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    FillCircle(CInt(Fix(x)), CInt(Fix(y)), CInt(Fix(radius)), ConsoleColor2PixelColor(c))
  End Sub

  Protected Sub FillCircle(x As Integer, y As Integer, radius As Single, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    FillCircle(x, y, CInt(Fix(radius)), ConsoleColor2PixelColor(c))
  End Sub

  Protected Sub DrawLine(x1 As Single, y1 As Single, x2 As Single, y2 As Single, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    DrawLine(CInt(Fix(x1)), CInt(Fix(y1)), CInt(Fix(x2)), CInt(Fix(y2)), ConsoleColor2PixelColor(c))
  End Sub

  Protected Sub DrawLine(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    DrawLine(x1, y1, x2, y2, ConsoleColor2PixelColor(c))
  End Sub

#End Region

End Class

Public MustInherit Class PgeX

  'Public Shared Property Pge As PixelGameEngine

End Class

#Region "CGE"

Public Enum Color As Short
  FgBlack = &H0
  FgDarkBlue = &H1
  FgDarkGreen = &H2
  FgDarkCyan = &H3
  FgDarkRed = &H4
  FgDarkMagenta = &H5
  FgDarkYellow = &H6
  FgGray = &H7
  FgDarkGray = &H8
  FgBlue = &H9
  FgGreen = &HA
  FgCyan = &HB
  FgRed = &HC
  FgMagenta = &HD
  FgYellow = &HE
  FgWhite = &HF
#Disable Warning CA1069 ' Enums values should not be duplicated
  BgBlack = &H0
#Enable Warning CA1069 ' Enums values should not be duplicated
  BgDarkBlue = &H10
  BgDarkGreen = &H20
  BgDarkCyan = &H30
  BgDarkRed = &H40
  BgDarkMagenta = &H50
  BgDarkYellow = &H60
  BgGray = &H70
  BgDarkGray = &H80
  BgBlue = &H90
  BgGreen = &HA0
  BgCyan = &HB0
  BgRed = &HC0
  BgMagenta = &HD0
  BgYellow = &HE0
  BgWhite = &HF0
End Enum

Public Enum PixelType As Short
  Solid = &H2588
  ThreeQuarters = &H2593
  Half = &H2592
  Quarter = &H2591
End Enum

#End Region
