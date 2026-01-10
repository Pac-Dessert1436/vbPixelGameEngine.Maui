# vbPixelGameEngine.Maui Technical Documentation

## Overview

This document provides detailed technical information about the vbPixelGameEngine.Maui project, a cross-platform game engine written in VB.NET targeting .NET MAUI. It covers the latest features, core components, and implementation details.

## Vector Mathematics

### 2D Vector Structures

The engine provides two primary vector structures for 2D game development:
- `Vi2d` - Integer-based 2D vector for precise positional calculations
- `Vf2d` - Floating-point 2D vector for smooth animations and physics

### Distance Calculation Methods

Both vector structures include comprehensive distance calculation methods:

```vb
' Distance functions in Vf2d structure (similar implementation in Vi2d)
Public Shared Function Dist(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return (vec1 - vec2).Mag()  ' Straight-line (Euclidean) distance
End Function

Public Shared Function Dist2(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return (vec1 - vec2).Mag2()  ' Squared Euclidean distance (optimized for comparisons)
End Function

Public Shared Function TaxiDist(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return MathF.Abs(vec1.x - vec2.x) + MathF.Abs(vec1.y - vec2.y)  ' Manhattan/taxicab distance
End Function

Public Shared Function ChebDist(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return MathF.Max(MathF.Abs(vec1.x - vec2.x), MathF.Abs(vec1.y - vec2.y))  ' Chebyshev distance
End Function
```

### Angle Calculations

```vb
Public Shared Function Angle(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return MathF.Atan2(vec1.y - vec2.y, vec1.x - vec2.x)  ' Returns angle in radians
End Function
```

### Vector Transformations

#### Projection and Distance to Line Segments

```vb
' Vf2d implementation - Vi2d has similar functionality
Public Function LineSegProj(a As Vf2d, b As Vf2d) As Vf2d
  Dim ab As Vf2d = b - a
  Dim ap As Vf2d = Me - a
  Dim t As Single = Vf2d.Dot(ap, ab) / Vf2d.Dot(ab, ab)
  t = MathF.Clamp(t, 0.0F, 1.0F)  ' Ensure result lies on the segment
  Return a + ab * t
End Function

Public Function DistToLineSeg(a As Vf2d, b As Vf2d) As Single
  Dim proj As Vf2d = LineSegProj(a, b)
  Return Vf2d.Dist(Me, proj)
End Function
```

## Geometry System

### Rectangle Structures

Two rectangle structures have been introduced for collision detection and spatial management:
- `RectI` - Integer-based rectangle for tile maps and pixel-perfect collisions
- `RectF` - Floating-point rectangle for smooth physics and UI layout

Both structures include:
- `Center` property for easy center-point calculations
- Overloaded operators for common operations
- Jaccard similarity/distance metrics for overlap analysis

## Sprite Management

### SpriteSheet Class

The `SpriteSheet` class provides comprehensive support for sprite animation and tilemap creation:

```vb
' Main constructor
Public Sub New(sprite As Sprite, frameScale As Vi2d, Optional noDefault As Boolean = False)
  sheet = sprite
  frameW = frameScale.x
  frameH = frameScale.y

  Columns = sheet.Width \ frameW
  Rows = sheet.Height \ frameH

  ReDim AllFrameIndices(Rows * Columns - 1)
  Dim i As Integer = 0
  For row As Integer = 0 To Rows - 1
    For col As Integer = 0 To Columns - 1
      AllFrameIndices(i) = (row, col)
      i += 1
    Next col
  Next row
  
  ' Add default character unless specified otherwise
  If Not noDefault Then AddCharacterName("default")
End Sub
```

### Key Features

- **Multi-Character Support**: Manage multiple animated characters within a single sprite sheet
- **Frame Animation**: Draw specific animation frames by character name
- **Tile Map Creation**: Generate tile maps as `List(Of Sprite)` for 2D level design
- **Scalable Rendering**: Support for scaling sprites during rendering

```vb
' Draw a specific animation frame
Public Sub DrawFrame(charaName As String, pos As Vf2d, Optional scale As Integer = 1)
  Pge.DrawSprite(pos, gameCharacters(charaName).CurrFrame, scale)
End Sub
```

## Mathematical Library (GameMath.vb)

