//  Copyright (c) 2018-present amlovey
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Text;
using System;
using System.Linq;
using System.Threading;

namespace uCodeEditor
{
    #region Core Commands
#if UNITY_EDITOR_WIN
	[UCommand("uce.toggle.maximize.editor", "Toggle uCodeEditor Maximize", KeyCode.CtrlCmd | KeyCode.Alt | KeyCode.KEY_X)]
#else
    [UCommand("uce.toggle.maximize.editor", "Toggle uCodeEditor Maximize", KeyCode.Shift | KeyCode.Space)]
#endif
    public class ToggleEditorMaximize : UDynmaicCommand
    {
        public override void Run()
        {
            if (MainWindow.LastActiveInstance != null)
            {
                MainWindow.LastActiveInstance.maximized = !MainWindow.LastActiveInstance.maximized;
            }
        }
    }

    [UCommand("uce.online.documents", "Help: Online Documents")]
    public class OnlineDocuments : UDynmaicCommand
    {
        public override void Run()
        {
            EditorApplication.ExecuteMenuItem("Tools/uCodeEditor/Online Documentation");
        }
    }

#if UNITY_EDITOR_WIN
	[UCommand("uce.open.in.shell", "Project: Open In Command Line")]
#else
    [UCommand("uce.open.in.shell", "Project: Open In Terminal")]
#endif
    public class OpenProjectInShell : UDynmaicCommand
    {
        public override void Run()
        {
            var path = Utility.PathCombine(Application.dataPath, "..");

#if UNITY_EDITOR_WIN
			Process.Start("cmd", string.Format("-k {0}", path));
#else
            Process.Start("open", string.Format("-b com.apple.Terminal {0}", path));
#endif
        }
    }

    [UCommand("uce.open.file", "Open File...")]
    public class OpenFile : UDynmaicCommand
    {
        public override void Run()
        {
            var fileToOpen = EditorUtility.OpenFilePanel("Open", Application.dataPath, "");
            if (!string.IsNullOrEmpty(fileToOpen))
            {
                if (MainWindow.CanExecuteCommunicateServices())
                {
                    MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(fileToOpen);
                }
            }
        }
    }

    [UCommand("uce.toggle.uce.association", "Toggle uCodeEditor Association")]
    public class ToggleUCEAssociationCommand : UDynmaicCommand
    {
        public override void Run()
        {
            var linkMacro = "DISABLE_UCE_ASSOCIATION";
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            bool isOn = false;
            if (macros.Contains(linkMacro))
            {
                macros = macros.Replace(linkMacro, "");
                isOn = true;
            }
            else
            {
                macros = string.Format("{0};{1}", macros, linkMacro);
                isOn = false;
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, macros);
            string msg = string.Format("uCodeEditor Association is {0}. It will available after recompilation.", isOn ? "on" : "off");
            EditorUtility.DisplayDialog("Info", msg, "Ok");
            AssetDatabase.Refresh();
        }
    }

    #endregion

    #region Quick Opens
    [UCommand("uce.export.settings", "Export Settings...")]
    public class ExportSettings : UDynmaicCommand
    {
        public override void Run()
        {
            var savePath = EditorUtility.SaveFilePanel(
                "Export Settings",
                Utility.PathCombine(Application.dataPath, ".."),
                "setting",
                Constants.SETTING_FILE_EXT);

            if (string.IsNullOrEmpty(savePath))
            {
                return;
            }

            // editor config
            var editorConfig = "{}";
            var editorConfigPath = PathManager.GetEditorConfigPath();
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if (File.Exists(editorConfigPath))
            {
                editorConfig = File.ReadAllText(editorConfigPath);
                sb.Append(string.Format("\"editor\":{0},", editorConfig));
            }

            // key mapping
            var keymapping = "{}";
            var keymappingPath = PathManager.GetKeyMappingConfigPath();
            if (File.Exists(keymappingPath))
            {
                keymapping = File.ReadAllText(keymappingPath);
                sb.Append(string.Format("\"keymap\":{0}", keymapping));
            }

            sb.Append("}");

            File.WriteAllText(savePath, sb.ToString());
        }
    }

