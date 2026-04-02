using System.Collections.Generic;

namespace Unity.Services.Apis.Sample
{
    class CloudSaveTabView : TabEditorView<CloudSaveTabView.CloudSaveSection>
    {
        public enum CloudSaveSection
        {
            Data,
            //Files
        }

        public CloudSaveTabView(App app, CloudSaveSectionView sectionView) : base(app)
        {
            Views = new Dictionary<CloudSaveSection, EditorView>()
            {
                { CloudSaveSection.Data, new CloudSaveDataView(app, sectionView) },
                //{ CloudSaveSection.Files, new CloudSaveFilesView(app, sectionView) },
            };
        }
    }
}
