using System.Collections.Generic;

namespace Unity.Services.Apis.Sample
{
    class EconomyTabView : TabEditorView<EconomyTabView.EconomySection>
    {
        public enum EconomySection
        {
            Configuration,
            Player
        }

        public EconomyTabView(App app, EconomySectionView sectionView) : base(app)
        {
            Views = new Dictionary<EconomySection, EditorView>()
            {
                { EconomySection.Configuration, new EconomyConfigurationView(app, sectionView) },
                { EconomySection.Player, new EconomyPlayerView(app, sectionView) },
            };
        }
    }
}
