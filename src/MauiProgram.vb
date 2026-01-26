Imports Microsoft.Maui.Hosting
Imports Microsoft.Maui.Controls.Hosting

Public Module MauiProgram
  Public Sub CreateMauiApp(appBuilder As MauiAppBuilder)
    appBuilder.UseMauiApp(Of App).ConfigureFonts(
      Sub(fonts)
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular")
      End Sub)
  End Sub
End Module
