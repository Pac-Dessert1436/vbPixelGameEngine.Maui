# vbPixelGameEngine.Maui

This project is an endeavor to port the `vbPixelGameEngine` to .NET MAUI (a cross-platform framework for developing desktop and mobile applications) using VB.NET. It's now a work in progress.

The original [vbPixelGameEngine](https://github.com/DualBrain/vbPixelGameEngine), developed by [@DualBrain](https://github.com/DualBrain) since 2023, is an easy-to-use game engine written in VB.NET. It fully utilizes the Windows API for pixel-level drawing, and it supports OpenGL rendering.

Besides, the .NET MAUI class library is originally a C# template, but it can be easily converted to a VB.NET project using the "Code Converter (VB-C#)" extension in Visual Studio 2022.

## Modified Content

- Added shared functions to the `Vi2d` and `Vf2d` structures for distance calculation (straight-line distance, squared Euclidean distance, Manhattan distance, and Chebyshev distance), together with angle calculation. The result for angle calculation will be measured in radians.
  - Manhattan distance is also known as **taxicab distance**, so the original function signature `ManhDist` is now changed into `TaxiDist`.
  - Besides, floating-point implementations of the dot product and cross product have been added to the `Vf2d` structure as `DotF` and `CrossF` methods.
```vb
' The following code lies in the `Vf2d` structure; implementation in `Vi2d` is similar.
' These shared functions are exactly placed before the overloaded operators.
Public Shared Function Dist(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return (vec1 - vec2).Mag()  ' Straight-line distance
End Function

Public Shared Function Dist2(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return (vec1 - vec2).Mag2()  ' Squared Euclidean distance
End Function

Public Shared Function TaxiDist(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return MathF.Abs(vec1.x - vec2.x) + MathF.Abs(vec1.y - vec2.y)
End Function

Public Shared Function ChebDist(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return MathF.Max(MathF.Abs(vec1.x - vec2.x), MathF.Abs(vec1.y - vec2.y))
End Function

Public Shared Function Angle(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return MathF.Atan2(vec1.y - vec2.y, vec1.x - vec2.x)
End Function
```
- Introduced two rectangle structures (`RectI` for integer coordinates, `RectF` for floating-point coordinates) to enhance collision detection. These structures draw inspiration from the C# MonoGame API, with an additional `Center` property to simplify center-point calculations.
- Introduced a `SpriteSheet` class to support **sprite animation** and **tilemap creation**.
  - This class is now capable of supporting multiple in-game characters within the same `SpriteSheet`, and the default character name is provided as "default" (unless you specify the `noDefault` parameter as True in the constructor).
  ``` vb
  ' The major constructor of the `SpriteSheet` class.
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
    ' Add the default character name without specifying the `noDefault` parameter.
    If Not noDefault Then AddCharacterName("default")
  End Sub
  ```
  - The `SpriteSheet` class also enables tilemap creation in the form of `List(Of Sprite)`, which can be used to build 2D maps with multiple tiles, and more importantly, it supports the drawing method for animation frames based on the character name:
  ``` vb
  Public Sub DrawFrame(charaName As String, pos As Vf2d, Optional scale As Integer = 1)
    ' Note: The original singleton object "Pge" might be changed in the future,
    '       because of the migration from Desktop to MAUI.
    Pge.DrawSprite(pos, gameCharacters(charaName).CurrFrame, scale)
  End Sub
  ```
- Manually rewrote the "__Microsoft.Android.Resource.Designer.cs" file in the directory "./src/obj/Debug/net8.0-android" using VB.NET syntax, especially the moment the ".vbproj" file is updated, to suppress compilation errors. (Note: This ".cs" file isn't uploaded because of the ".gitignore" rule.)
```vb
' Source Path: ./src/obj/Debug/net8.0-android/__Microsoft.Android.Resource.Designer.cs
' This file has to be manually rewritten with VB.NET syntax every time the ".vbproj" file is 
' updated, despite the system warning against it.
Partial Public Class Resource
  Inherits _Microsoft.Android.Resource.Designer.Resource
End Class
```
- Renamed the `Randoms.vb` module to `GameMath.vb` and significantly expanded its core functionality to cover more mathematical operations for game development, with new additions including Minkowski distance, Jaccard similarity & distance, scalar utilities, angle helpers, vector/target movement tools, line-segment intersection detection, and Bezier/spline curve calculators.
- Migrated the vector-specific `Reflect()` and `Rotate()` methods (originally extension methods in `GameMath.vb`) to `Vi2d.vb` and `Vf2d.vb` respectively, aligning these core vector transformation operations with their dedicated type modules. Additionally, two new geometry methods for line segment projection have been added to both 2D vector structs:  
  - **Floating-point 2D vectors (`Vf2d`)**  
    - `LineSegProj(a As Vf2d, b As Vf2d) As Vf2d`: Computes the orthogonal projection of the current vector onto the line segment defined by endpoints `a` and `b`. The method first calculates the vector from `a` to `b` (`ab`), then normalizes the projection parameter `t` to the range `[0, 1]` (clamping values to ensure the result lies on the segment rather than the infinite line). If the segment has zero length (i.e., `a` and `b` are the same point), the method returns `a` directly.  
    - `DistToLineSeg(a As Vf2d, b As Vf2d) As Single`: Calculates the shortest Euclidean distance from the current vector to the target line segment `ab`. This method leverages `LineSegProj()` to find the closest point on the segment to the current vector, then computes the straight-line distance between the current vector and this projected point.
  - **Integer 2D vectors (`Vi2d`)**  
    - `LineSegProj(a As Vi2d, b As Vi2d) As Vi2d`: The integer-type counterpart to the `Vf2d` method, computing the orthogonal projection of the current integer vector onto the line segment defined by integer endpoints `a` and `b`, with parameter clamping to ensure the result falls within the segment bounds.  
    - `DistToLineSeg(a As Vi2d, b As Vi2d) As Integer`: Calculates the shortest integer Euclidean distance from the current integer vector to the target line segment `ab`, relying on the integer version of `LineSegProj()` to find the closest segment point.

## Latest Features in `GameMath.vb`

1. **Minkowski Distance**: A generalized distance metric that unifies common distance calculations, which automatically handles Manhattan distance (p=1), Euclidean distance (straight-line distance, p=2), and Chebyshev distance (p=Integer.MaxValue) as special cases, while supporting custom positive order parameters (p>0) for flexible distance measurements between 2D vectors.
2. **Jaccard Similarity & Distance**: Tailored for rectangle overlap analysis, with overloaded support for both floating-point (RectF) and integer (RectI) rectangles. Jaccard similarity quantifies the ratio of overlapping area to union area, while Jaccard distance is conveniently calculated as `1F - Jaccard(rectA, rectB)` (with dedicated `JaccardDist` methods for direct use).
3. **Basic Scalar Utilities**: Enhanced numerical control with functions like `ClampF` (clamp values to a range), `WrapF` (wrap values within a range), `LerpF` (linear interpolation), `CompareF` (precision-aware floating-point comparison), `RepeatF` (repeat values over a length), and `SmoothStep` (smooth interpolation for transitions/animations).
4. **Angle Helpers**: Radians-focused tools including `NormalizeAngle` (normalize angles to the [-π, π] range) and `DeltaAngle` (calculate the shortest angular difference between two radians values).
5. **Target Movement Extensions**: Seamless `MoveTowards` extension methods for smooth, max-step-limited movement, which supports scalar values (Single), 2D floating-point vectors (Vf2d), and 2D integer vectors (Vi2d), enabling natural movement toward targets without overshooting.
6. **Line-Segment Intersection**: Detects whether two 2D line segments (defined by Vf2d endpoints) intersect, and returns the exact intersection point as an output parameter for collision detection or geometry-based logic.
7. **Bezier/Spline Curve Tools**: Comprehensive curve calculation functions for smooth motion and shape generation, including Quadratic Bezier (3 control points), Cubic Bezier (4 control points), Catmull-Rom spline (4 control points with optional normalization), and Hermite spline (2 endpoints + 2 tangent vectors) - all optimized for 2D vector (Vf2d) inputs.

Additionally, the module retains and refines its original random number generation capabilities (e.g., `RandomByte`, `RandomBytes`, `RandomInt`, `RandomFloat`) with seed management via the `RandomSeed` property and `RefreshRandom` method, ensuring consistent and flexible randomness for game mechanics. All new functions are designed for seamless integration with existing workflows, with overloaded methods and extension-style implementations where applicable.

## Plans for This Project

- Port the Windows API calls to .NET MAUI, using P/Invoke or some other approaches.
- Migrate the OpenGL rendering code to `SkiaSharp` and `Microsoft.Maui.Graphics`.
- Adapt the input handling code to .NET MAUI event handlers.
- Make good use of the Inversion of Control (IoC) design pattern.
- Add more components to this engine (`BitmapFont`, `AudioPlayer` etc.).

## Current Progress

The Postgraduate Entrance Exam finally wrapped up on December 21, 2025. I only did quite well in English, and I'm not even sure if I'll pass the Politics section. Only a few college friends and I know how poorly I performed in Chinese Medicine Integration, since I had to rely entirely on guesswork to answer those TCM-related questions. Mixed feelings aside, I have a sinking suspicion that I'll have to retake the exam next year.

As for my `vbPixelGameEngine` porting project, I think its design is similar to MonoGame but with a much simpler API. Without a fully functional MAUI template for VB.NET, I'm torn on whether to keep the project going or put it on hold. *__What's more, if I do decide to retake the Postgraduate Entrance Exam, I'll have to suspend development entirely, and after all, I need to prioritize studying over coding to make sure I pass next time.__*

The porting task remains to be challenging in itself, and it's not a good start for my part. Despite the fact that the engine supports OpenGL rendering, the original code has a strong dependency on Windows API, giving me a painful experience when I'm trying to port the code. I'm just a hobbyist in game development, so my time to work on this project is very limited.

For now, I'm focusing on adding features that can be easily extended within this project. The Minkowski distance algorithm, which I first encountered in an extracurricular bioinformatics textbook during my undergraduate studies, specifically while researching in the university library before graduation, seemed like an excellent fit for implementing advanced distance calculations in game development.

In terms of cross-platform compatibility, I'm considering utilizing either the `SkiaSharp` library or the built-in `Microsoft.Maui.Graphics` namespace. However, my familiarity with MAUI programming is still limited, so I plan to sort out my ideas gradually and replace the Windows API-based drawing logic with MAUI-compatible implementations step by step.  

Beyond the updates to `GameMath.vb` (including the addition of Minkowski distance, Jaccard similarity, and renaming from `Randoms.vb`), the `Reflect()` and `Rotate()` methods for 2D vectors, which were originally implemented as extension methods in `GameMath.vb`, have now been fully migrated to `Vi2d.vb` and `Vf2d.vb` respectively, aligning vector-specific operations with their dedicated type modules.  

All in all, if you have any suggestions or guidance, please feel free to let me know. I'll update the progress on GitHub.

## License

This project is licensed under the MIT License. For more information, please refer to the [LICENSE](LICENSE) file included in this repository.
