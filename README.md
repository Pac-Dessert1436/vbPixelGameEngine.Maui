# vbPixelGameEngine.Maui

This project is an endeavor to port the `vbPixelGameEngine` to .NET MAUI (a cross-platform framework for developing desktop and mobile applications) using VB.NET. It's now a work in progress.

The original [vbPixelGameEngine](https://github.com/DualBrain/vbPixelGameEngine), developed by [@DualBrain](https://github.com/DualBrain) since 2023, is an easy-to-use game engine written in VB.NET. It fully utilizes the Windows API for pixel-level drawing, and it supports OpenGL rendering.

Besides, the .NET MAUI class library is originally a C# template, but it can be easily converted to a VB.NET project using the "Code Converter (VB-C#)" extension in Visual Studio 2022.

## Modified Content

- Added shared functions to the `Vi2d` and `Vf2d` structures for distance calculation (linear distance, squared distance, and Manhattan distance), together with angle calculation. The result for angle calculation will be measured in radians.
```vb
' The following code lies in the `Vf2d` structure; implementation in `Vi2d` is similar.
' These shared functions are exactly placed before the overloaded operators.
Public Shared Function Dist(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return (vec1 - vec2).Mag()
End Function

Public Shared Function Dist2(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return (vec1 - vec2).Mag2()
End Function

Public Shared Function ManhDist(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return MathF.Abs(vec1.x - vec2.x) + MathF.Abs(vec1.y - vec2.y)
End Function

Public Shared Function Angle(vec1 As Vf2d, vec2 As Vf2d) As Single
  Return MathF.Atan2(vec1.y - vec2.y, vec1.x - vec2.x)
End Function
```
- Introduced two rectangle structures (`RectI` and `RectF`) for better collision detection. This essential feature is inspired by C# MonoGame API.
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
    If Not noDefault Then AddCharacter("default")
  End Sub
  ```
  - The `SpriteSheet` class also enables tilemap creation in the form of `List(Of Sprite)`, which can be used to build 2D maps with multiple tiles, and more importantly, it supports the drawing method for animation frames based on the character name:
  ``` vb
  Public Sub DrawFrame(charaName As String, pos As Vf2d, Optional scale As Integer = 1)
    ' Note: The original singleton object "Pge" might be changed in the future,
    '       because of the migration from Desktop to MAUI.
    Pge.DrawSprite(pos, animations(charaName).CurrFrame, scale)
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

## Plans for This Project

- Port the Windows API calls to .NET MAUI, using P/Invoke or some other approaches.
- Migrate the OpenGL rendering code to SkiaSharp.
- Adapt the input handling code to .NET MAUI event handlers.

## Current Progress

I'm currently working on this porting project all by myself. *__However, I'm now busy retaking the Postgrauate Entrance Exam in my home city, probably about Biochemistry, General Biology, or TCM-related subjects, and I will have to set the project aside until the end of this year.__* I will definitely try my best to finish the project after that, but not these five or six months.

The porting task is quite challenging, and it's not a good start for my part. Despite the fact that the engine supports OpenGL rendering, the original code has a strong dependency on Windows API, giving me a painful experience when I'm trying to port the code. I'm just a hobbyist in game development, so my time to work on this project is very limited.

For now I'm just adding the features which are easy to extend in this project. In order to achieve cross-platform compatibility, I'm thinking about using the "SkiaSharp" library or the built-in "Microsoft.Maui.Graphics" namespace, but I will have to take this plan with a grain of salt.

If you have any suggestions or guidance, please feel free to let me know. I'll update the progress on GitHub.

## License

This project is licensed under the MIT License. For more information, please refer to the [LICENSE](LICENSE) file included in this repository.
