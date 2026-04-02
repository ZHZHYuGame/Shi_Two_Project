using System.Collections.Generic;
using UnityEditor;

namespace Unity.Services.Apis.Sample
{
    class PolicyTabView : TabEditorView<PolicyTabView.PolicySection>
    {
        const string SectionKey = "UGS.Admin.Policy.Tab";

        public enum PolicySection
        {
            Player,
            Project
        }

        public PolicyTabView(App app, AccessPolicySectionView sectionView) : base(app)
        {
            Views = new Dictionary<PolicySection, EditorView>()
            {
                { PolicySection.Player, new PlayerPolicyView(app, sectionView) },
                { PolicySection.Project, new ProjectPolicyView(app, sectionView) },
            };

            Section = (PolicySection)EditorPrefs.GetInt(SectionKey, 0);
            TabChange += OnTabChange;
        }

        void OnTabChange(PolicySection section)
        {
            EditorPrefs.SetInt(SectionKey, (int)section);
        }
    }
}
