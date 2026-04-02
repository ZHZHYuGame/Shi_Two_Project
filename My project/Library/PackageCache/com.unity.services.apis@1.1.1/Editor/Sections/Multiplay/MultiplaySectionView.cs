#if ENABLE_SERVICES_EXPERIMENTAL_APIS
namespace Unity.Services.Apis.Sample
{
    class MultiplaySectionView : SectionView
    {
        public override Section Section => Section.Multiplay;
        public override string Url => "https://services.docs.unity.com/multiplay-config";
        public override string Info => "Manage your configuration, view allocations, etc.";

        MultiplayTabView TabView { get; }

        public MultiplaySectionView(App app) : base(app)
        {
            TabView = new MultiplayTabView(app, this);
        }

        protected override void CreateGUISection()
        {
            TabView.CreateGUI();
        }
    }
}
#endif
