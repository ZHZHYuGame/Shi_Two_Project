using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class ApiSamplesView : EditorView
    {
        const string SectionKey = "ApiSamples.Tab";

        readonly string[] m_Sections = Enum.GetNames(typeof(Section));

        bool ShowMenu = true;
        Section m_Section;
        readonly IUIBinding<Section> SelectionBinding;

        Dictionary<Section, EditorView> Views { get; } = new Dictionary<Section, EditorView>();
        Vector2 m_ToolbarScrollPos;

        public ApiSamplesView(App app) : base(app)
        {
            Views = new Dictionary<Section, EditorView>()
            {
                { Section.About, new AboutSectionView(app) },
                { Section.Settings, new SettingsSectionView(app) },
                { Section.Authorization, new AuthorizationSectionView(app) },
                { Section.AccessPolicy, new AccessPolicySectionView(app) },
                { Section.Accounts, new AccountsSectionView(app) },
                { Section.CloudCode, new CloudCodeSectionView(app) },
                { Section.CloudSave, new CloudSaveSectionView(app) },
                { Section.Economy, new EconomySectionView(app) },
                { Section.Environment, new EnvironmentSectionView(app) },
                { Section.Leaderboards, new LeaderboardsSectionView(app) },
                { Section.Lobbies, new LobbiesSectionView(app) },
                { Section.PlayerNames, new PlayerNamesSectionView(app) },
                { Section.RemoteConfig, new RemoteConfigSectionView(app) },

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
                { Section.Matchmaker, new MatchmakerSectionView(app) },
                { Section.Multiplay, new MultiplaySectionView(app) },
                { Section.UGC, new UGCSectionView(app) },
#endif
            };

            SelectionBinding = UI.BindField(this, () => m_Section);
        }

        void OnTabChange(Section section)
        {
            EditorPrefs.SetInt(SectionKey, (int)m_Section);
        }

        public override void Initialize()
        {
            foreach (var view in Views.Values)
            {
                view.Initialize();
            }

            m_Section = (Section)EditorPrefs.GetInt(SectionKey, 0);
        }

        public override void CreateGUI()
        {
            using (UI.TitlePanel().Scope())
            {
                UI.H1("Unity Services API Samples").SetFlexGrow(1).SetTextAlign(TextAnchor.MiddleCenter);
            }

            using (UI.ContentPanel(true).SetHeightPercent(100).Scope())
            using (UI.HorizontalLayout().Scope())
            {
                CreateGUIMenu();
                CreateSectionViews();
            }
        }

        void CreateGUIHeader()
        {
        }

        void CreateGUIMenu()
        {
            var binding = UI.BindReadOnly(() => ShowMenu);
            using (UI.VerticalLayout().BindVisibility(binding, true).SetMinWidth(200).SetMaxWidth(200).SetPaddingRight(5).Scope())
            {
                using (UI.HeaderPanel().Scope())
                {
                    UI.H1("Samples");
                }
                using (UI.ScrollPanel().Scope())
                using (UI.ScrollContent().Scope())
                {
                    UI.SelectionGrid<Section>()
                        .BindValue(SelectionBinding)
                        .RegisterSelectionCallback(OnTabChange)
                        .SetVertical();
                }
            }
        }

        void CreateSectionViews()
        {
            using (UI.VerticalLayout().SetPaddingLeft(5).Expand().Scope())
            {
                foreach (var viewKvp in Views)
                {
                    var sectionBinding = UI.BindReadOnly(() => m_Section == viewKvp.Key);

                    using (UI.VerticalElement()
                           .SetName($"{viewKvp.Key}Layout")
                           .BindVisibility(sectionBinding, true)
                           .Expand()
                           .Scope())
                    {
                        viewKvp.Value.CreateGUI();
                    }
                }
            }
        }

        void ToggleMenu()
        {
            ShowMenu = !ShowMenu;
        }

        void HideMenu()
        {
            ShowMenu = false;
        }
    }
}
