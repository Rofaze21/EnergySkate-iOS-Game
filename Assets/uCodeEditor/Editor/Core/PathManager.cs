//  Copyright (c) 2018-present amlovey
//  
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace uCodeEditor
{
    public class PathManager
    {
        /// <summary>
        /// Get code editor index page
        /// </summary>
        public static string GetIndexHTMLPath()
        {
            var guids = AssetDatabase.FindAssets("index");
            string checkMark = "<!--uCodeEditor:d9c4-dcfc-ca98-kklo-->";
            string htmlPath = string.Format(@"file://{0}/uCodeEditor/Editor/index.html", Application.dataPath);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.ToLower().EndsWith("index.html"))
                {
                    var obj = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    if (obj != null && !string.IsNullOrEmpty(obj.text) && obj.text.Contains(checkMark))
                    {
                        htmlPath = string.Format("file://{0}", Path.GetFullPath(path));
                    }
                }
            }

            return htmlPath;
        }

        /// <summary>
        /// Get UCE folder in project
        /// </summary>
        /// <returns></returns>
        public static string GetUCEFolderInProject()
        {
            var htmlPath = GetIndexHTMLPath();
            if (string.IsNullOrEmpty(htmlPath))
            {
                return string.Empty;
            }

            return Path.GetDirectoryName(htmlPath.Replace("file://", ""));
        }

        /// <summary>
        /// Get override shortcut mapping config path
        /// </summary>
        public static string GetKeyMappingConfigPath()
        {
            string folder = OmniSharpManager.GetGlobalInstallLocation();
            string configFilePath = Utility.PathCombine(folder, Constants.KEYBINDING_CONFIG_FILE);
            configFilePath = Path.GetFullPath(configFilePath);
            return configFilePath;
        }

        /// <summary>
        /// Get code editor config file path
        /// </summary>
        public static string GetEditorConfigPath()
        {
            string folder = OmniSharpManager.GetGlobalInstallLocation();
            return Path.GetFullPath(Path.Combine(folder, "settings.json"));
        }

        /// <summary>
        /// Get cache file path of Go To file feature
        /// </summary>
        public static string GetGoToFileCachePath()
        {
            return Utility.PathCombine("Temp", "fileCache");
        }

        /// <summary>
        /// Get cache file path of go to scene feature
        /// </summary>
        /// <returns></returns>
        public static string OpenedSceneFileCachePath()
        {
            return Utility.PathCombine(LocalSettings.GetOrCreateLocalSettingsFolder(), "LastScene.txt");
        }

        public static string GetSyncSolutionLockFile()
        {
            return Utility.PathCombine("Temp", "syncsoution.lock");
        }

        /// <summary>
        /// Get last opened files cache file
        /// </summary>
        public static string GetLastOpenedFilePath()
        {
            return Utility.PathCombine(LocalSettings.GetOrCreateLocalSettingsFolder(), "LastOpened.txt");
        }

        /// <summary>
        /// Get Global folder
        /// </summary>
        public static string GetUCEFolder()
        {
            var folder = Utility.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".uce");
            Directory.CreateDirectory(folder);
            return folder;
        }

        /// <summary>
        /// Get path of unity packages folder
        /// </summary>
        /// <returns>Path of unity packages folder</returns>
        public static string GetUnityPackagesFolder()
        {
            string folder;
#if UNITY_2018_1_OR_NEWER
            folder = Utility.PathCombine(Application.dataPath, "..", "Library", "PackageCache");
#else
#if UNITY_EDITOR_WIN
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            folder = Utility.PathCombine(localAppData, "Unity", "cache", "packages");
#else  
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            folder = Utility.PathCombine(userFolder, "Library", "Unity", "cache", "packages");
#endif
#endif
            return Path.GetFullPath(folder);
        }

        /// <summary>
        /// Get Omnisharp lock file
        /// </summary>
        /// <returns></returns>
        public static string OmnisharpRestartLockFile()
        {
            return Utility.PathCombine(Application.temporaryCachePath, "omnisharp_restart.lock");
        }

        /// <summary>
        /// Get user snippets config file path of a language
        /// </summary>
        /// <param name="language">Language name, for example shaderlab and csharp.</param>
        /// <returns>Path of user snippets config file</returns>
        public static string GetUserSnippetsFilePath(string language)
        {
            var folder = OmniSharpManager.GetGlobalInstallLocation();
            return Utility.PathCombine(folder, string.Format("{0}.json", language));
        }

        /// <summary>
        /// Get file path of stdio bridge version file.
        /// </summary>
        /// <returns>Path of stdio bridge version file</returns>
        public static string GetStdioBridgeVersionFilePath()
        {
            var folder = Utility.PathCombine(OmniSharpManager.GetGlobalInstallLocation(), "Versions");
            Directory.CreateDirectory(folder);
            return Utility.PathCombine(folder, "sb");
        }

        /// <summary>
        /// Get file path fo Omnisharp version file.
        /// </summary>
        /// <returns>Path of Omnisharp version file</returns>
        public static string GetOmnisharpVersionFilePath()
        {
            var folder = Utility.PathCombine(OmniSharpManager.GetGlobalInstallLocation(), "Versions");
            Directory.CreateDirectory(folder);
            return Utility.PathCombine(folder, "omnisharp");
        }

        /// <summary>
        /// Model sync files cache path
        /// </summary>
        /// <param name="modelPath">model path</param>
        /// <returns>Path of model sync files cache</returns>
        public static string GetModelTempCacheOverviewFilePath(string modelPath)
        {
            var md5 = Utility.MD5(modelPath);
            var folder = Utility.PathCombine(Application.dataPath, "..", "Temp", "modelCache");
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, md5 + ".ov");
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Path of files store all model chagnes
        /// </summary>
        /// <returns>Path of files store all model chagnes</returns>
        public static string GetModelStatesStorePath()
        {
            var path = Utility.PathCombine(Application.dataPath, "..", "Temp", "modelStateStore");
            return Path.GetFullPath(path);
        }
    }
}
