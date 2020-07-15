//  Copyright (c) 2018-present amlovey
//  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

namespace uCodeEditor
{
    /// <summary>
    /// Actions when files changed
    /// </summary>
    public class FileWatcher : AssetPostprocessor
    {
        public static HashSet<string> ALLOWED_FILES_CACHE = new HashSet<string>();
        public static List<string> LAST_OPENED_FILES = new List<string>();

        private delegate void HandleAction(string path, string type, bool hasRemoveAction, string oldFile = "");

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool hasCSFileChanged = false;
            bool needRestartOmnisharp = false;

            HandleAction action = (string path, string type, bool hasRemoveAction, string oldFile) =>
            {
                FileChangedWithAllowedCheck(path, type, oldFile);
                if (!hasCSFileChanged)
                {
                    hasCSFileChanged = Utility.IsCSharpScript(path);
                }

                // WORKAROUND: If have rename action for cs file, we need to restart Omnisharp
                // to take the intellisense back.
                if (!string.IsNullOrEmpty(oldFile) && !needRestartOmnisharp) 
                {
                    needRestartOmnisharp = Utility.IsCSharpScript(oldFile);
                    if (needRestartOmnisharp)
                    {
                        var lockFile = PathManager.OmnisharpRestartLockFile();
                        if (!File.Exists(lockFile))
                        {
                            File.WriteAllText(PathManager.OmnisharpRestartLockFile(), "");
                        }
                    }
                }

                if (hasRemoveAction)
                {
                    if (Utility.IsFileAllowed(path))
                    {
                        ALLOWED_FILES_CACHE.Add(Path.GetFullPath(path));
                    }

                    if (!string.IsNullOrEmpty(oldFile))
                    {
                        ALLOWED_FILES_CACHE.Remove(Path.GetFullPath(oldFile));
                    }
                }
                else
                {
                    if (Utility.IsFileAllowed(path))
                    {
                        ALLOWED_FILES_CACHE.Add(Path.GetFullPath(path));
                    }
                }
            };

            foreach (string path in deletedAssets)
            {
                Utility.Log("deleting " + path);
                action(path, "delete", true);
            }

            for (int i = 0; i < movedAssets.Length; i++)
            {
                Utility.Log(string.Format("move {0} to {1}", movedFromAssetPaths[i], movedAssets[i]));
                action(movedAssets[i], "rename", true, movedFromAssetPaths[i]);
            }

            foreach (string path in importedAssets)
            {
                Utility.Log("chaning " + path);
                action(path, "change", false);
            }
            
            Utility.Log("hasCSFileChanged = " + hasCSFileChanged);

            if (hasCSFileChanged)
            {
#if !DISABLE_UCE_ASSOCIATION
                FileWatcher.SyncSolution();
#else
                FileWatcher.SendSyncProjectRequests();
#endif
            }

            File.WriteAllLines(PathManager.GetGoToFileCachePath(), ALLOWED_FILES_CACHE.ToArray());
        }

        public static void AddToLastOpen(string path)
        {
            // We don't want to same item added to list mulitple times, so we need to 
            // filter out dupliate item and then append new item.
            //
            var fullPath = Path.GetFullPath(path);
            LAST_OPENED_FILES.RemoveAll(p => p.Equals(fullPath, StringComparison.OrdinalIgnoreCase));
            LAST_OPENED_FILES.Add(fullPath);

            // Save data to cache file
            File.WriteAllLines(PathManager.GetLastOpenedFilePath(), LAST_OPENED_FILES.ToArray());
        }

        public static void RefreshAllowedFilesCache()
        {
            var files = Utility.GetAllAllowedFiles(OnLoad.WORKING_DIRECTORY);
            ALLOWED_FILES_CACHE.Clear();

            foreach (var item in files)
            {
                ALLOWED_FILES_CACHE.Add(item);
            }

            File.WriteAllLines(PathManager.GetGoToFileCachePath(), files.ToArray());

            var lastOpenedFilesCache = PathManager.GetLastOpenedFilePath();
            if (File.Exists(lastOpenedFilesCache))
            {
                var filesInCache = File.ReadAllLines(lastOpenedFilesCache);
                LAST_OPENED_FILES = filesInCache.ToList();
            }
        }

        private static void FileChangedWithAllowedCheck(string path, string changeType, string oldFile)
        {
            if (string.IsNullOrEmpty(path)
                || string.IsNullOrEmpty(changeType))
            {
                return;
            }

            if (Utility.IsFileAllowed(path))
            {
                // We need Update all Main Window instances here:
                var allMainWindows = MainWindow.GetAllInstances();
                if (allMainWindows == null)
                {
                    return;
                }
                
                foreach (var window in allMainWindows)
                {
                    if (window.CommunicateServices != null)
                    {
                        window.CommunicateServices.FileChanged(path, changeType, oldFile);
                    }
                }
            }
        }

        public static void SyncSolution(bool sendProjectChangeRequest = true)
        {
            var lockFile = PathManager.GetSyncSolutionLockFile();
            try
            {
                if (File.Exists(lockFile))
                {
                    return;
                }

                File.WriteAllText(lockFile, "");
                ClearSolution();
                System.Type T = System.Type.GetType("UnityEditor.SyncVS,UnityEditor");
                System.Reflection.MethodInfo SyncSolution = T.GetMethod("SyncSolution", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                SyncSolution.Invoke(null, null);

                if (!sendProjectChangeRequest)
                {
                    File.Delete(lockFile);
                    return;
                }

                SendSyncProjectRequests();
                File.Delete(lockFile);
            }
            catch (System.Exception e)
            {
                if (File.Exists(lockFile))
                {
                    File.Delete(lockFile);
                }
                Debug.LogWarning(e);
            }
        }

        public static void SendSyncProjectRequests()
        {
            var projectFolder = Path.Combine(Application.dataPath, "..");
            var projectFiles = Directory.GetFiles(projectFolder, "*.csproj");
            foreach (var item in projectFiles)
            {
                if (MainWindow.CanExecuteCommunicateServices())
                {
                    var path = Utility.PathNormalized(Path.GetFullPath(item));
                    MainWindow.LastActiveInstance.CommunicateServices.FileChanged(path, "change", "");
                }
            }
        }

        private static void ClearSolution()
        {
            var projectFolder = Path.Combine(Application.dataPath, "..");
            var solutionFiles = Directory.GetFiles(projectFolder, "*.sln");
            var projectFiles = Directory.GetFiles(projectFolder, "*.csproj");
            var unityProjectFiles = Directory.GetFiles(projectFolder, "*.unityproj");

            foreach (string solutionFile in solutionFiles)
            {
                File.Delete(solutionFile);
            }

            foreach (string projectFile in projectFiles)
            {
                File.Delete(projectFile);
            }

            foreach (string unityProjectFile in unityProjectFiles)
            {
                File.Delete(unityProjectFile);
            }
        }
    }
}