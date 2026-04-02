namespace Unity.Services.Apis.Sample
{
    class CloudSaveSectionView : SectionView
    {
        public override Section Section => Section.CloudSave;
        public override string Url => "https://services.docs.unity.com/cloud-save-admin/";
        public override string Info => "View and manage your game and players cloud save data and files.";

        CloudSaveTabView TabView { get; }

        public CloudSaveSectionView(App app) : base(app)
        {
            TabView = new CloudSaveTabView(app, this);
        }

        protected override void CreateGUISection()
        {
            TabView.CreateGUI();
        }
    }
}
