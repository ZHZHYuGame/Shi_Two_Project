using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class ApiSamplesWindow : EditorWindow
    {
        App App { get; set; }
        ApiSamplesView View { get; set; }

        UIController UI => App.UI;

        ApiSamplesWindow()
        {
        }

        [MenuItem("Services/API Samples", priority = 10000)]
        public static void OpenSamples()
        {
            FindOrCreateWindow();
        }

        public static ApiSamplesWindow FindOrCreateWindow()
        {
            var existingWindow = Resources.FindObjectsOfTypeAll<ApiSamplesWindow>().FirstOrDefault();

            if (existingWindow != null)
            {
                existingWindow.Show();
                existingWindow.Focus();
                return existingWindow;
            }

            var window = CreateInstance<ApiSamplesWindow>();
            window.minSize = new Vector2(800, 500);
            window.Show();
            window.name = "API Samples";
            window.titleContent.text = "API Samples";
            return window;
        }

        void OnInspectorUpdate()
        {
            Repaint();
            UI.Update();
        }

        void OnEnable()
        {
            Reset();
        }

        void Reset()
        {
            App = new App();
            View = new ApiSamplesView(App);
            App.Initialize();
            View.Initialize();
        }

        void CreateGUI()
        {
            using (UI.Scope(rootVisualElement))
            {
                View.CreateGUI();
            }
        }
    }
}
