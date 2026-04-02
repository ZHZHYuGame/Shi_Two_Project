using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class StatusView : EditorView
    {
        public Status Status { get; private set; }
        public string Message { get; private set; }

        UIElement Panel { get; set; }

        public StatusView(App app) : base(app)
        {
        }

        public override void CreateGUI()
        {
            Panel = UI.HorizontalPanel()
                .SetBorderRadius(1)
                .SetPadding(1, 10, 1, 10)
                .SetFlexShrink(0)
                .BindVisibility(UI.BindReadOnly(() => Status != Status.None), true);

            using (Panel.Scope())
            {
                UI.Label().SetTextAlign(TextAnchor.MiddleLeft).BindValue(UI.BindReadOnly(() => Message));
                UI.Flex();
                UI.Button("X", Clear).SetFontSize(10).SetWidth(25);
            }
        }

        public void SetStatus(Status status, string message)
        {
            Status = status;
            Message = message;

            switch (Status)
            {
                case Status.Error:
                    Panel.SetBackgroundColor(UI.ErrorBackground);
                    break;
                case Status.Success:
                    Panel.SetBackgroundColor(UI.SuccessBackground);
                    break;
                case Status.Warning:
                    Panel.SetBackgroundColor(UI.WarningBackground);
                    break;
            }
        }

        public void Clear()
        {
            Status = Status.None;
            Message = null;
        }
    }
}
