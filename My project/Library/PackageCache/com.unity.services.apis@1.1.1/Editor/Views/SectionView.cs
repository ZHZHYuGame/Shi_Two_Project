using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    abstract class SectionView : EditorView, ISectionView
    {
        public abstract Section Section { get; }
        public abstract string Info { get; }
        public abstract string Url { get; }

        protected StatusView StatusView { get; }
        protected ExceptionView ExceptionView { get; }
        Vector2 m_SectionScrollPos;

        public SectionView(App app) : base(app)
        {
            StatusView = new StatusView(app);
            ExceptionView = new ExceptionView(app);
        }

        public override void CreateGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H1(Section.ToString());
                UI.Flex();
                UI.Button("Learn More", () => Application.OpenURL(Url));
            }

            using (UI.InfoPanel().SetBorderRadius(0).SetBorderBottomWidth(0).Scope())
            {
                UI.Label(Info).Wrap();
            }

            StatusView.CreateGUI();

            using (UI.ScrollPanel(false).Scope())
            using (UI.ScrollContent().Scope())
            {
                CreateGUISection();
            }
            using (UI.FooterPanel().Scope())
            {
                ExceptionView.CreateGUI();
            }

            UI.HelpBox().SetMessageType(HelpBoxMessageType.Warning)
                .SetText("Enter your cloud settings to enable samples.")
                .SetFlexShrink(0)
                .BindVisibility(App.InvalidConfigurationBinding, true)
                .SetMargin(5, 0, 0, 0);
        }

        protected abstract void CreateGUISection();

        public void SetStatus(Status status, string message, Exception exception = null)
        {
            StatusView.SetStatus(status, message);
            ExceptionView.Exception = exception;
        }
    }
}
