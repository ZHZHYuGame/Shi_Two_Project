namespace Unity.Services.Apis.Sample
{
    class AboutClientsView : EditorView
    {
        readonly SectionView SectionView;

        public AboutClientsView(App app, SectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H4("Clients");
            }
            using (UI.ContentPanel().Scope())
            {
                UI.Label("Clients wrap features based on the type of authentication and intent to simplify use.").Wrap().SetPaddingBottom(10);

                UI.H5("Game Client").SetPaddingBottom(5);
                UI.Label("Perform game operations as a player.").Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.GameClient).SetPaddingBottom(10);

                UI.H5("Server Client").SetPaddingBottom(5);
                UI.Label("Perform game operations as a server").Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.ServerClient).SetPaddingBottom(10);

                UI.H5("Trusted Client").SetPaddingBottom(5);
                UI.Label("Perform game operations as a trusted authority.").Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.TrustedClient).SetPaddingBottom(10);

                UI.H5("Admin Client").SetPaddingBottom(5);
                UI.Label("Perform admin operations as a trusted authority.").Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.AdminClient);
            }
        }
    }
}
