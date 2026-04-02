#if ENABLE_SERVICES_EXPERIMENTAL_APIS

namespace Unity.Services.Apis.Sample
{
    class UGCSectionView : SectionView
    {
        public override Section Section => Section.UGC;
        public override string Url => "https://services.docs.unity.com/user-generated-content-admin";
        public override string Info => "Manage content, moderators, audits, tags, etc.";

        UGCController UGC => App.UGC;

        UGCTabView TabView { get; }

        public UGCSectionView(App app) : base(app)
        {
            TabView = new UGCTabView(app, this);
        }

        protected override void CreateGUISection()
        {
            TabView.CreateGUI();
        }
    }
}
#endif
