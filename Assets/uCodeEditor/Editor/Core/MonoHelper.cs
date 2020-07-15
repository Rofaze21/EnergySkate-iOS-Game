//  Copyright (c) 2018-present amlovey
//  
using UnityEngine;
using UnityEditor;
using System.IO;

namespace uCodeEditor
{
    public class MonoHelper
    {
        public static string GetMonoLocation()
        {
            string editorPath = EditorApplication.applicationPath;
            string frameworkPath = string.Empty;

            if (Application.platform == RuntimePlatform.OSXEditor)
            {
#if UNITY_5_4_OR_NEWER
                frameworkPath = Path.Combine(editorPath, "Contents");
#else
			    frameworkPath = Path.Combine(editorPath, Path.Combine("Contents", "Frameworks"));
#endif
            }
            else
            {
                frameworkPath = Path.Combine(Path.GetDirectoryName(editorPath), "Data");
            }

            string monoInstallation = Path.Combine(frameworkPath, "MonoBleedingEdge");
            string monoPath = Utility.PathCombine(monoInstallation, "bin", "mono");
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                monoPath = monoPath + ".exe";
            }

            return monoPath;
        }
    }
}