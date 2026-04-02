#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class UGCTagsView : EditorView
    {
        UGCController UGC => App.UGC;
        UGCSectionView SectionView { get; }

        UIElement Container;
        string m_CreateTagInput;

        public UGCTagsView(App app, UGCSectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            UGC.TagsChanged += RefreshTags;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.UGCTagsAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                using (UI.Block()
                .SetName("CreateTagContainer")
                .SetMarginBottom(10)
                .Scope())
                {
                    UI.Label("Create Tag").SetWidth(100);
                    UI.TextField().BindValue(UI.BindField(this, () => m_CreateTagInput)).SetWidth(200);
                    UI.Button("Add", () => CreateTagAsync(m_CreateTagInput)).SetWidth(100);
                }

                using (UI.HeaderPanel().Scope())
                {
                    UI.H2("Tags");
                    UI.Flex();

                    if (UGC.Tags != null)
                    {
                        UI.Label($"{UGC.Tags.Count} tags");
                    }
                }
                using (UI.HorizontalPanel().SetBorderRadius(0).Scope())
                {
                    UI.H5("Id").SetWidth(300);
                    UI.H5("Name").SetWidth(150);
                    UI.H5("Actions").SetWidth(100);
                }

                Container = UI.VerticalPanel().SetBorderRadius(0).SetPadding(0);

                using (UI.FooterPanel().Scope())
                {
                    UI.Button("Load", LoadTagsAsync).SetWidth(50);
                    UI.Flex();
                    UI.Button("Clear", ClearTags).SetWidth(50);
                }
            }
        }

        void RefreshTags()
        {
            Container.Clear();

            using (Container.Scope())
            {
                if (UGC.Tags != null)
                {
                    for (var i = 0; i != UGC.Tags.Count; ++i)
                    {
                        var tag = UGC.Tags[i];

                        using (UI.HorizontalPanel()
                            .SetBorderRadius(0)
                            .SetBorderBottomWidth(0)
                            .SetBackgroundColor(i % 2 == 0 ? Color.clear : new Color(1f, 1f, 1f, 0.05f))
                            .SetPadding(6)
                            .Scope())
                        {
                            UI.Label(tag.Id).SetWidth(300);
                            UI.Label(tag.Name).SetWidth(150);
                            UI.Button("Delete", () => DeleteTagAsync(tag.Id)).SetWidth(100);
                        }
                    }
                }
                else
                {
                    UI.Label("-");
                }
            }
        }

        public async Task CreateTagAsync(string tagName)
        {
            try
            {
                SectionView.SetStatus(Status.Info, "CreateTag started.");
                await UGC.CreateTagAsync(tagName);
                SectionView.SetStatus(Status.Success, "CreateTag successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "CreateTag failed.");
            }
        }

        public async Task DeleteTagAsync(string tagId)
        {
            try
            {
                SectionView.SetStatus(Status.Info, "DeleteTag started.");
                await UGC.DeleteTagAsync(tagId);
                SectionView.SetStatus(Status.Success, "DeleteTag successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "DeleteTag failed.");
            }
        }

        public async Task LoadTagsAsync()
        {
            try
            {
                SectionView.SetStatus(Status.Info, "LoadTags started.");
                await UGC.LoadTagsAsync();
                SectionView.SetStatus(Status.Success, "LoadTags successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "LoadTags failed.");
            }
        }

        void ClearTags()
        {
            try
            {
                SectionView.SetStatus(Status.Info, "ClearTags started.");
                UGC.ClearTags();
                SectionView.SetStatus(Status.Success, "ClearTags successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "ClearTags failed.");
            }
        }
    }
}
#endif
