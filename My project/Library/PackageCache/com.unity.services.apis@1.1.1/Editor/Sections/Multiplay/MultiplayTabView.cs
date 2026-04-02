#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System.Collections.Generic;

namespace Unity.Services.Apis.Sample
{
    class MultiplayTabView : TabEditorView<MultiplayTabView.MultiplaySection>
    {
        public enum MultiplaySection
        {
            Allocations,
            //BuildConfigurations,
            //Builds,
            //Fleets,
            //Providers,
            Servers
        }

        public MultiplayTabView(App app, MultiplaySectionView sectionView) : base(app)
        {
            Views = new Dictionary<MultiplaySection, EditorView>()
            {
                { MultiplaySection.Allocations, new MultiplayAllocationsView(app, sectionView) },
                //{ MultiplaySection.BuildConfigurations, new MultiplayBuildConfigurationsView(app, sectionView) },
                //{ MultiplaySection.Builds, new MultiplayBuildsView(app, sectionView) },
                //{ MultiplaySection.Fleets, new MultiplayFleetsView(app, sectionView) },
                //{ MultiplaySection.Providers, new MultiplayProvidersView(app, sectionView) },
                { MultiplaySection.Servers, new MultiplayServersView(app, sectionView) },
            };
        }
    }
}
#endif
