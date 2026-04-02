#if ENABLE_SERVICES_EXPERIMENTAL_APIS
namespace Unity.Services.Apis.Sample
{
    class MultiplayBuildConfigurationsView : EditorView
    {
        MultiplayController Multiplay => App.Multiplay;

        MultiplaySectionView SectionView;

        public MultiplayBuildConfigurationsView(App app, MultiplaySectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
        }
    }
}
#endif
