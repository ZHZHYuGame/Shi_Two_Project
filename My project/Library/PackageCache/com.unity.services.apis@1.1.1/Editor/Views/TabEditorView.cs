using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    abstract class TabEditorView<T> : EditorView where T : Enum
    {
        public event Action<T> TabChange;

        protected Dictionary<T, EditorView> Views { get; set; }

        readonly string[] m_Sections = Enum.GetNames(typeof(T));
        readonly List<T> m_Values = Enum.GetValues(typeof(T)).Cast<T>().ToList();

        public T Section;
        Vector2 m_ToolbarScrollPos;
        Vector2 m_SectionScrollPos;
        readonly IUIBinding<T> SelectionBinding;

        public TabEditorView(App app) : base(app)
        {
            SelectionBinding = UI.BindTarget(() => Section, (value) => Section = value);
        }

        public override void CreateGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.SelectionGrid<T>()
                    .BindValue(SelectionBinding)
                    .RegisterSelectionCallback(TabChange)
                    .SetHorizontal();
            }
            using (UI.ScrollPanel().Scope())
            using (UI.ScrollContent().Scope())
            {
                foreach (var viewKvp in Views)
                {
                    var sectionBinding = UI.BindReadOnly(() => Section.Equals(viewKvp.Key));

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
    }
}