Formerly known as `Randoms.vb`, the `GameMath.vb` module has been significantly expanded to provide comprehensive mathematical utilities for game development.

### Core Features

#### 1. Advanced Distance Metrics

```vb
' Minkowski distance - generalizes Manhattan, Euclidean, and Chebyshev distances
Public Shared Function Minkowski(vec1 As Vf2d, vec2 As Vf2d, p As Single) As Single
  If p = Single.MaxValue Then
    Return ChebDist(vec1, vec2)
  ElseIf p = 1.0F Then
    Return TaxiDist(vec1, vec2)
  ElseIf p = 2.0F Then
    Return Dist(vec1, vec2)
  Else
    Dim dx As Single = MathF.Abs(vec1.x - vec2.x)
    Dim dy As Single = MathF.Abs(vec1.y - vec2.y)
    Return MathF.Pow(MathF.Pow(dx, p) + MathF.Pow(dy, p), 1.0F / p)
  End If
End Function
```

#### 2. Jaccard Similarity & Distance

```vb
' Jaccard similarity for rectangle overlap analysis
Public Shared Function Jaccard(rect1 As RectF, rect2 As RectF) As Single
  Dim intersection As Single = IntersectionArea(rect1, rect2)
  Dim union As Single = rect1.Area + rect2.Area - intersection
  Return If(union = 0.0F, 0.0F, intersection / union)
End Function

' Jaccard distance
Public Shared Function JaccardDist(rect1 As RectF, rect2 As RectF) As Single
  Return 1.0F - Jaccard(rect1, rect2)
End Function
```

#### 3. Scalar Utilities

```vb
' Clamp a value to a range
Public Shared Function ClampF(value As Single, min As Single, max As Single) As Single
  Return MathF.Max(min, MathF.Min(max, value))
End Function

' Linear interpolation
Public Shared Function LerpF(start As Single, [end] As Single, t As Single) As Single
  Return start + t * ([end] - start)
End Function

' Smooth step interpolation for smooth transitions
Public Shared Function SmoothStep(edge0 As Single, edge1 As Single, x As Single) As Single
  x = ClampF((x - edge0) / (edge1 - edge0), 0.0F, 1.0F)
  Return x * x * (3.0F - 2.0F * x)
End Function
```

#### 4. Angle Helpers

```vb
' Normalize angle to [-π, π] range
Public Shared Function NormalizeAngle(angle As Single) As Single
  angle = angle Mod (2.0F * MathF.PI)
  If angle < -MathF.PI Then
    angle += 2.0F * MathF.PI
  ElseIf angle > MathF.PI Then
    angle -= 2.0F * MathF.PI
  End If
  Return angle
End Function

' Calculate shortest angular difference
Public Shared Function DeltaAngle(current As Single, target As Single) As Single
  Return NormalizeAngle(target - current)
End Function
```

#### 5. Movement Utilities

```vb
' Move towards a target with max step limit
Public Shared Function MoveTowards(current As Single, target As Single, maxStep As Single) As Single
  If MathF.Abs(target - current) <= maxStep Then
    Return target
  End If
  Return current + MathF.Sign(target - current) * maxStep
End Function

' Vector version (extension method)
<Extension>
Public Shared Function MoveTowards(current As Vf2d, target As Vf2d, maxStep As Single) As Vf2d
  Dim direction As Vf2d = target - current
  Dim distance As Single = direction.Mag()
  If distance <= maxStep OrElse distance < Single.Epsilon Then
    Return target
  End If
  Return current + direction.Normalized() * maxStep
End Function
```

#### 6. Geometry Algorithms

```vb
' Line segment intersection detection
Public Shared Function LineSegIntersect(
    a1 As Vf2d, a2 As Vf2d, b1 As Vf2d, b2 As Vf2d, 
    ByRef intersection As Vf2d) As Boolean
  ' Implementation omitted for brevity
  ' Returns True if segments intersect, with intersection point in output parameter
End Function
```

#### 7. Curve Calculators

