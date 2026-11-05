# Changelog

- Fixes: FFmpeg conversions hanging indefinitely due to stdout pipe deadlock (thanks to HapppppyMoon).
- Fixes: Modern Microsoft 365 Word/PowerPoint conversion throwing DISP_E_TYPEMISMATCH (thanks to mzyqzmd).
- Fixes: Missing NetOffice dependencies in WiX installer causing Office conversions to fail on clean installs (thanks to mzyqzmd).
- Fixes: PDF to image conversion crash when probing page counts before Ghostscript initialization (thanks to 0xyo).
- Fixes: Loss of transparency when converting SVG to PNG/WebP/ICO and when encoding ICO files (thanks to Chimpanther).
- Fixes: Windows Explorer context menu failure when registry keys are missing via HKLM and local assembly fallback paths with auto self-healing (thanks to DavidMWu).
- Fixes: Version reporting consistency across application UI, CLI, shell extension DLL, and upgrade service feed.
- Fixes: NullReferenceException when removing root folders or creating folders/presets in Settings view model.
- Fixes: Potential memory leaks and process handle leaks across PowerPoint, Word, Excel, ImageMagick, and CDA conversion jobs.
- Tech: Updated bundled middleware: Ghostscript upgraded from 10.02.1 to 10.08.0 (fixing PDF rendering issues and security vulnerabilities).
- Tech: Modernized FFmpeg integration with single-image `-update 1` muxing for PNG, JPG, and GIF palette generation, clamped PNG compression level (0-9), and resilient stderr progress/duration regex parsing.
- Tech: Updated dependencies to latest versions: Magick.NET (15.0.0), CommunityToolkit.Mvvm (8.4.2), Microsoft.Extensions.DependencyInjection (10.0.13), Microsoft.Xaml.Behaviors.Wpf (1.1.161).
- Tech: Automated GitHub Actions CI workflow building x64 Release MSI installer and uploading test artifacts on push/PR.
