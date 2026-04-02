#if ENABLE_SERVICES_EXPERIMENTAL_APIS
namespace Unity.Services.Apis.Sample
{
    class MultiplayBuildsView : EditorView
    {
        MultiplayController Multiplay => App.Multiplay;

        MultiplaySectionView SectionView;

        public MultiplayBuildsView(App app, MultiplaySectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
        }
    }
}
#endif
