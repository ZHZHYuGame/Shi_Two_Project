using System.Collections.Generic;

namespace Unity.Services.Apis.Sample
{
    class AboutTabView : TabEditorView<AboutTabView.AboutSection>
    {
        public enum AboutSection
        {
            Clients,
            Operations,
            Responses,
            Examples,
        }

        public AboutTabView(App app, AboutSectionView sectionView) : base(app)
        {
            Views = new Dictionary<AboutSection, EditorView>()
            {
                { AboutSection.Clients, new AboutClientsView(app, sectionView) },
                { AboutSection.Operations, new AboutOperationsView(app, sectionView) },
                { AboutSection.Responses, new AboutResponsesView(app, sectionView) },
                { AboutSection.Examples, new AboutExamplesView(app, sectionView) },
            };
        }
    }
}
