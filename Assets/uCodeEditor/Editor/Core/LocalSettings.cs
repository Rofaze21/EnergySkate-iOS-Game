//  Copyright (c) 2018-present amlovey
//  
using System.IO;

namespace uCodeEditor
{
    public class LocalSettings
    {
        private const string SETTINGS_FILE = "settings.json";
        private const string SETTINGS_FOLDER = ".uce";

        /// <summary>
        /// Get content of local settings
        /// </summary>
        /// <returns>Setting content</returns>
        public static string GetLocalSettingsPath(int id = 1)
        {
            string settingFile = GetLocalSettingFileById(id);
            if (!File.Exists(settingFile))
            {
                SaveLocalSettings("{}", id);
            }
            
            return settingFile;
        }

        /// <summary>
        /// Save sttings to local
        /// </summary>
        /// <param name="settingsJson">Json string of settings</param>
        public static void SaveLocalSettings(string settingsJson, int id = 1)
        {
            string settingFile = GetLocalSettingFileById(id);
            File.WriteAllText(settingFile, settingsJson);
        }

        /// <summary>
        /// Get path of local settings folder
        /// </summary>
        /// <returns>Local settings folder</returns>
        public static string GetOrCreateLocalSettingsFolder()
        {
            var folder = Utility.PathCombine("Library", SETTINGS_FOLDER);
			Directory.CreateDirectory(folder);
			return Path.GetFullPath(folder);
        }

        public static string GetLocalSettingFileById(int id)
        {
            var idString = id > 1 ? id.ToString() : string.Empty;
            return Utility.PathCombine(GetOrCreateLocalSettingsFolder(), idString + SETTINGS_FILE);
        }
    }
}