```vb
' Quadratic Bezier curve
Public Shared Function BezierQuad(p0 As Vf2d, p1 As Vf2d, p2 As Vf2d, t As Single) As Vf2d
  Dim u As Single = 1.0F - t
  Return u * u * p0 + 2.0F * u * t * p1 + t * t * p2
End Function

' Cubic Bezier curve
Public Shared Function BezierCubic(p0 As Vf2d, p1 As Vf2d, p2 As Vf2d, p3 As Vf2d, t As Single) As Vf2d
  Dim u As Single = 1.0F - t
  Return u * u * u * p0 + 3.0F * u * u * t * p1 + 3.0F * u * t * t * p2 + t * t * t * p3
End Function
```

## Platform-Specific Implementation Notes

### Android Resource Handling

When working with Android, a special VB.NET wrapper must be created for the Android resource designer:

```vb
' Source: ./src/obj/Debug/net8.0-android/__Microsoft.Android.Resource.Designer.cs
' This file needs to be rewritten in VB.NET syntax after each .vbproj update
Partial Public Class Resource
  Inherits _Microsoft.Android.Resource.Designer.Resource
End Class
```

## Random Number Generation

The `GameMath.vb` module retains its random number generation capabilities with enhanced seed management:

```vb
' Random seed property
Public Shared Property RandomSeed As Integer
  Get
    Return m_seed
  End Get
  Set(value As Integer)
    m_seed = value
    RefreshRandom()
  End Set
End Property

' Generate random values
Public Shared Function RandomInt(minValue As Integer, maxValue As Integer) As Integer
  Return s_random.Next(minValue, maxValue + 1)
End Function

Public Shared Function RandomFloat(minValue As Single, maxValue As Single) As Single
  Return minValue + CSng(s_random.NextDouble()) * (maxValue - minValue)
End Function
```

## Architecture Overview

The engine follows a modular architecture:

| Component | Responsibility |
|-----------|----------------|
| `PixelGameEngine.vb` | Core game loop, rendering abstraction, and input management |
| `GameMath.vb` | Comprehensive mathematical utilities for game development |
| `Vi2d.vb`/`Vf2d.vb` | Vector mathematics and geometric operations |
| `RectI.vb`/`RectF.vb` | Rectangle structures for collision detection |
| `Sprite.vb`/`SpriteSheet.vb` | Sprite and animation management |
| `SkiaSharpRenderer.vb` | Cross-platform rendering implementation |

## Performance Considerations

- **Distance Calculations**: Use `Dist2` (squared distance) for comparisons to avoid expensive square root operations
- **Vector Operations**: Prefer integer vectors (`Vi2d`) when precision isn't critical for better performance
- **Memory Management**: Dispose of unused resources properly, especially with large sprite sheets
- **Rendering**: Batch similar drawing operations for better performance

## Future Enhancements

Planned features for upcoming releases:

1. **Bitmap Font System**: Custom bitmap font rendering with support for multiple character sets
2. **Audio Integration**: Cross-platform audio playback capabilities
3. **Physics Engine**: 2D physics simulation with collision response
4. **Particle System**: Efficient particle effects for visual enhancements
5. **Networking**: Multiplayer support using MAUI-compatible networking libraries

## Usage Examples

### Basic Game Implementation

```vb
Public Class MyGame
    Inherits PixelGameEngine

    Public Sub New()
        Title = "My MAUI Game"
    End Sub

    Public Overrides Function OnUserCreate() As Boolean
        ' Initialize game resources
        Return True
    End Function

    Public Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
        ' Update game logic
        Return True
    End Function

    Public Overrides Function OnUserRender(elapsedTime As Single) As Boolean
        ' Render game graphics
        Return True
    End Function
End Class
```

### Sprite Animation Example

```vb
' Load a sprite sheet
Dim playerSpriteSheet As New SpriteSheet(
    sprite:=LoadSprite("player_sprites.png"),
    frameScale:=New Vi2d(32, 32)
)

' Add animation frames for walking
playerSpriteSheet.AddCharacterName("player")
playerSpriteSheet.DefineAnimation("player", (0, 0), (2, 0))

' In game loop
Public Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    ' Update animation
    playerSpriteSheet.UpdateCharacterFrame("player", elapsedTime)
    Return True
End Function

Public Overrides Function OnUserRender(elapsedTime As Single) As Boolean
    ' Draw current frame
    playerSpriteSheet.DrawFrame("player", New Vf2d(100, 100))
    Return True
End Function
```