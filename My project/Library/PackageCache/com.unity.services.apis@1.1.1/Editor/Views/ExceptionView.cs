using System;

namespace Unity.Services.Apis.Sample
{
    class ExceptionView : EditorView
    {
        public Exception Exception { get; set; }

        public ExceptionView(App app) : base(app)
        {
        }

        public override void CreateGUI()
        {
            using (UI.ErrorPanel()
                .SetFlexGrow(1)
                .BindVisibility(UI.BindReadOnly(() => Exception != null), true)
                .Scope())
            {
                UI.SelectableLabel().BindValue(UI.BindReadOnly(() => $"{Exception?.GetType().Name}: {Exception?.Message}"));
                UI.Flex();
                UI.Button("Clear", ClearException);
            }
        }

        void ClearException()
        {
            Exception = null;
        }
    }
}
