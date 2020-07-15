//  Copyright (c) 2018-present amlovey
//  
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace uCodeEditor.Extensions
{
    /// <summary>
    /// UCE Editor API
    /// </summary>
    public class EditorExtensions
    {
        /// <summary>
        /// Get current opened file path in active editor instance
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentOpenFilePathInActiveEditor()
        {
            if (!MainWindow.LastActiveInstance)
            {
                return string.Empty;
            }

            var id = MainWindow.LastActiveInstance.id;
            var localSettingsFile = LocalSettings.GetLocalSettingFileById(id);

            if (!File.Exists(localSettingsFile))
            {
                return string.Empty;
            }

            var jsonContent = File.ReadAllText(localSettingsFile);
            var startMarker = "\"CurrentActiveFile\":\"";

            var startIndex = jsonContent.IndexOf(startMarker);
            return jsonContent.Substring(startIndex + startMarker.Length, jsonContent.Length - 2 - startIndex - startMarker.Length);
        }

        /// <summary>
        /// Open file in active editor
        /// </summary>
        /// <param name="path">Path of file to be opened</param>
        public static void OpenFileInActiveEditor(string path)
        {
            if (MainWindow.CanExecuteCommunicateServices())
            {
                MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(path);
            }
        }

        /// <summary>
        /// Open File at specific line number in active editor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="line"></param>
        public static void OpenFileInActiveEditor(string path, int line)
        {
            if (MainWindow.CanExecuteCommunicateServices())
            {
                MainWindow.LastActiveInstance.CommunicateServices.UEOpenFile(path, line);
            }
        }

        /// <summary>
        /// Exextur an action or command defined in editor side. For example, toogle comment.
        /// Action ids can be found in this link: http://www.amlovey.com/uce/actions/
        /// </summary>
        /// <param name="actionId">Acton Id</param>
        public static void ExecuteAction(string actionId)
        {
            if (MainWindow.CanExecuteCommunicateServices())
            {
                MainWindow.LastActiveInstance.CommunicateServices.TriggerEditorActon(actionId);
            }
        }
    }
}

