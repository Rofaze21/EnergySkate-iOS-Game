//  Copyright (c) 2018-present amlovey
//  
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Callbacks;
using System.Linq;
using System.IO;

namespace uCodeEditor
{
    public class MainWindow : EditorWindow
    {
        private Webview webview;
        private const string Title = "uCodeEditor";

        public bool active;
        public int id;

        public WebviewComService CommunicateServices;
        public static MainWindow LastActiveInstance
        {
            get
            {
                var allMainWindows = GetAllInstances();
                if (allMainWindows == null)
                {
                    return null;
                }

                return allMainWindows.FirstOrDefault(window => window.active);
            }
        }

        public static int PrecastNewId()
        {
            var instances = GetAllInstances();
            if (instances == null)
            {
                return 1;
            }

            return instances.Length + 1;
        }

        public static bool CanExecuteCommunicateServices()
        {
            return LastActiveInstance != null && LastActiveInstance.CommunicateServices != null;
        }

        public static MainWindow[] GetAllInstances()
        {
            return Resources.FindObjectsOfTypeAll<MainWindow>();
        }

        public static bool HasInstances()
        {
            var allMainWindows = GetAllInstances();
            if (allMainWindows == null || allMainWindows.Length == 0)
            {
                return false;
            }

            return true;
        }

        [UnityEditor.MenuItem("Tools/uCodeEditor/Online Documentation", false, 33)]
        public static void OpenOnlineDocumentation()
        {
            Application.OpenURL("http://www.amlovey.com/uce/index/");
        }

        [UnityEditor.MenuItem("Tools/uCodeEditor/Open An Issue", false, 33)]
        public static void OpenIssue()
        {
            Application.OpenURL("https://github.com/amloveyweb/amloveyweb.github.io/issues");
        }

        [UnityEditor.MenuItem("Tools/uCodeEditor/Star And Review", false, 33)]
        public static void StarAndReview()
        {
            Utility.StarAndReview();
        }

        [UnityEditor.MenuItem("Tools/uCodeEditor/uCodeEditor %#e", false, 11)]
        private static void OpenEditorWindowFromMenu()
        {
            LoadWindow();
        }

        public static void CreateNewWindowWithSettingCopy()
        {
            var nextId = PrecastNewId();
            if (nextId > 1)
            {
                var newWindowLocalSettingFile = LocalSettings.GetLocalSettingsPath(nextId);
                if (LastActiveInstance != null)
                {
                    var lastId = LastActiveInstance.id;
                    var lastWindowLocalSettinsFile = LocalSettings.GetLocalSettingsPath(lastId);
                    File.Copy(lastWindowLocalSettinsFile, newWindowLocalSettingFile, true);
                }
            }

            CreateNewWindow();
        }

        public static void CreateNewWindow()
        {
            MainWindow instance;
            if (!HasInstances())
            {
                int id = 1;
                Type[] desiredDockNextTo = new Type[] { typeof(SceneView) };
                instance = EditorWindow.GetWindow<MainWindow>(Title, desiredDockNextTo);
                instance.id = id;
                instance.active = true;
            }
            else
            {
                var id = PrecastNewId();
                instance = EditorWindow.CreateInstance<MainWindow>();
                instance.id = id;
                instance.titleContent = new GUIContent(string.Format("{0} ({1})", Title, id - 1));
            }

            if (instance != null)
            {
                instance.Show();
                instance.Focus();
            }
        }

        public static void LoadWindow()
        {
            if (LastActiveInstance == null)
            {
                CreateNewWindow();
            }
            else
            {
                LastActiveInstance.Show();
                LastActiveInstance.Focus();
            }
        }

        public void InitWebView(Rect webviewRect)
        {
            if (webview == null)
            {
                this.webview = ScriptableObject.CreateInstance<Webview>();
                this.webview.hideFlags = HideFlags.HideAndDontSave;
            }

            this.webview.InitWebView(Webview.GetView(this), webviewRect, false);
            var path = PathManager.GetIndexHTMLPath();
#if uCE_DEV
            this.webview.AllowRightClickMenu(true);
#endif
            EditorCoroutine.StartCoroutine(()=>{
                InitWebviewComService();
                this.webview.LoadURL(path);
                SetFocus(true);
            });
        }

        public void Reload()
        {
            var path = PathManager.GetIndexHTMLPath();
            this.webview.LoadURL(path);
        }

        public void InitWebviewComService()
        {
            CommunicateServices = ScriptableObject.CreateInstance<WebviewComService>();
            CommunicateServices.hideFlags = HideFlags.HideAndDontSave;
            CommunicateServices.Init(this.webview, this.id);
        }

        public void OnAddedAsTab()
        {
            // Make sure the parent window is set correctly for 
            // CEF if this instance is being moved from a different DockArea.
            SetFocus(true);
        }

        public void OnBeforeRemovedAsTab()
        {
            // Set the CEF parent window as if the tab were becoming invisible prior to tab removal. 
            // This will keep CEF from cleaning up the browser window if the window is being dragged
            //  to a new DockArea and was the last tab in it's old DockArea.
            OnBecameInvisible();
        }

        public void OnBecameInvisible()
        {
            if (this.webview != null)
            {
                this.webview.SetHostView(null);
                this.webview.Hide();
                this.webview.SetFocus(false);
            }
        }

        void OnDestroy()
        {
            DestroyImmediate(this.webview);
        }

        public void OnLostFocus()
        {
            this.SetFocus(false);
        }

        private void RebindWebViewIfNeeded()
        {
            if (CommunicateServices == null)
            {
                InitWebviewComService();
            }
        }

        public void OnFocus()
        {
            SetFocus(true);
            RebindWebViewIfNeeded();
        }

        public void OnEnable()
        {
            RebindWebViewIfNeeded();
        }

        private int repeatedShow;
        private bool syncingFocus;
        private void SetFocus(bool value)
        {
            if (!this.syncingFocus)
            {
                this.syncingFocus = true;
                if (this.webview != null)
                {
                    if (value)
                    {
                        this.webview.SetHostView(Webview.GetView(this));
                        this.webview.Show();
                        this.repeatedShow = 5;
                    }

                    this.webview.SetFocus(value);
                }
                this.syncingFocus = false;
            }
        }

        public void Refresh()
        {
            this.webview.Hide();
            this.webview.Show();
        }

        void OnGUI()
        {
            Rect webViewRect = GUIClip.Unclip(new Rect(0f, 0, base.position.width, base.position.height));

            if (this.webview == null)
            {
                this.InitWebView(webViewRect);
            }

            RebindWebViewIfNeeded();

            if (this.repeatedShow-- > 0)
            {
                this.Refresh();
            }

            if (Event.current.type == EventType.Repaint && webview != null)
            {
                this.webview.SetHostView(Webview.GetView(this));
                this.webview.SetSizeAndPosition(webViewRect);
            }
        }
    }
}