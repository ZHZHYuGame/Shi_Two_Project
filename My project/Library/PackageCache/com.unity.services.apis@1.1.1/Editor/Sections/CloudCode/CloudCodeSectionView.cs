namespace Unity.Services.Apis.Sample
{
    class CloudCodeSectionView : SectionView
    {
        public override Section Section => Section.CloudCode;
        public override string Url => "https://services.docs.unity.com/cloud-code-admin/";
        public override string Info => "Manage cloud code modules and scripts...";

        CloudCodeController CloudCode => App.CloudCode;

        CloudCodeTabView TabView { get; }

        public CloudCodeSectionView(App app) : base(app)
        {
            TabView = new CloudCodeTabView(app, this);
        }

        protected override void CreateGUISection()
        {
            TabView.CreateGUI();
        }
    }
}
