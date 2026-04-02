using System.Collections;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class AboutOperationsView : EditorView
    {
        readonly SectionView SectionView;

        public AboutOperationsView(App app, SectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H4("Operation Handling");
            }
            using (UI.ContentPanel().Scope())
            {
                UI.Label("This package lets you choose how you want to handle operations.").Wrap().SetPaddingBottom(10);

                UI.Label("<b>Tasks</b>\nUse an async method with the await operator.").Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.OperationAwait).SetPaddingBottom(10);

                UI.Label("<b>Events</b>\nRegister a callback invoked upon completion.").Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.OperationEvents).SetPaddingBottom(10);

                UI.Label("<b>Coroutines</b>\nYield until the operation completes.").Wrap().SetPaddingBottom(5);
                UI.Snippet().SetText(Snippets.OperationCoroutine).SetPaddingBottom(10);
            }
        }
    }
}
