//  Copyright (c) 2018-present amlovey
//  
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace uCodeEditor
{
    public class MenuActions
    {
        [UnityEditor.MenuItem("Assets/Open In uCodeEditor %j", false, 66)]
        private static void OpenInUE()
        {
            if (Selection.objects.Length > 1)
            {
                var files = Selection.objects
                                .Select(obj => AssetDatabase.GetAssetPath(obj))
                                .Where(f => !Utility.IsDirectory(f) && Utility.IsFileAllowed(f))
                                .Select(f => Path.GetFullPath(f));

                if (MainWindow.CanExecuteCommunicateServices())
                {
                    MainWindow.LastActiveInstance.CommunicateServices.OpenFiles(files);
                }
            }
            else
            {
                var result = ShowContentInUE(Selection.activeObject);
                if (result.HasValue)
                {
                    if (result.Value)
                    {
                        MainWindow.LoadWindow();
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Not Support", "This asset is not supported!", "Ok");
                    }
                }
            }
        }

        [UnityEditor.MenuItem("Assets/Open In uCodeEditor %j", true)]
        private static bool OpenInUEEnable()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(path))
            {
                return Utility.IsFileAllowed(path);
            }

            return false;
        }

        /// <summary>
        /// Show selected asset content in uCodeEditor
        /// </summary>
        /// <param name="asset">The selected object</param>
        /// <param name="openNew">Open new tab or not</param>
        public static bool? ShowContentInUE(Object asset, bool openNew = true)
        {
            if (asset == null)
            {
                return null;
            }

            var path = AssetDatabase.GetAssetPath(asset);
            if (Utility.IsFileAllowed(path) && MainWindow.CanExecuteCommunicateServices())
            {
                MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(path, openNew);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}