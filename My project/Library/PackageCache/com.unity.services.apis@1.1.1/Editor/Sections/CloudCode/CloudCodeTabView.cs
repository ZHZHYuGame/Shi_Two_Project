using System.Collections.Generic;

namespace Unity.Services.Apis.Sample
{
    class CloudCodeTabView : TabEditorView<CloudCodeTabView.CloudCodeSection>
    {
        public enum CloudCodeSection
        {
            Modules,
            Scripts
        }

        public CloudCodeTabView(App app, CloudCodeSectionView sectionView) : base(app)
        {
            Views = new Dictionary<CloudCodeSection, EditorView>()
            {
                { CloudCodeSection.Modules, new CloudCodeModulesView(app, sectionView) },
                { CloudCodeSection.Scripts, new CloudCodeScriptsView(app, sectionView) }
            };
        }
    }
}
