//  Copyright (c) 2018-present amlovey
//  
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections;
using System.Threading;
using UnityEditor.Callbacks;

namespace uCodeEditor
{
    /// <summary>
    /// On Someting on Editor Load. But note that it will execute twice on Editor launch in some Unity Editor version.
    /// </summary>
    [InitializeOnLoad]
    public class OnLoad
    {
        public static string SolutionPath;
        public static string Id;

        // Host that connect to Omnisharp. Please don't change the value.
        public static string HOST = "http://127.0.0.1:8188/";

        public static int PORT = 8188;
        public static string WORKING_DIRECTORY;
        private static string MONO_PATH;
        private static string UCE_FOLDER_IN_PROJECT;
        private static string STDIO_BRIDGE_PATH;
        private static string OMNISHARP_LOCK;
        private static Thread STDIO_THREAD;
        private static int PROCESS_ID;
        private static string PATH_EXTRA = "/Library/Frameworks/Mono.framework/Versions/Current/Commands/:/usr/local/bin/";

        static OnLoad()
        {
            SetDefaultScriptEditor();
            EditorUtility.ClearProgressBar();
            UCE_FOLDER_IN_PROJECT = PathManager.GetUCEFolderInProject();
            
            EditorCoroutine.StartCoroutine(SyncSolution());
            
            Utility.Log(string.Format("OmniSharpManager.CheckIfOmnisharpNeedsUpgrade = {0}", OmniSharpManager.CheckIfOmnisharpNeedsUpgrade()));

            if (!OmniSharpManager.CheckInstallationExists())
            {
                OmniSharpManager.InstallOmnisharp();
            }
            else if (OmniSharpManager.CheckIfOmnisharpNeedsUpgrade())
            {
                OmniSharpManager.UpdateOmnisharp();
            }
            
            MONO_PATH = MonoHelper.GetMonoLocation();
            STDIO_BRIDGE_PATH = GetStdioBridgePath();
            WORKING_DIRECTORY = Application.dataPath;
            PROCESS_ID = Process.GetCurrentProcess().Id;
            OMNISHARP_LOCK = PathManager.OmnisharpRestartLockFile();

            STDIO_THREAD = new Thread(new ThreadStart(LanuchOmniSharp));
            STDIO_THREAD.Start();

            EditorApplication.update += FileSearch.Update;
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        private static void UndoRedoPerformed()
        {
#if UNITY_2018_3_OR_NEWER && UNITY_EDITOR_OSX
            if (MainWindow.LastActiveInstance)
            {
                // MainWindow.LastActiveInstance.CommunicateServices.OnRedoUndoCalled();
            }
#endif
        }

        private static void SetDefaultScriptEditor()
        {
#if !DISABLE_UCE_ASSOCIATION
            string key = "kScriptsDefaultApp";

            // Backup current value
            // var currentApp = EditorPrefs.GetString(key);
            EditorPrefs.SetString(key, "MonoDevelop (built-in)");
#endif
        }

        private static IEnumerator SyncSolution()
        {
#if !DISABLE_UCE_ASSOCIATION
            FileWatcher.SyncSolution(false);
#else
            FileWatcher.SendSyncProjectRequests();
#endif
            // we don't want to loop forever here
            int count = 0;
            while (string.IsNullOrEmpty(SolutionPath) || count > 100)
            {
                yield return new WaitForSeconds(1);
                count++;
                FindSolutionPath();
            }
            yield return null;
        }

        ~OnLoad()
        {
            if (STDIO_THREAD != null)
            {
                STDIO_THREAD.Abort();
            }

            EditorApplication.update -= FileSearch.Update;
            Undo.undoRedoPerformed -= UndoRedoPerformed;
        }

        private static void LanuchOmniSharp()
        {
            FileWatcher.RefreshAllowedFilesCache();

            if (!IsStdioBridgeServerAlive())
            {
                StartStdioBridgeServer();

                // waiting for solution alive
                while (!IsStdioBridgeServerAlive())
                {
                    Thread.Sleep(1000);
                }
            }

            // Waiting for solution path
            while (string.IsNullOrEmpty(SolutionPath))
            {
                Thread.Sleep(1000);
            }

            Utility.Log("id=" + Utility.MD5(SolutionPath).ToLower());

            if (!string.IsNullOrEmpty(SolutionPath))
            {
                var existsProcess = Utility.HttpGET(CreateLaunchProcessUrl());
                // waiting for process up
                while (existsProcess != "exists")
                {
                    Thread.Sleep(1000);
                    existsProcess = Utility.HttpGET(CreateLaunchProcessUrl());
                    Utility.Log(existsProcess);
                }

                if (existsProcess == "exists")
                {
                    Utility.LogWithName("Connected to Omnisharp Server");
                }

                // If omnisharp lock file exits, we need to restart omnisharp
                if (File.Exists(OMNISHARP_LOCK))
                {
                    File.Delete(OMNISHARP_LOCK);

                    var url = string.Format("{0}?action=restart&id={1}", HOST, Id);
                    Utility.HttpGET(url);
                }
            }
        }

        private static bool IsStdioBridgeServerAlive()
        {
            string url = string.Format("{0}?action=checkalive", HOST);
            string ret = Utility.HttpGET(url);
            return ret == "200";
        }

        private static void StartStdioBridgeServer()
        {
            Utility.LogWithName("Starting StdioBridge Server");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = MONO_PATH;
            startInfo.Arguments = string.Format("\"{0}\" -port {1} -omnisharp \"{2}\" -platform {3} -mono \"{4}\"", STDIO_BRIDGE_PATH, PORT, GetOmnisharpPath(), GetCurrentPlatform(), MONO_PATH);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.WorkingDirectory = WORKING_DIRECTORY;

            // need to set mono path to PATH enviroment variable, otherwise application will fails
            startInfo.EnvironmentVariables["PATH"] = string.Format("{0}:{1}", Environment.GetEnvironmentVariable("PATH"), PATH_EXTRA);

            Process p = new Process();
            p.StartInfo = startInfo;

            if (p.Start())
            {
                Utility.LogWithName("Started StdioBridge Server");
            }

            Utility.Log(MONO_PATH + " " + startInfo.Arguments);
        }

        private static string GetCurrentPlatform()
        {
            return Application.platform == RuntimePlatform.OSXEditor ? "mac" : "win";
        }

        private static string GetStdioBridgePath()
        {
            var installedPath = Utility.PathCombine(PathManager.GetUCEFolder(), "StdioBridge.exe");
            var versionFilePath = PathManager.GetStdioBridgeVersionFilePath();
            
            bool needUpdated = false;
            if (File.Exists(versionFilePath))
            {
                var cachedVersion = File.ReadAllText(versionFilePath).Trim();
                int cachedVersionNumber;
                if (int.TryParse(cachedVersion, out cachedVersionNumber))
                {
                    if (cachedVersionNumber < Constants.STDIO_BRIDGE_VERSION)
                    {
                        needUpdated = true;
                    }
                }
                else
                {
                    needUpdated = true;
                }
            }
            else
            {
                needUpdated = true;
            }

            var srcPath = Path.Combine(UCE_FOLDER_IN_PROJECT, "Data/sb.db");
            if (!File.Exists(installedPath))
            {
                Utility.Unpack(srcPath, installedPath);
                if (needUpdated)
                {
                    File.WriteAllText(versionFilePath, Constants.STDIO_BRIDGE_VERSION.ToString());
                }

                return installedPath;
            }

            if (needUpdated)
            {
                Utility.Unpack(srcPath, installedPath);
                File.WriteAllText(versionFilePath, Constants.STDIO_BRIDGE_VERSION.ToString());
            }

            return installedPath;
        }

        private static void FileCopyWithErrorLess(string src, string dst)
        {
            try
            {
                File.Copy(src, dst, true);
            }
            catch(Exception e)
            {
                Utility.Log(e);
            }
        }

        private static string GetOmnisharpPath()
        {
            return OmniSharpManager.GetInstalledOmnisharpPath();
        }

        private static string CreateLaunchProcessUrl()
        {
            return string.Format("{0}?action=create&solution={1}&hostid={2}", HOST, Uri.EscapeDataString(SolutionPath), PROCESS_ID);
        }

        private static void FindSolutionPath()
        {
            var files = Directory.GetFiles(Path.Combine(Application.dataPath, ".."), "*.sln");
            foreach (var file in files)
            {
                SolutionPath = Path.GetFullPath(file);
                Id = Utility.MD5(SolutionPath).ToLower();
                return;
            }
        }

#if !DISABLE_UCE_ASSOCIATION
        [OnOpenAssetAttribute(0)]
#endif
        public static bool OpenInUEByDoubleClick(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);
            var path = AssetDatabase.GetAssetPath(asset);

            Utility.Log(string.Format("Open {0} with line {1}", path, line));

            // If double click on Console Window
            var stackTrace = GetSelectedStackTrace();
            if (!string.IsNullOrEmpty(stackTrace))
            {
                if (line >= 0)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (MainWindow.LastActiveInstance != null)
                        {
                            MainWindow.LoadWindow();
                            if (MainWindow.LastActiveInstance.CommunicateServices != null)
                            {
                                MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(path, line);
                            }
                        }
                        else
                        {
                            var filePath = Utility.PathNormalized(Path.GetFullPath(path));
                            PlayerPrefs.SetString(Constants.CURRENT_FILE_KEY, string.Format("{0}:{1}", filePath, line));
                            MainWindow.LoadWindow();
                        }

                        return true;
                    }
                }

