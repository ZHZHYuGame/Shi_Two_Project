namespace Unity.Services.Apis.Sample
{
    class AboutExamplesView : EditorView
    {
        readonly SectionView SectionView;

        public AboutExamplesView(App app, SectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H4("Examples & Use Cases");
            }
            using (UI.ContentPanel().Scope())
            {
                UI.H5("Editor Tooling").SetPaddingBottom(5);
                UI.Label("Build automation, tools and interfaces to improve your workflows.").Wrap().SetPaddingBottom(20);

                UI.H5("Server Logic").SetPaddingBottom(5);
                UI.Label("Enable authoritative logic for your servers.").Wrap().SetPaddingBottom(5);
                UI.Label("Update players data, scores and more from your server.").Wrap().SetPaddingBottom(20);

                UI.H5("Game Logic").SetPaddingBottom(5);
                UI.Label("Extend your game logic with full control over APIs.").Wrap().SetPaddingBottom(20);

                UI.H5("Admin & Trusted APIs at Runtime").SetPaddingBottom(5);
                UI.Label("Use admin & trusted operations for internal support use cases.").Wrap().SetPaddingBottom(10);
            }
        }
    }
}
