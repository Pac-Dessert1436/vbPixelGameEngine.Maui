"""
Converts the auto-generated C# `__Microsoft.Android.Resource.Designer.cs` file to VB.NET.

This Python script ensures the resource designer class is available in VB.NET, eliminating
build-time errors in MAUI projects that use VB.NET as the primary language. It overwrites
the C# file with a VB.NET partial class that inherits from the original resource type.
"""
from os import walk

TARGET_CSHARP_FILE = "__Microsoft.Android.Resource.Designer.cs"
NEW_CONTENT = """Partial Public Class Resource
	Inherits _Microsoft.Android.Resource.Designer.Resource
End Class"""

for root, dirs, files in walk("."):
    for file in files:
        if file == TARGET_CSHARP_FILE:
            with open(root + "/" + file, "w") as f:
                f.write(NEW_CONTENT)