                return false;
            }

            Utility.Log(string.Format("stack tracke = {0}", !string.IsNullOrEmpty(stackTrace)));

            // If not in Console Window
            // Check if it's a folder
            bool isDirectory = Utility.IsDirectory(path);
            if (isDirectory)
            {
                return true;
            }

            // If it's a file, check if it's valid file type
            if (Utility.IsFileAllowed(path))
            {
                if (MainWindow.LastActiveInstance == null)
                {
                    var filePath = Utility.PathNormalized(Path.GetFullPath(path));
                    if (line > 0)
                    {
                        PlayerPrefs.SetString(Constants.CURRENT_FILE_KEY, string.Format("{0}:{1}", filePath, line));
                    }
                    else
                    {
                        PlayerPrefs.SetString(Constants.CURRENT_FILE_KEY, filePath);
                    }
                    
                    MainWindow.LoadWindow();
                }
                else
                {
                    MainWindow.LoadWindow();
                    if (MainWindow.CanExecuteCommunicateServices())
                    {
                        MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(path, line);
                    }
                }
            
                return true;
            }

            return false;
        }

        private static string GetSelectedStackTrace()
        {
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            var fieldInfo = type.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            var console = fieldInfo.GetValue(null);

            if (null != console)
            {
                if ((object)EditorWindow.focusedWindow == console)
                {
                    fieldInfo = type.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
                    string activeText = fieldInfo.GetValue(console).ToString();
                    return activeText;
                }
            }

            return "";
        }
    }
}