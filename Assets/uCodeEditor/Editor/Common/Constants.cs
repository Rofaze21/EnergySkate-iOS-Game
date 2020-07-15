//  Copyright (c) 2018-present amlovey
//  
namespace uCodeEditor
{
    public partial class Constants
    {
        public const string KEYBINDING_CONFIG_FILE = "keybinding.json";
        
        public const string CURRENT_FILE_KEY = "ue_current_file_path_key";

        /// <summary>
        /// File extension of the files support by uCodeEditor
        /// </summary>
        public static string[] ALLOWED_FILE_EXTENSIONS = new string[] {
          ".cs", ".csx", ".js", ".jsx", ".json", ".shader", ".cginc", ".cg", ".glsl", ".hlsl", ".compute",
          ".html", ".htm", ".shtml", ".xhtml", ".mdoc", ".jsp", ".asp", ".aspx", ".jshtm", ".xml",
          ".dtd", ".ascx", ".csproj", ".config", ".wxi", ".sln", ".wxl", ".wxs", ".xaml", ".svg",
          ".svgz", ".c", ".h", ".cpp", ".cc", ".cxx", ".hpp", ".hh", ".hxx", ".java", ".jav", ".m",
          ".mm", ".py", ".rpy", ".pyw", ".cpy", ".gyp", ".gypi", ".css", ".swift", ".txt", ".rsp",
          ".lua", ".css", ".scss", ".md", ".markdown", ".meta", ".uss", ".uxml", ".xsd", ".sql", ".ts",
          ".tsx", ".uce"
        };

        public const string ASSET_ID = "content/117349";
        public const string UCE_SETTINGS_KEY = "UES_SETTINGS_KEY";
        public const string SETTING_FILE_EXT = "ucesettings";

        public const int STDIO_BRIDGE_VERSION = 3;
        public const int OMNISHARP_VERSION = 0;
    }
}