    [UCommand("uce.reveal.current.file.folder", "Unity Editor: Reveal Current File folder")]
    public class RevealCurrentFileFolder : UDynmaicCommand
    {
        public override void Run()
        {
            var currentActiveFile = Extensions.EditorExtensions.GetCurrentOpenFilePathInActiveEditor();
            if (File.Exists(currentActiveFile))
            {
                EditorUtility.RevealInFinder(Path.GetDirectoryName(currentActiveFile));
            }
        }
    }

#if UNITY_EDITOR_OSX
    [UCommand("uce.quick.open.scenes", "Unity Editor: Go To Scene...", KeyCode.CtrlCmd | KeyCode.US_QUOTE)]
#else
    [UCommand("uce.quick.open.scenes", "Unity Editor: Go To Scene...", KeyCode.CtrlCmd | KeyCode.KEY_P)]
#endif
    public class QuickOpenScenes : UQuickOpenCommand
    {
        public override void RunEntry(string id)
        {
            var guids = AssetDatabase.FindAssets("t:Scene");
            foreach (var gid in guids)
            {
                if (gid.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    var path = AssetDatabase.GUIDToAssetPath(gid);
#if UNITY_5_3_OR_NEWER
                    if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().isDirty)
                    {
                        UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    }

                    var openMode = UnityEditor.SceneManagement.OpenSceneMode.Single;
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path, openMode);
#else
                    if (EditorApplication.isSceneDirty)
                    {
                        EditorApplication.SaveCurrentSceneIfUserWantsTo();
                    }
                    
                    EditorApplication.OpenScene(path);
#endif
                    SaveOpenedId(id);
                    break;
                }
            }
        }

        private void SaveOpenedId(string id)
        {
            var cacheFile = PathManager.OpenedSceneFileCachePath();
            if (File.Exists(cacheFile))
            {
                var ids = File.ReadAllLines(cacheFile).ToList();
                if (ids.Contains(id))
                {
                    ids.Remove(id);
                }

                ids.Add(id);

                File.WriteAllLines(cacheFile, ids.ToArray());
            }
            else
            {
                File.WriteAllLines(cacheFile, new string[] { id });
            }
        }

        public override IEnumerable<QuickOpenEntryMeta> GetQuickOpenEntries()
        {
            var cacheFile = PathManager.OpenedSceneFileCachePath();
            List<string> ids = null;
            if (File.Exists(cacheFile))
            {
                ids = File.ReadAllLines(cacheFile).Reverse().ToList();
            }

            var guids = AssetDatabase.FindAssets("t:Scene");

            if (ids != null)
            {
                foreach (var id in guids)
                {
                    if (ids.Contains(id))
                    {
                        continue;
                    }
                    ids.Add(id);
                }
            }
            else
            {
                ids = guids.ToList();
            }

            List<QuickOpenEntryMeta> data = new List<QuickOpenEntryMeta>();
            foreach (var id in ids)
            {
                if (string.IsNullOrEmpty(id))
                {
                    continue;
                }

                QuickOpenEntryMeta meta = new QuickOpenEntryMeta();
                meta.id = id;

                var scenePath = AssetDatabase.GUIDToAssetPath(id);
                meta.label = Path.GetFileNameWithoutExtension(scenePath);
                meta.description = Path.GetDirectoryName(scenePath).Replace("\\", "/");
                meta.icon = "scene";

                data.Add(meta);
            }

            return data;
        }
    }

    [UCommand("uce.reveal.special.folder", "Unity Editor: Reveal Special Folder...")]
    public class RevealSpecialFolder : UQuickOpenCommand
    {
        private static string[] SPECIAL_FOLDERS = new string[] {
            "Application.persistentDataPath", // id = 0
            "Application.dataPath", //id = 1
            "Application.streamingAssetsPath", // id = 2
            "Application.temporaryCachePath", // id = 3 
            "Asset Store Packages Folder", // id = 4
            "Editor Application Path", // id = 5
            "Editor Log Folder", // id = 6
#if UNITY_2017_1_OR_NEWER
            "Packages Folder", // id = 7
#endif
            "Scripts Templates", // id = 8
        };

        public override void RunEntry(string id)
        {
            switch (id)
            {
                case "0":
                    Utility.Reveal(Application.persistentDataPath);
                    break;
                case "1":
                    Utility.Reveal(Application.dataPath);
                    break;
                case "2":
                    Utility.Reveal(Application.streamingAssetsPath);
                    break;
                case "3":
                    Utility.Reveal(Application.temporaryCachePath);
                    break;
                case "4":
#if UNITY_EDITOR_OSX
                    string path = GetAssetStorePackagesPathOnMac();
#elif UNITY_EDITOR_WIN
			        string path = GetAssetStorePackagesPathOnWindows();
#endif
                    Utility.Reveal(path);
                    break;
                case "5":
                    Utility.Reveal(GetEditorApplicationInstallFolder());
                    break;
                case "6":
                    OpenEditorLogFolderPath();
                    break;
                case "7":
                    Utility.Reveal(PathManager.GetUnityPackagesFolder());
                    break;
                case "8":
                    Utility.Reveal(GetScriptsTemplateFolder());
                    break;
                default:
                    break;
            }
        }

        public override IEnumerable<QuickOpenEntryMeta> GetQuickOpenEntries()
        {
            List<QuickOpenEntryMeta> metas = new List<QuickOpenEntryMeta>();
            for (int i = 0; i < SPECIAL_FOLDERS.Length; i++)
            {
                QuickOpenEntryMeta meta = new QuickOpenEntryMeta();
                meta.id = i.ToString();
                meta.label = SPECIAL_FOLDERS[i];
                meta.description = "";

                metas.Add(meta);
            }

            return metas;
        }

        private string GetScriptsTemplateFolder()
        {
            return Utility.PathCombine(EditorApplication.applicationContentsPath, "Resources", "ScriptTemplates");
        }

        private string GetEditorApplicationInstallFolder()
        {
            return new FileInfo(EditorApplication.applicationPath).Directory.FullName;
        }

        private const string ASSET_STORE_FOLDER_NAME = "Asset Store-5.x";
        private string GetAssetStorePackagesPathOnMac()
        {
            var rootFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(rootFolderPath, "Library");
            var unityFolder = Path.Combine(libraryPath, "Unity");
            return Path.Combine(unityFolder, ASSET_STORE_FOLDER_NAME);
        }

        private string GetAssetStorePackagesPathOnWindows()
        {
            var rootFolderPath = System.Environment.ExpandEnvironmentVariables("%appdata%");
            var unityFolder = Path.Combine(rootFolderPath, "Unity");
            return Path.Combine(unityFolder, ASSET_STORE_FOLDER_NAME);
        }

        private void OpenEditorLogFolderPath()
        {
#if UNITY_EDITOR_OSX
            string rootFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(rootFolderPath, "Library");
            var logsFolder = Path.Combine(libraryPath, "Logs");
            var UnityFolder = Path.Combine(logsFolder, "Unity");
            Utility.Reveal(UnityFolder);
#elif UNITY_EDITOR_WIN
			var rootFolderPath = System.Environment.ExpandEnvironmentVariables("%localappdata%");
			var unityFolder = Path.Combine(rootFolderPath, "Unity");
			Utility.Reveal(Path.Combine(unityFolder, "Editor"));
#endif
        }
    }

    [UCommand("uce.config.user.snippets", "Config User Snippets...")]
    public class ConfigUserSnippetsCommand : UQuickOpenCommand
    {
        private static string[] SupportSnippetLanguages = new string[] {
            "CSharp",
            "Shaderlab"
        };

        const string EmptySnippetsTempate = @"{{
/*
	// Place your snippets for {0} here. Each snippet is defined under a snippet name and has a prefix, body and 
	// description. The prefix is what is used to trigger the snippet and the body will be expanded and inserted. Possible variables are:
	// $1, $2 for tab stops, $0 for the final cursor position, and ${{1:label}}, ${{2:another}} for placeholders. Placeholders with the 
	// same ids are connected.
    //
    // For more, please refer to https://code.visualstudio.com/docs/editor/userdefinedsnippets
    // 
	// Example:
    //
	""Print to console"": {{
		""prefix"": ""log"",
		""body"": [
			""console.log('$1');"",
			""$2""
		],
		""description"": ""Log output to console""
	}}
*/
}}";

        public override void RunEntry(string id)
        {
            var snippetsFilePath = PathManager.GetUserSnippetsFilePath(id);

            if (!File.Exists(snippetsFilePath))
            {
                var content = string.Format(EmptySnippetsTempate, id);
                File.WriteAllText(snippetsFilePath, content);
            }

            if (MainWindow.CanExecuteCommunicateServices())
            {
                MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(Utility.PathNormalized(snippetsFilePath));
            }
        }

        public override IEnumerable<QuickOpenEntryMeta> GetQuickOpenEntries()
        {
            List<QuickOpenEntryMeta> list = new List<QuickOpenEntryMeta>();
            foreach (var item in SupportSnippetLanguages)
            {
                QuickOpenEntryMeta meta = new QuickOpenEntryMeta();
                meta.id = item;
                meta.label = item;

                list.Add(meta);
            }

            return list;
        }
    }

    [UCommand("uce.config.scripts.templates", "Config Script Templates...")]
    public class ConfigScriptTemplatesCommand : UQuickOpenCommand
    {
        public override IEnumerable<QuickOpenEntryMeta> GetQuickOpenEntries()
        {
            var folder = Utility.PathCombine(EditorApplication.applicationContentsPath, "Resources", "ScriptTemplates");
            var files = Directory.GetFiles(Path.GetFullPath(folder), "*.txt");
            List<QuickOpenEntryMeta> quickOpens = new List<QuickOpenEntryMeta>();
            foreach (var file in files)
            {
                QuickOpenEntryMeta meta = new QuickOpenEntryMeta();
                meta.id = file.Replace("\\", "/");
                meta.label = Path.GetFileNameWithoutExtension(file);
                meta.icon = "text";
                
                quickOpens.Add(meta);
            }

            return quickOpens;
        }

        public override void RunEntry(string id)
        {
            if (File.Exists(id) && MainWindow.CanExecuteCommunicateServices())
            {
                MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(id);
            }
        }
    }

    #endregion

    #region Trouble Shooting

    [UCommand("uce.get.omnisharp.errors", "Omnisharp: Get Error Log")]
    public class GetOmnisharpErrorsCommand : UDynmaicCommand
    {
        public override void Run()
        {
            Utility.RunAsyncTask(() =>
            {
                var url = string.Format("{0}/?action=geterror&id={1}", OnLoad.HOST, OnLoad.Id);
                var content = Utility.HttpGET(url);
                EditorCoroutine.StartCoroutine(() =>
                {
                    var savePath = Utility.PathCombine(Application.temporaryCachePath, "omnisharp_errors.log");
                    File.WriteAllText(savePath, content);
                    if (MainWindow.CanExecuteCommunicateServices())
                    {
                        MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(savePath);
                    }
                });
            });
        }
    }

    [UCommand("uce.get.omnisharp.logs", "Omnisharp: Get All Log")]
    public class GetOmnisharpLogsCommand : UDynmaicCommand
    {
        public override void Run()
        {
            Utility.RunAsyncTask(() =>
            {
                var url = string.Format("{0}/?action=getall&id={1}", OnLoad.HOST, OnLoad.Id);
                var content = Utility.HttpGET(url);
                EditorCoroutine.StartCoroutine(() =>
                {
                    var savePath = Utility.PathCombine(Application.temporaryCachePath, "omnisharp_all.log");
                    File.WriteAllText(savePath, content);
                    if (MainWindow.CanExecuteCommunicateServices())
                    {
                        MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(savePath);
                    }
                });
            });
        }
    }

    [UCommand("uce.restart.omnisharp.logs", "Omnisharp: Restart")]
    public class RestartOmnisharpCommand : UDynmaicCommand
    {
        public override void Run()
        {
            Utility.RunAsyncTask(() =>
            {
                var url = string.Format("{0}?action=restart&id={1}", OnLoad.HOST, OnLoad.Id);
                var result = Utility.HttpGET(url);
                Utility.LogWithName(result);
            });
        }
    }

    #endregion
}
