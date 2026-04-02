namespace Unity.Services.Apis.Sample
{
    class CloudSaveFilesView : EditorView
    {
        CloudSaveController CloudSave => App.CloudSave;

        readonly CloudSaveSectionView SectionView;

        //string m_LoadFileIdInput = "";
        //string m_CreateFileIdInput = "";
        //string m_CreateFileContentInput = "";

        public CloudSaveFilesView(App app, CloudSaveSectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
            using (UI.WarningPanel().Scope())
            {
                UI.Label("Work in progress");
            }
            /*
            using (UI.Highlight().Scope())
            {
                UI.Label("Load File").SetWidth(150);
                UI.TextField().BindValue(UI.BindField(this, () => m_LoadFileIdInput).SetWidth(300);
                UI.Button("Load", LoadFileAsync).SetWidth(100);
            }

            using (UI.Highlight().Scope())
            {
                UI.Label("Create File").SetWidth(150);
                UI.TextField().BindValue(UI.BindField(this, () => m_CreateFileIdInput).SetWidth(300);
                UI.Button("Create", CreateFileAsync).SetWidth(100);
            }
            */
        }

        /*
        void LoadFileAsync()
        {
            if (!string.IsNullOrEmpty(m_LoadFileIdInput))
            {
                CloudSave.LoadFileAsync(m_LoadFileIdInput);
            }

            m_LoadFileIdInput = "";
        }

        void CreateFileAsync()
        {
            if (!string.IsNullOrEmpty(m_LoadFileIdInput))
            {
                CloudSave.CreateFileAsync(m_LoadFileIdInput);
            }

            m_LoadFileIdInput = "";
        }

        void SetStatus(Status status, string message, Exception exception = null)
        {
            SectionView.SetStatus(status, message, exception);
        }
        */
    }
}
