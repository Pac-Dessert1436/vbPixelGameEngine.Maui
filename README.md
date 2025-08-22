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
- Latest Update: Renamed the `Randoms.vb` module to `GameMath.vb`, and added new core functions including Minkowski distance, vector rotation, and vector reflection.
  - The Minkowski distance serves as a generalization of common distance metrics: Manhattan distance, Euclidean distance (i.e., straight-line distance), and Chebyshev distance.
  - Vector rotation and vector reflection are implemented as extension methods for seamless integration with existing vector operations.
```vb
' In `GameMath.vb`, the Minkowski distance is defined as follows (not an extension method).
Public Function MinkoDist(vec1 As Vf2d, vec2 As Vf2d, p As Integer) As Single
  If p <= 0 Then Throw New ArgumentException(
    "Minkowski distance requires a positive order parameter (p > 0).", NameOf(p))

  Dim absDiffX = MathF.Abs(vec1.x - vec2.x)
  Dim absDiffY = MathF.Abs(vec1.y - vec2.y)
  ' Handle special cases for p=1 (Manhattan), p=2 (Euclidean) and p->inf (Chebyshev).
  Select Case p
    Case 1
      Return absDiffX + absDiffY
    Case 2
      Return MathF.Sqrt(absDiffX * absDiffX + absDiffY * absDiffY)
    Case Integer.MaxValue
      Return MathF.Max(absDiffX, absDiffY)
    Case Else  ' General formula for Minkowski distance
      Return CSng((absDiffX ^ p + absDiffY ^ p) ^ (1 / p))
  End Select
End Function
```

## Plans for This Project

- Port the Windows API calls to .NET MAUI, using P/Invoke or some other approaches.
- Migrate the OpenGL rendering code to `SkiaSharp` and `Microsoft.Maui.Graphics`.
- Adapt the input handling code to .NET MAUI event handlers.
- Make good use of the Inversion of Control (IoC) design pattern.
- Add more components to this engine (`BitmapFont`, `AudioPlayer` etc.).

## Current Progress

I'm currently working on this porting project all by myself. *__However, I'm now busy retaking the Postgrauate Entrance Exam in my home city, probably about Biochemistry, General Biology, or TCM-related subjects, and I will have to set the project aside until the end of this year.__* I will definitely try my best to finish the project after that, but not these five or six months.

The porting task is quite challenging, and it's not a good start for my part. Despite the fact that the engine supports OpenGL rendering, the original code has a strong dependency on Windows API, giving me a painful experience when I'm trying to port the code. I'm just a hobbyist in game development, so my time to work on this project is very limited.

For now, I'm focusing on adding features that can be easily extended within this project. The Minkowski distance algorithm, which I first encountered in an extracurricular bioinformatics textbook during my undergraduate studies, specifically while researching in the university library before graduation, seemed like an excellent fit for implementing advanced distance calculations in game development.

In terms of cross-platform compatibility, I'm considering utilizing either the `SkiaSharp` library or the built-in `Microsoft.Maui.Graphics` namespace. I will have to take this plan with a grain of salt, and after all, I will have to set the project aside until the end of this year since the Postgraduate Entrance Exam is drawing near.

If you have any suggestions or guidance, please feel free to let me know. I'll update the progress on GitHub.

## License

This project is licensed under the MIT License. For more information, please refer to the [LICENSE](LICENSE) file included in this repository.
