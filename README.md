# `vbPixelGameEngine.Maui` - A VB.NET Pixel Game Engine Targeting .NET MAUI

## Description

`vbPixelGameEngine.Maui` is a cross-platform pixel game engine written in VB.NET, ported from the original [vbPixelGameEngine](https://github.com/DualBrain/vbPixelGameEngine) developed by [@DualBrain](https://github.com/DualBrain). This port aims to make full use of .NET MAUI to create a unified, cross-platform game development framework that runs on Windows, Android, iOS, and MacCatalyst.

- **Current Framework**: .NET 9.0 with MAUI
- **Rendering**: Transitioning from platform-specific OpenGL to SkiaSharp
- **Input Handling**: Adapted for MAUI event system
- **API Design**: Simplified, MonoGame-inspired architecture with VB.NET-friendly syntax

The project started as an experiment to create a VB.NET-compatible game engine template for MAUI, filling a gap in the ecosystem while providing an accessible entry point for VB.NET developers into cross-platform game development.

## Key Features & Progress

### ✅ Completed
- **Platform Independence**: Removed all Windows API and Linux system calls, achieving true cross-platform compatibility
- **Vector Geometry**: Enhanced 2D vector structures (`Vi2d`, `Vf2d`) with comprehensive distance, angle, and transformation methods
- **Collision Detection**: Introduced `RectI` and `RectF` rectangle structures with Jaccard similarity/distance metrics
- **Sprite System**: Implemented `SpriteSheet` class for sprite animation and tilemap creation
- **Math Utilities**: Expanded `GameMath.vb` (formerly `Randoms.vb`) with advanced game development utilities

### 🔄 In Progress
- **Rendering Migration**: Transitioning from platform-specific OpenGL to SkiaSharp
- **Input System**: Adapting pixel-perfect input handling to MAUI's event system
- **Documentation**: Creating comprehensive guides and API reference

### 📋 Planned Features
- **Bitmap Font Support**: Custom bitmap font rendering system
- **Audio Integration**: Cross-platform audio playback capabilities
- **Component Architecture**: Modular system for easy feature extension
- **Example Projects**: Mini-games demonstrating engine capabilities

## Technical Architecture

The engine follows a clean, modular design:
- **Engine Core**: `PixelGameEngine.vb` - Main game loop and rendering abstraction
- **Math Module**: `GameMath.vb` - Comprehensive mathematical utilities for game development
- **Graphics**: `Sprite.vb`, `SpriteSheet.vb` - Sprite and animation management
- **Rendering**: `SkiaSharpRenderer.vb` - Cross-platform rendering implementation
- **Input**: MAUI event handlers integrated with engine input state

## Getting Started

### Prerequisites
- [Visual Studio 2022/2026](https://visualstudio.microsoft.com/vs/) with .NET MAUI workload
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Android SDK (for Android deployment)
- Optional: iOS/MacCatalyst development environment (requires macOS)

### Installation
1. Clone the repository and navigate to the project directory:
   ```
   git clone https://github.com/Pac-Dessert1436/vbPixelGameEngine.Maui.git
   cd vbPixelGameEngine.Maui
   ```
2. Open the solution in Visual Studio 2022/2026.
3. Restore project dependencies using the NuGet Package Manager in Visual Studio.
4. Build the project for your target platform.
5. Alternatively, if you are using Visual Studio Code:
   - Install the .NET MAUI workload via the `dotnet workload install maui` command.
   - Open the project folder in Visual Studio Code.
   - Restore NuGet packages using `dotnet restore` in the terminal.
   - Build the project with the `dotnet build` command.

### Development Status

The engine has reached a significant milestone with the successful removal of all platform-specific dependencies. The core architecture is now platform-agnostic, with rendering and input systems being abstracted through MAUI-compatible interfaces. 

While still in active development, the engine is functional and ready for experimentation. Key focus areas include completing the SkiaSharp rendering integration and refining the input handling system to provide a seamless pixel-perfect experience across all platforms.

## Contributing

Contributions are welcome! Whether you're fixing bugs, improving documentation, or adding new features, your help is valuable. Please feel free to:
- Submit bug reports
- Suggest new features
- Contribute code improvements
- Help with documentation

## Project Vision

`vbPixelGameEngine.Maui` aims to provide VB.NET developers with an accessible, powerful game engine that leverages modern cross-platform technologies. By combining the simplicity of the original vbPixelGameEngine with the versatility of .NET MAUI, this project seeks to create a unique development experience that bridges the gap between traditional VB.NET development and modern game creation.

## License

This project is licensed under the MIT License. For more information, please refer to the [LICENSE](LICENSE) file.