using System;
using Unity.Services.Apis.Admin.AccessPolicy;

namespace Unity.Services.Apis.Sample
{
    static class PlayerStatementGUI
    {
        public static void CreateGUI(UIController UI, PlayerStatement statement)
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.Label("Statement").SetWidth(100);
                UI.SelectableLabel().BindValue(UI.BindReadOnly(() => statement.Sid));
            }
            using (UI.ContentPanel(false).Scope())
            {
                using (UI.Block().Scope())
                {
                    UI.Label("Version").SetWidth(100);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => statement._Version));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Principal").SetWidth(100);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => statement.Principal));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Effect").SetWidth(100);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => statement.Effect));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Resource").SetWidth(100);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => statement.Resource));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("ExpiresAt").SetWidth(100);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => statement.ExpiresAt.ToString()));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Actions").SetWidth(100);

                    foreach (var action in statement.Action)
                    {
                        UI.SelectableLabel().BindValue(UI.BindReadOnly(() => action));
                        UI.Space();
                    }
                }
            }
        }

        public static void DrawEdit(UIController UI, PlayerStatement statement)
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.Label("Statement").SetWidth(100);
                UI.SelectableLabel().BindValue(UI.BindReadOnly(() => statement.Sid));
            }
            using (UI.ContentPanel().Scope())
            {
                using (UI.Block().Scope())
                {
                    UI.Label($"Version").SetWidth(100);
                    UI.TextField().BindValue(UI.BindProperty(() => statement._Version));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Principal").SetWidth(100);
                    UI.TextField().BindValue(UI.BindProperty(() => statement.Principal));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Effect").SetWidth(100);
                    UI.TextField().BindValue(UI.BindProperty(() => statement.Effect));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Resource").SetWidth(100);
                    UI.TextField().BindValue(UI.BindProperty(() => statement.Resource));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("ExpiresAt").SetWidth(100);
                    UI.TextField().BindValue(
                        UI.BindTarget(
                            getter: () => statement.ExpiresAt.ToString(),
                            setter: (value) =>
                            {
                                if (DateTime.TryParse(value, out var result))
                                {
                                    statement.ExpiresAt = result;
                                }
                            }));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Actions").SetWidth(100);

                    foreach (var action in statement.Action)
                    {
                        UI.SelectableLabel().BindValue(UI.BindReadOnly(() => action));
                        UI.Space();
                    }
                }
            }
        }
    }
}
