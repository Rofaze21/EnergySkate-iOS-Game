//  Copyright (c) 2018-present amlovey
//  
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

namespace uCodeEditor
{
    public partial class Utility
    {
        public static string ConvertToBase64(string content)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(content)));
        }

        public static string FromBase64(string content)
        {
            return Uri.UnescapeDataString(Encoding.UTF8.GetString(Convert.FromBase64String(content)));
        }

        public static string MD5(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static void Unpack(string srcFilePath, string dstPath)
        {
            if (string.IsNullOrEmpty(srcFilePath) || string.IsNullOrEmpty(dstPath) || !File.Exists(srcFilePath))
            {
                return;
            }

            string content = File.ReadAllText(srcFilePath);
            try 
            {
                File.WriteAllBytes(dstPath, Convert.FromBase64String(content));
            }
            catch(IOException e)
            {
                Utility.LogWithName(string.Format("Please restart all Unity Editor instances due to IO Exception: {0}", e.Message));
            }
        }

        public static void Pack(string srcFilePath, string dstPath)
        {
            if (string.IsNullOrEmpty(srcFilePath) || string.IsNullOrEmpty(dstPath) || !File.Exists(srcFilePath))
            {
                return;
            }

            byte[] bytes = File.ReadAllBytes(srcFilePath);
            File.WriteAllText(dstPath, Convert.ToBase64String(bytes));
        }

        public static string GetFileMD5(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return MD5(File.ReadAllText(path));
        }

        public static void Log(object msg)
        {
#if uCE_DEV
            UnityEngine.Debug.Log(msg);
#endif
        }

        public static void LogWithName(object msg)
        {
            UnityEngine.Debug.Log(string.Format("[uCodeEditor] {0}", msg.ToString()));
        }

        public static string PathCombine(params string[] parts)
        {
            var path = parts[0];
            for (var i = 1; i < parts.Length; ++i)
            {
                path = Path.Combine(path, parts[i]);
            }

            return path;
        }

        public static bool IsCSharpScript(string path)
        {
            if (IsDirectory(path))
            {
                return false;
            }

            var lowerPath = path.ToLower();
            return lowerPath.EndsWith(".cs") || lowerPath.EndsWith(".csx");
        }

        public static string PathNormalized(string path)
        {
            return path.Replace("\\", "/");
        }

        public static List<string> GetAllAllowedFiles(string folder)
        {
            var files = new List<string>();
            foreach (var item in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
            {
                if (item.ToString().EndsWith(".meta")) 
                {
                    continue;    
                }

                if (Constants.ALLOWED_FILE_EXTENSIONS.Contains(Path.GetExtension(item).ToLower()))
                {
                    files.Add(item);
                }
            }

            return files;
        }

        public static bool IsFileAllowed(string path)
        {
            if (IsDirectory(path))
            {
                return false;
            }

            return Constants.ALLOWED_FILE_EXTENSIONS.Any(fe => path.ToLower().EndsWith(fe));
        }

        public static string EscapeJson(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            string escape = str.Replace("\\", "\\\\");
            escape = escape.Replace("\b", "\\b");
            escape = escape.Replace("\f", "\\f");
            escape = escape.Replace("\r", "\\r");
            escape = escape.Replace("\n", "\\n");
            escape = escape.Replace("\"", "\\\"");
            escape = escape.Replace("\t", "\\t");

            return escape;
        }

        public static string EscapeRegularExpression(string text)
        {
            string escapedText = text;
            escapedText = escapedText.Replace("\\", "\\\\");
            escapedText = escapedText.Replace("*", "\\*");
            escapedText = escapedText.Replace("?", "\\?");
            escapedText = escapedText.Replace("^", "\\^");
            escapedText = escapedText.Replace("$", "\\$");
            escapedText = escapedText.Replace("+", "\\+");
            escapedText = escapedText.Replace("(", "\\(");
            escapedText = escapedText.Replace(")", "\\)");
            escapedText = escapedText.Replace("[", "\\]");
            escapedText = escapedText.Replace("]", "\\]");
            escapedText = escapedText.Replace("{", "\\{");
            escapedText = escapedText.Replace("}", "\\}");
            escapedText = escapedText.Replace(".", "\\.");
            escapedText = escapedText.Replace("|", "\\|");
            return escapedText;
        }

        public static bool IsDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            try
            {
                // sometimes the path is not exists, this line will throw exception
                return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
            }
            catch
            {
                return false;
            }
        }

        public static void StarAndReview()
        {
            UnityEditorInternal.AssetStore.Open(Constants.ASSET_ID);
        }

        public static void Reveal(string folderPath)
		{
			if(!Directory.Exists(folderPath))
			{
				return;
			}

			UnityEditor.EditorUtility.RevealInFinder(folderPath);
		}

        public static string HttpGET(string url)
        {
            try
            {
                Utility.Log("Get " + url);
                var requeset = WebRequest.Create(url);
                var response = requeset.GetResponse();
                var reponseStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(reponseStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Utility.Log(e);
            }

            return string.Empty;
        }

        /// <summary>
        /// Run action is a thread.
        /// </summary>
        /// <param name="action">Action to be executed on thread</param>
        public static void RunAsyncTask(Action action)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.Start();
        }
    }
}