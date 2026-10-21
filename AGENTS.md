# AGENTS.md

Instructions and context for automated agents working on the FileConverter codebase.

## Repository Overview

FileConverter is a Windows shell context menu file conversion utility written in C# targeting .NET Framework 4.8 and WiX v3 toolset for installer packaging.

### Solution Structure
- `FileConverter.sln`: Main Visual Studio solution.
- `Application/FileConverter/`: Core WPF application, conversion engine, preset system, and settings UI.
- `Application/FileConverterExtension/`: Windows Explorer shell extension DLL registered into Windows context menus.
- `Installer/`: WiX 3.11 `.wixproj` and `Product.wxs` producing `FileConverter-{version}-x64.msi`.
- `Middleware/`: Third-party binaries (`ffmpeg/`, `gs/`) bundled with the application.
- `Resources/`: Icons, color schemes, and artwork.

## Environment & Build Guidelines

1. **Host Environment:**
   - WSL2 Linux (`Ubuntu 24.04`) environment mounted from `/mnt/c/Users/Aretzera/source/repos/FileConverter`.
   - Windows host has VS Code 2026 / Visual Studio and .NET SDK.
   - Code editing and git operations occur within WSL; builds and MSI packaging are performed on Windows via GitHub Actions (`.github/workflows/build.yml`) or Visual Studio / MSBuild.
2. **Line Endings:**
   - Git repository uses `core.autocrlf=true`. Ensure Windows CRLF line endings are preserved for XML/CS/SLN files.
3. **Artifacts & Release Policy:**
   - All builds on push/PR upload build artifacts (`FileConverter-x64-Setup-{hash}.msi` and Application binaries) to GitHub Actions for independent verification.
   - **Versioning & Publishing:** Halted per user directive until explicit human confirmation. Do not create release tags or publish to GitHub Releases automatically without operator instruction.

## Branch and Fork Knowledge

Refer to `BRANCHES.md` for in-depth fork and branch analysis.
- Working branch: `integration` (incorporates latest bugfixes and community hardening).
- Key community fixes merged in this fork:
  - FFmpeg stdout pipe deadlock fix (PR #732).
  - Modern Office M365 `DISP_E_TYPEMISMATCH` fix & NetOffice installer bundling (PR #779).
  - PDF to image Ghostscript initialization ordering fix (PR #806).
  - SVG and ICO alpha transparency preservation (PR #808).
  - Robust shell extension registry/fallback paths (PR #804).
  - Multi-threaded COM and process disposal hardening.
