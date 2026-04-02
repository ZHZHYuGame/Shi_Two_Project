#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System.Collections.Generic;
using UnityEditor;

namespace Unity.Services.Apis.Sample
{
    class UGCTabView : TabEditorView<UGCTabView.UGCSection>
    {
        const string SectionKey = "UGS.Admin.UGC.Tab";

        public enum UGCSection
        {
            //Moderators,
            //Audit,
            Tags
        }

        public UGCTabView(App app, UGCSectionView sectionView) : base(app)
        {
            Views = new Dictionary<UGCSection, EditorView>()
            {
                //{ UGCSection.Moderators, new UGCModeratorsView(app, sectionView) },
                //{ UGCSection.Audit, new UGCAuditView(app, sectionView) },
                { UGCSection.Tags, new UGCTagsView(app, sectionView) },
            };

            Section = (UGCSection)EditorPrefs.GetInt(SectionKey, 0);
            TabChange += OnTabChange;
        }

        void OnTabChange(UGCSection section)
        {
            EditorPrefs.SetInt(SectionKey, (int)section);
        }
    }
}
#endif
