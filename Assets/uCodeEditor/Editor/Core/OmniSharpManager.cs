//  Copyright (c) 2018-present amlovey
//  
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using System.Diagnostics;

#if UNITY_EDITOR_WIN
using System.Runtime.InteropServices;
#endif

namespace uCodeEditor
{
    /// <summary>
    /// Class that use to manage Omnisharp installation
    /// </summary>
    public class OmniSharpManager
    {
        public static string GetGlobalInstallLocation()
        {
            return Utility.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".uce");
        }

        public static string GetInstallationFolder()
        {
            string folder = GetGlobalInstallLocation();
            folder = Path.Combine(folder, "omnisharp");
            Directory.CreateDirectory(folder);
            return folder;
        }

        public static string GetInstalledOmnisharpPath()
        {
            return Utility.PathCombine(GetInstallationFolder(), "omnisharp", "omnisharp.exe");
        }

        public static void UpdateOmnisharp()
        {
            try
            {
                Directory.Delete(GetInstallationFolder(), true);
                
                string zipFilePath = GetOmnisharpZipFile();

                // If the omnisharp zip file is missing, 
                // don't do anythin and just return 
                if (!File.Exists(zipFilePath))
                {
                    return;
                }

                string title = "Upgrading Omnisharp";
                string message = "Omnisharp is upgrading...";

                string zipFileName = GetOminsharpZipFileName();

                string installationFolder = GetInstallationFolder();
                EditorUtility.DisplayProgressBar(title, message, 0f);

                // 1. copy zip file to installation folder
                string targetZipFilePath = Utility.PathCombine(installationFolder, zipFileName);
                EditorUtility.DisplayProgressBar(title, message, 0.3f);
                File.Copy(zipFilePath, targetZipFilePath, true);

                // 2. unzip it
                EditorUtility.DisplayProgressBar(title, message, 0.6f);
                UnZipFile(targetZipFilePath);

                // 3. delete zip file
                EditorUtility.DisplayProgressBar(title, message, 1f);
                Clean(targetZipFilePath);

                // 4. save omnisharp version
                File.WriteAllText(PathManager.GetOmnisharpVersionFilePath(), Constants.OMNISHARP_VERSION.ToString());
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static void InstallOmnisharp()
        {
            try
            {
                string zipFilePath = GetOmnisharpZipFile();

                // If the omnisharp zip file is missing, 
                // don't do anythin and just return 
                if (!File.Exists(zipFilePath))
                {
                    return;
                }

                string title = "Install Omnisharp";
                string message = "There are no omnisharp (required by C#) installed, click 'Install' button to install one.";

                string zipFileName = GetOminsharpZipFileName();

                if (EditorUtility.DisplayDialog(title, message, "Install", ""))
                {
                    string installationFolder = GetInstallationFolder();
                    string msg = "Downloading... {0}";
                    EditorUtility.DisplayProgressBar(title, msg, 0f);

                    // 1. copy zip file to installation folder
                    string targetZipFilePath = Utility.PathCombine(installationFolder, zipFileName);
                    EditorUtility.DisplayProgressBar(title, msg, 0.3f);
                    File.Copy(zipFilePath, targetZipFilePath, true);

                    // 2. unzip it
                    msg = "Downloading and Installing";
                    EditorUtility.DisplayProgressBar(title, msg, 0.6f);
                    UnZipFile(targetZipFilePath);

                    // 3. delete zip file
                    msg = "Installing...";
                    EditorUtility.DisplayProgressBar(title, msg, 1f);
                    Clean(targetZipFilePath);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static bool CheckIfOmnisharpNeedsUpgrade()
        {
            if (Constants.OMNISHARP_VERSION == 0)
            {
                return false;
            }
            #pragma warning disable
            var omnisharpVersionFile = PathManager.GetOmnisharpVersionFilePath();
            if (File.Exists(omnisharpVersionFile))
            {
                var version = File.ReadAllText(omnisharpVersionFile);
                Utility.Log(string.Format("Omnisharp Local Version: {0}", version));
                if (int.Parse(version) < Constants.OMNISHARP_VERSION)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
            #pragma warning restore
        }

        public static bool CheckInstallationExists()
        {
            return File.Exists(GetInstalledOmnisharpPath());
        }

        private static void Clean(string zipFile)
        {
            File.Delete(zipFile);
        }

        private static void UnZipFile(string zipFile)
        {
            // unzip on different platforms
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                UnzipFileOnWindows(zipFile);
            }
            else
            {
                UnzipFileOnMac(zipFile);
            }
        }

        private static void PrepareTools()
        {
            var toolPath = Utility.PathCombine(PathManager.GetUCEFolder(), "zp.exe");
            var srcPath = Utility.PathCombine(PathManager.GetUCEFolderInProject(), "Data", "zp.db");
            if (!File.Exists(toolPath))
            {
                Utility.Unpack(srcPath, toolPath);
            }
        }

        private static void UnzipFileOnMac(string zipFile)
        {
            Process p = new Process();
            p.StartInfo.FileName = "tar";
            p.StartInfo.Arguments = string.Format("-zxvpf {0}", zipFile);
            p.StartInfo.WorkingDirectory = GetInstallationFolder();
            p.Start();
            p.WaitForExit();
        }

        private static void UnzipFileOnWindows(string zipFile)
        {
            PrepareTools();

            Process p = new Process();
            p.StartInfo.FileName = Utility.PathCombine(PathManager.GetUCEFolder(), "zp.exe");

            string arguemnts = string.Format("\"{0}\" -d \"{1}\\omnisharp\"", zipFile, GetInstallationFolder());
            p.StartInfo.Arguments = arguemnts;

            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();
        }

        private static string GetOminsharpZipFileName()
        {
#if UNITY_EDITOR_WIN
            if (Is64Bit())
            {
                return "x64.db";
            }
            else
            {
                return "x86.db";
            }        
#elif UNITY_EDITOR_OSX
            return "osx.db";
#else   
            return "";
#endif
        }

        private static string GetOmnisharpZipFile()
        {
            string folder = Utility.PathCombine(PathManager.GetUCEFolderInProject(), "Data", "Omnisharp");
            return Utility.PathCombine(folder, GetOminsharpZipFileName());
        }

#if UNITY_EDITOR_WIN
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        private static bool Is64Bit()
        {
            if (IntPtr.Size == 8 || (IntPtr.Size == 4 && Is32BitProcessOn64BitProcessor()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool Is32BitProcessOn64BitProcessor()
        {
            bool retVal;

            IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);

            return retVal;
        }
#endif
    }
}
