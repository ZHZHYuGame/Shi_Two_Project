namespace Unity.Services.Apis.Sample
{
    class AboutResponsesView : EditorView
    {
        readonly SectionView SectionView;

        public AboutResponsesView(App app, SectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H4("Response Handling");
            }
            using (UI.ContentPanel().Scope())
            {
                UI.Label("Operations do not throw exceptions by default.").Wrap().SetPaddingBottom(10);

                UI.Label("You can check the state of an operation's response with <b>IsSuccessful</b>.")
                    .Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.ResponseIsSuccessful).SetPaddingBottom(10);

                UI.Label("You can trigger an <b>ApiException</b> in case of errors by using <b>EnsureSuccessful()</b> after completion.")
                    .Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.ResponseEnsureSuccessful).SetPaddingBottom(10);

                
            }
        }
    }
}
