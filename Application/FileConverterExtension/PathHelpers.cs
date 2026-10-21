// <copyright file="FileConverterExtension.cs" company="AAllard">License: http://www.gnu.org/licenses/gpl.html GPL version 3.</copyright>

namespace FileConverterExtension
{
    using System.IO;
    using System;
    using Microsoft.Win32;

    public static class PathHelpers
    {
        private static RegistryKey fileConverterRegistryKey;
        private static string fileConverterPath;

        public static string UserSettingsFilePath => Path.Combine(PathHelpers.GetUserDataFolderPath, "Settings.user.xml");

        public static string DefaultSettingsFilePath
        {
            get
            {
                string pathToFileConverterExecutable = PathHelpers.FileConverterPath;
                if (string.IsNullOrEmpty(pathToFileConverterExecutable))
                {
                    return null;
                }

                return Path.Combine(Path.GetDirectoryName(pathToFileConverterExecutable), "Settings.default.xml");
            }
        }

        public static RegistryKey FileConverterRegistryKey
        {
            get
            {
                if (PathHelpers.fileConverterRegistryKey == null)
                {
                    try
                    {
                        PathHelpers.fileConverterRegistryKey = Registry.CurrentUser.OpenSubKey(@"Software\FileConverter")
                            ?? Registry.LocalMachine.OpenSubKey(@"Software\FileConverter");
                    }
                    catch
                    {
                        PathHelpers.fileConverterRegistryKey = null;
                    }
                }

                return PathHelpers.fileConverterRegistryKey;
            }
        }

        public static string FileConverterPath
        {
            get
            {
                if (string.IsNullOrEmpty(PathHelpers.fileConverterPath) || !File.Exists(PathHelpers.fileConverterPath))
                {
                    PathHelpers.fileConverterPath = LocateFileConverterExecutable();
                }

                return PathHelpers.fileConverterPath;
            }
        }

        private static string LocateFileConverterExecutable()
        {
            // 1. Registry: HKCU and HKLM in 64-bit and 32-bit views
            string pathFromRegistry = GetPathFromRegistry();
            if (!string.IsNullOrEmpty(pathFromRegistry) && File.Exists(pathFromRegistry))
            {
                return pathFromRegistry;
            }

            // 2. Assembly CodeBase (works even when running from GAC)
            try
            {
                string codeBase = typeof(PathHelpers).Assembly.CodeBase;
                if (!string.IsNullOrEmpty(codeBase))
                {
                    Uri uri = new Uri(codeBase);
                    string localDir = Path.GetDirectoryName(uri.LocalPath);
                    string localExe = Path.Combine(localDir, "FileConverter.exe");
                    if (File.Exists(localExe))
                    {
                        TrySavePathToRegistry(localExe);
                        return localExe;
                    }
                }
            }
            catch
            {
            }

            // 3. Assembly Location (when running side-by-side)
            try
            {
                string location = typeof(PathHelpers).Assembly.Location;
                if (!string.IsNullOrEmpty(location))
                {
                    string localDir = Path.GetDirectoryName(location);
                    string localExe = Path.Combine(localDir, "FileConverter.exe");
                    if (File.Exists(localExe))
                    {
                        TrySavePathToRegistry(localExe);
                        return localExe;
                    }
                }
            }
            catch
            {
            }

            // 4. AppDomain BaseDirectory
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                if (!string.IsNullOrEmpty(baseDir))
                {
                    string localExe = Path.Combine(baseDir, "FileConverter.exe");
                    if (File.Exists(localExe))
                    {
                        TrySavePathToRegistry(localExe);
                        return localExe;
                    }
                }
            }
            catch
            {
            }

            // 5. COM InprocServer32 CodeBase registration
            try
            {
                using (RegistryKey clsidKey = Registry.ClassesRoot.OpenSubKey(@"CLSID\{AF9B72B5-F4E4-44B0-A3D9-B55B748EFE90}\InprocServer32"))
                {
                    string comCodeBase = clsidKey?.GetValue("CodeBase") as string;
                    if (!string.IsNullOrEmpty(comCodeBase))
                    {
                        Uri uri = new Uri(comCodeBase);
                        string localDir = Path.GetDirectoryName(uri.LocalPath);
                        string localExe = Path.Combine(localDir, "FileConverter.exe");
                        if (File.Exists(localExe))
                        {
                            TrySavePathToRegistry(localExe);
                            return localExe;
                        }
                    }
                }
            }
            catch
            {
            }

            // 6. Standard Program Files and LocalAppData installation folders
            string[] standardFolders = new string[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "File Converter"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "File Converter"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "File Converter"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "File Converter")
            };

            foreach (string folder in standardFolders)
            {
                try
                {
                    string localExe = Path.Combine(folder, "FileConverter.exe");
                    if (File.Exists(localExe))
                    {
                        TrySavePathToRegistry(localExe);
                        return localExe;
                    }
                }
                catch
                {
                }
            }

            return null;
        }

        private static string GetPathFromRegistry()
        {
            RegistryHive[] hives = new RegistryHive[] { RegistryHive.CurrentUser, RegistryHive.LocalMachine };
            RegistryView[] views = new RegistryView[] { RegistryView.Default, RegistryView.Registry64, RegistryView.Registry32 };

            foreach (RegistryHive hive in hives)
            {
                foreach (RegistryView view in views)
                {
                    try
                    {
                        using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, view))
                        using (RegistryKey subKey = baseKey.OpenSubKey(@"Software\FileConverter"))
                        {
                            string path = subKey?.GetValue("Path") as string;
                            if (!string.IsNullOrEmpty(path))
                            {
                                return path;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }

        private static void TrySavePathToRegistry(string exePath)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\FileConverter"))
                {
                    key?.SetValue("Path", exePath);
                }
            }
            catch
            {
            }
        }

        public static string GetUserDataFolderPath
        {
            get
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                path = Path.Combine(path, "FileConverter");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
    }
}
