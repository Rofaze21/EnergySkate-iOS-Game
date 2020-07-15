//  Copyright (c) 2018-present amlovey
//  
using System.Collections.Generic;
using UnityEditor;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace uCodeEditor
{
    public class SearchResult
    {
        public string Path { get; set; }
        public List<SearchResultRange> Ranges { get; set; }

        public SearchResult()
        {
            Ranges = new List<SearchResultRange>();
        }

        public override string ToString()
        {
            var ranges = Ranges.Select(r => r.ToString()).ToArray();
            return string.Format("{{\"Path\":\"{0}\",\"Ranges\":[{1}]}}", Utility.PathNormalized(Path), string.Join(",", ranges));
        }
    }

    public class SearchResultRange
    {
        public int Line { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public string LineText { get; set; }

        public override string ToString()
        {
            return string.Format("{{\"Text\":\"{0}\",\"Line\":{1},\"StartColumn\":{2},\"EndColumn\":{3}}}", Utility.EscapeJson(LineText), Line, StartColumn, EndColumn);
        }
    }

    public class FileSearch
    {
        private static object _lockObject = new object();
        private static List<Thread> threads = new List<Thread>();
        private static List<SearchResult> searchResults = new List<SearchResult>();
        private static string[] files;
        private const int THREAD_COUNT = 1;
        private static int index = 0;
        private static string searchPattern;
        private static RegexOptions searchRegexOptions = RegexOptions.None;
        private static float REPORT_PROGRESS_INTERVAL = 0.1f;
        private static double time = 0;
        private static double timeForProgress = 0;
        private static bool searching = false;

        public static void Update()
        {
            if (!searching)
            {
                return;
            }

            if (EditorApplication.timeSinceStartup - timeForProgress > REPORT_PROGRESS_INTERVAL)
            {
                SetSearchProgress();
                timeForProgress = EditorApplication.timeSinceStartup;
            }

            if (IsSearchCompleted())
            {
                SetSearchProgress();
                SendResult(searchResults);
                searching = false;
            }
        }

        private static void SetSearchProgress()
        {
            if (files.Length >= 0)
            {
                if (MainWindow.CanExecuteCommunicateServices())
                {
                    MainWindow.LastActiveInstance.CommunicateServices.SetSearchProgress(index * 1.0f / files.Length);
                }
            }
        }

        public static void Search(string searchText, bool ignoreCase, bool useRegex, string[] searchFiles)
        {
            if (string.IsNullOrEmpty(searchText) || searchFiles == null || searchFiles.Length == 0)
            {
                return;
            }

            if (threads == null)
            {
                threads = new List<Thread>();
            }

            Stop();

            // If searchText stars with | and is regular express, it means
            // match everything, we don't want to handle this huge results.
            if (useRegex && searchText.Trim().StartsWith("|"))
            {
                EditorUtility.DisplayDialog("Error", "Expression matches everything.", "Ok");
                return;
            }

            threads.Clear();
            index = 0;
            files = FilterFiles(searchFiles);

            searchRegexOptions = ignoreCase ? RegexOptions.IgnoreCase | RegexOptions.Singleline : RegexOptions.Singleline;
            searchPattern = useRegex ? searchText : Utility.EscapeRegularExpression(searchText);

            searchResults.Clear();
            time = EditorApplication.timeSinceStartup;
            timeForProgress = time;
            searching = true;
            SetSearchProgress();

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                threads.Add(new Thread(new ThreadStart(SearchInternal)));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        private static void SearchInternal()
        {
            while (files.Length >= 0 && index < files.Length)
            {
                string filePath;
                lock (_lockObject)
                {
                    filePath = files[index];
                    index++;
                }

                try
                {
                    GetAndAddResultsIfNeeded(filePath);
                }
                catch
                {

                }
            }
        }

        private static void GetAndAddResultsIfNeeded(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    SearchResult result = new SearchResult();
                    result.Path = filePath;

                    int lineNumber = 0;
                    string line = reader.ReadLine();

                    while (line != null)
                    {
                        lineNumber++;

                        foreach (Match item in Regex.Matches(line, searchPattern, searchRegexOptions))
                        {
                            SearchResultRange range = new SearchResultRange();

                            range.LineText = line;
                            range.Line = lineNumber;
                            range.StartColumn = item.Index + 1;
                            range.EndColumn = range.StartColumn + item.Length;
                            result.Ranges.Add(range);
                        }

                        line = reader.ReadLine();
                    }

                    if (result.Ranges.Count > 0)
                    {
                        AddToResults(result);
                    }
                }
            }
        }

        private static string[] FilterFiles(string[] filesToFilter)
        {
            return filesToFilter.Where(file => !IsExcludeFile(Utility.PathNormalized(file))).ToArray();
        }

        private static bool IsExcludeFile(string file)
        {
            return file.Contains("uCodeEditor/Editor/index.html")
                 || file.Contains("uCodeEditor/Editor/bundle.jsx")
                 || file.Contains("uCodeEditor/Editor/libs");
        }

        private static void AddToResults(SearchResult result)
        {
            lock (_lockObject)
            {
                if (result == null || result.Ranges == null || result.Ranges.Count == 0)
                {
                    return;
                }

                searchResults.Add(result);
            }
        }

        private static bool IsSearchCompleted()
        {
            if (threads == null || threads.Count() <= 0)
            {
                return true;
            }

            for (int i = 0; i < threads.Count(); i++)
            {
                if (threads[i].IsAlive)
                {
                    return false;
                }
            }

            return true;
        }

        private static void SendResult(List<SearchResult> results)
        {
            if (MainWindow.CanExecuteCommunicateServices())
            {
                var resultsJson = string.Format("[{0}]", string.Join(",", results.ToArray().Select(r => r.ToString()).ToArray()));
                MainWindow.LastActiveInstance.CommunicateServices.SendSearchResult(resultsJson);
            }
        }

        public static void Stop()
        {
            if (threads != null)
            {
                foreach (Thread thread in threads)
                {
                    thread.Abort();
                }
            }
        }
    }
}
