# Changelog

- Fixes: FFmpeg conversions hanging indefinitely due to stdout pipe deadlock (thanks to HapppppyMoon).
- Fixes: Modern Microsoft 365 Word/PowerPoint conversion throwing DISP_E_TYPEMISMATCH (thanks to mzyqzmd).
- Fixes: Missing NetOffice dependencies in WiX installer causing Office conversions to fail on clean installs (thanks to mzyqzmd).
- Fixes: PDF to image conversion crash when probing page counts before Ghostscript initialization (thanks to 0xyo).
- Fixes: Loss of transparency when converting SVG to PNG/WebP/ICO and when encoding ICO files (thanks to Chimpanther).
- Fixes: Windows Explorer context menu failure when registry keys are missing via HKLM and local assembly fallback paths with auto self-healing (thanks to DavidMWu).
- Fixes: NullReferenceException when removing root folders or creating folders/presets in Settings view model.
- Fixes: Potential memory leaks and process handle leaks across PowerPoint, Word, Excel, ImageMagick, and CDA conversion jobs.
- Tech: Updated dependencies to latest versions: Magick.NET (15.0.0), CommunityToolkit.Mvvm (8.4.2), Microsoft.Extensions.DependencyInjection (10.0.13), Microsoft.Xaml.Behaviors.Wpf (1.1.161).
- Tech: Automated GitHub Actions CI workflow building x64 Release MSI installer and uploading test artifacts on push/PR.
