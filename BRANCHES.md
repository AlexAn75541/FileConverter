# Branches and Forks Analysis

This document catalogs remote branches, upstream pull requests, and forks of FileConverter, noting their useful features, bug fixes, and integration status.

---

## 1. Local & Origin Branches (`AlexAn75541/FileConverter`)

| Branch | Base / Target | Purpose & State |
|---|---|---|
| `integration` | Default branch | Contains latest development changes, community PR merges (e.g. #807 image rotation fix, #699 UpgradeService null-safety fix). Active working branch for v2.2.x hardening. |
| `master` | Stable release | Points to v2.2.0 release (`fdaae52`). Merges occur from `integration` when a milestone is ready for release. |
| `release/2.0` | Maintenance | Historical release branch for v2.0 release line. |
| `gh-pages` | Documentation | Static website and download portal for FileConverter. |

---

## 2. Key Upstream Pull Requests (`Tichau/FileConverter`)

Evaluated community pull requests on upstream:

### Integrated in this working branch:
1. **PR #732 (`HapppppyMoon:fix/stdout-pipe-deadlock`) - FFmpeg Stdout Pipe Deadlock Fix**
   - **Issue:** FFmpeg conversions hung indefinitely on larger files.
   - **Root Cause:** `-progress pipe:1` wrote progress records to standard output, but the application only read `StandardError`. Once the OS pipe buffer (~4-64 KB) filled, FFmpeg blocked on stdout write while FileConverter blocked on stderr read.
   - **Solution:** Removed `-progress pipe:1`, set `RedirectStandardOutput = false`. Progress continues to be parsed reliably from stderr via existing regex.

2. **PR #779 (`mzyqzmd:integration`) - Modern Office (M365) & Installer NetOffice Bundling**
   - **Issue 1 (Clean install):** v2.2 migrated Office interop to NetOffice, but `Installer/Product.wxs` did not bundle the NetOffice assemblies (`NetOffice.dll`, `OfficeApi.dll`, `WordApi.dll`, `ExcelApi.dll`, `PowerPointApi.dll`, `VBIDEApi.dll`), causing `FileNotFoundException` / `TypeLoadException` on clean installations.
   - **Issue 2 (Runtime DISP_E_TYPEMISMATCH):** On recent Microsoft 365 builds, late-bound `ExportAsFixedFormat` throws `DISP_E_TYPEMISMATCH (0x80020005)`.
   - **Solution:** Added missing NetOffice assemblies to `Installer/Product.wxs`. Replaced `ExportAsFixedFormat` with `document.SaveAs(path, WdSaveFormat.wdFormatPDF)` in Word and `document.SaveAs(path, PpSaveAsFileType.ppSaveAsPDF, msoFalse)` in PowerPoint. Added `DisplayAlerts = wdAlertsNone` to suppress modal prompts in automated conversions.

3. **PR #806 (`0xyo:fix/790-pdf-to-image-crash`) - PDF to Image Ghostscript Initialization Fix**
   - **Issue:** PDF to image conversions crashed with `rangecheck in setscreen` (issue #790).
   - **Root Cause:** `ConversionJob.PrepareConversion()` calls `GetOutputFilesCount()` before `Initialize()`. In `ConversionJob_ImageMagick`, Ghostscript directory setup was only done in `Initialize()`, so page-count probing read PDF without Ghostscript configured.
   - **Solution:** Configured Ghostscript directory in `GetOutputFilesCount()` as well via shared `SetupGhostscriptDirectory()` helper.

4. **PR #808 (`Chimpanther:fix/svg-alpha-transparency`) - SVG & ICO Alpha Channel Transparency**
   - **Issue:** Converting SVG to PNG, WebP, or ICO lost transparency and filled with black or white. Converting images to ICO also lost alpha.
   - **Solution:** Set transparent background for SVG inputs unless converting to JPG. Added `-pix_fmt bgra` to FFmpeg arguments when encoding ICO.

5. **PR #804 (`DavidMWu:feature/animated-webp-support`) - Build Robustness & Shell Extension Resilience**
   - **Extracted:** Robust registry fallback (`HKCU` -> `HKLM` -> local assembly path) in `FileConverterExtension.PathHelpers` to prevent Explorer crashes. Quoted robocopy target paths in `FileConverter.csproj` to support paths with whitespace.
   - **Deferred:** Animated WebP presets and UI templates (can be added in a future feature release).

### Noted for future evaluation:
- **PR #702 (`bharatvansh:feature/auto-retry-software-encoding`)**: Auto-retry conversion with CPU software encoding (`libx264`) if hardware acceleration (NVENC / QSV / AMF) fails mid-conversion.
- **PR #775 (`huseyincancalti:feature/bmp-tiff-output`)**: Native BMP and TIFF image output formats.
- **PR #776 / #802**: Markdown (.md) to PDF/Docx conversion using pandoc/markdig.
- **PR #761 / #707 / #712 / #763 / #698**: Community localization updates (Khmer, Catalan, Italian, Chinese).

---

## 3. Forks Ecosystem

1. **`masterofobzene/FileConverter-H265`**
   - **Status:** Outdated (based on v1.2, 2022).
   - **Notes:** Committed pre-built binaries into git and hardcoded `hevc_nvenc` without fallback. Not recommended for merging.
2. **`Th3Nekit/FileConverter`**
   - **Status:** Inactive since 2022, 11 commits behind v2.2.
   - **Notes:** Focused on HEVC presets.
3. **`ZaidNAlAsali/ZFileConverter`**
   - **Status:** Active fork with smart output naming templates (`{orig_name}_{preset}_{date}`) and enhanced settings UX.
   - **Notes:** Good reference for custom output naming features.
4. **`Karthikprasad020/FileConverter-Lume`**
   - **Status:** Modernized UI revamp experiment targeting .NET 8 / modern WPF.
   - **Notes:** High-effort re-architecture; useful reference for future .NET 8 / WinUI migration.
