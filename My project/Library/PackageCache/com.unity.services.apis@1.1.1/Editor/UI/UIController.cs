using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class UIController : UIEditorService
    {
        public readonly Color TitleBackground = new Color(0f, 0f, 0f, 0.2f);
        public readonly Color HeaderBackground = new Color(0f, 0f, 0f, 0.1f);
        public readonly Color PanelBackground = new Color(0.4f, 0.4f, 0.4f, 0.05f);
        public readonly Color FooterBackground = new Color(0f, 0f, 0f, 0.05f);
        public readonly Color PanelBorder = new Color(0.4f, 0.4f, 0.4f, 0.5f);

        public readonly Color ErrorBackground = new Color(0.5f, 0.1f, 0.1f, 0.3f);
        public readonly Color SuccessBackground = new Color(0f, 0.5f, 0.2f, 0.3f);
        public readonly Color InfoBackground = new Color(0.2f, 0.2f, 0.5f, 0.3f);
        public readonly Color WarningBackground = new Color(0.5f, 0.5f, 0.1f, 0.3f);
        public readonly Color HighlightBackground = new Color(0f, 0.3f, 0.3f, 0.3f);

        public readonly Color BlockBackground = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        public readonly Color BlockHoverBackground = new Color(0.5f, 0.5f, 0.5f, 0.15f);
        public readonly Color BlockBorder = new Color(0.3f, 0.3f, 0.3f, 0.2f);

        public UIController()
        {
        }

        public UIButton Button(string text, Action action)
        {
            return Button()
                .SetText(text)
                .SetClickCallback(action);
        }

        public UIButton Button(string text, Func<Task> action)
        {
            return Button()
                .SetText(text)
                .SetClickCallback(action);
        }

        public override UILabel Label()
        {
            return base.Label()
                .SetName("Label")
                .SetWhitespace(WhiteSpace.Normal);
        }

        public UILabel Label(string text)
        {
            return Label()
                .SetText(text);
        }

        public override UISelectableLabel SelectableLabel()
        {
            return base.SelectableLabel()
                .SetName("SelectableLabel")
                .SetWhitespace(WhiteSpace.Normal);
        }

        public UISelectableLabel SelectableLabel(string text)
        {
            return SelectableLabel()
                .SetName("SelectableLabel")
                .SetText(text);
        }

        public UILabel H1(string text)
        {
            return Label()
                .SetName("H1")
                .SetText(text)
                .SetFontStyle(FontStyle.Bold)
                .SetFontSize(18);
        }

        public UILabel H2(string text)
        {
            return Label()
                .SetName("H2")
                .SetText(text)
                .SetFontStyle(FontStyle.Bold)
                .SetFontSize(17);
        }

        public UILabel H3(string text)
        {
            return Label()
                .SetName("H3")
                .SetText(text)
                .SetFontStyle(FontStyle.Bold)
                .SetFontSize(16);
        }

        public UILabel H4(string text)
        {
            return Label()
                .SetName("H4")
                .SetText(text)
                .SetFontStyle(FontStyle.Bold)
                .SetFontSize(15);
        }

        public UILabel H5(string text)
        {
            return Label()
                .SetName("H5")
                .SetText(text)
                .SetFontStyle(FontStyle.Bold)
                .SetFontSize(14);
        }

        public UIElement Separator(Color? color = null)
        {
            return Element()
                .SetName("Separator")
                .SetBorderTopWidth(2)
                .SetBorderTopColor(color.HasValue ? color.Value : new Color(0.5f, 0.5f, 0.5f, 1f))
                .SetMargin(5, 0, 5, 0);
        }

        public UIElement Space()
        {
            return Element()
                .SetName("Space")
                .SetPadding(5, 5, 5, 5);
        }

        public UIElement Space(StyleLength length)
        {
            return Element()
                .SetName("Space")
                .SetPadding(length, length, length, length);
        }

        public UIElement VerticalSpace(StyleLength length)
        {
            return Element()
                .SetName("VerticalSpace")
                .SetPadding(length, 0, length, 0);
        }

        public UIElement HorizontalSpace(StyleLength length)
        {
            return Element()
                .SetName("HorizontalSpace")
                .SetPadding(0, length, 0, length);
        }

        public UIElement Flex()
        {
            return Element()
                .SetName("Flex")
                .SetFlexShrink(1f)
                .SetFlexGrow(1f)
                .SetPadding(5, 5, 5, 5);
        }

        public UIElement HorizontalElement()
        {
            return Element()
                .SetName("HorizontalElement")
                .SetFlexDirection(FlexDirection.Row);
        }

        public UIElement HorizontalLayout()
        {
            return HorizontalElement().Expand();
        }

        public UIElement VerticalElement()
        {
            return Element()
                .SetName("VerticalElement")
                .SetFlexDirection(FlexDirection.Column);
        }

        public UIElement VerticalLayout()
        {
            return VerticalElement().Expand();
        }

        public UIElement Panel()
        {
            return Element()
                .SetName("Panel")
                .SetBackgroundColor(PanelBackground)
                .SetBorderWidth(1)
                .SetBorderColor(PanelBorder)
                .SetBorderRadius(5)
                .SetPadding(5, 10, 5, 10)
                .SetOverflow(Overflow.Hidden);
        }

        public UIElement HorizontalPanel()
        {
            return Panel()
                .SetName("HorizontalPanel")
                .SetFlexDirection(FlexDirection.Row);
        }

        public UIElement VerticalPanel()
        {
            return Panel()
                .SetName("VerticalPanel")
                .SetFlexDirection(FlexDirection.Column);
        }

        public UIElement TitlePanel()
        {
            return Panel()
                .SetName("TitlePanel")
                .SetFlexDirection(FlexDirection.Row)
                .SetFlexShrink(0)
                .SetBackgroundColor(TitleBackground)
                .SetBorderColor(PanelBorder)
                .SetBorderWidth(1, 1, 0, 1)
                .SetPadding(5, 10, 5, 10);
        }

        public UIElement HeaderPanel()
        {
            return Panel()
                .SetName("HeaderPanel")
                .SetFlexDirection(FlexDirection.Row)
                .SetFlexShrink(0)
                .SetBackgroundColor(HeaderBackground)
                .SetBorderRadius(5, 5, 0, 0)
                .SetBorderColor(PanelBorder)
                .SetBorderWidth(1, 1, 0, 1)
                .SetPadding(5, 10, 5, 10);
        }

        public UIElement HeaderPanel(IUIBinding<bool> openBinding)
        {
            var element = HeaderPanel();

            var openTrigger = new Action<bool>(open =>
            {
                if (open)
                {
                    element.SetBorderRadius(5, 5, 0, 0);
                    element.SetBorderWidth(1, 1, 0, 1);
                }
                else
                {
                    element.SetBorderRadius(5, 5, 5, 5);
                    element.SetBorderWidth(1, 1, 1, 1);
                }
            });

            openBinding.Changed += openTrigger;
            openTrigger(openBinding.Value);
            return element;
        }

        public UIScrollView ScrollPanel(bool includeFooter = true)
        {
            var panel = ScrollView()
                .SetName("ScrollPanel")
                .SetBackgroundColor(PanelBackground)
                .SetBorderColor(PanelBorder)
                .SetBorderWidth(1, 1, 1, 1)
                .SetBorderRadius(0, 0, 5, 5)
                .SetFlexShrink(1)
                .SetFlexGrow(1);

            if (includeFooter)
            {
                panel.SetBorderRadius(0, 0, 5, 5);
            }
            else
            {
                panel.SetBorderRadius(0, 0, 0, 0);
            }

            return panel;
        }

        public UIElement ScrollContent()
        {
            return Element()
                .SetName("ScrollContent")
                .SetPadding(5)
                .SetWidth(new Length(100, LengthUnit.Percent))
                .SetHeight(new Length(100, LengthUnit.Percent));
        }

        public UIElement ContentPanel(bool includeFooter = true)
        {
            var panel = VerticalElement()
                .SetName("ContentPanel")
                .SetBackgroundColor(PanelBackground)
                .SetBorderColor(PanelBorder)
                .SetBorderWidth(1, 1, 1, 1)
                .SetBorderRadius(0, 0, 5, 5)
                .SetPadding(8);

            if (includeFooter)
            {
                panel.SetBorderRadius(0, 0, 5, 5);
            }
            else
            {
                panel.SetBorderRadius(0, 0, 0, 0);
            }

            return panel;
        }

        public UIElement FooterPanel()
        {
            return VerticalElement()
                .SetName("FooterPanel")
                .SetBackgroundColor(FooterBackground)
                .SetBorderRadius(0, 0, 5, 5)
                .SetBorderColor(PanelBorder)
                .SetBorderWidth(0, 1, 1, 1)
                .SetMinHeight(5)
                .SetFlexDirection(FlexDirection.Row)
                .SetFlexShrink(0)
                .SetPadding(5, 10, 5, 10);
        }

        public UIElement ErrorPanel()
        {
            return Panel()
                .SetName("ErrorPanel")
                .SetFlexDirection(FlexDirection.Row)
                .SetFlexShrink(0)
                .SetBackgroundColor(ErrorBackground);
        }

        public UIElement SuccessPanel()
        {
            return Panel()
                .SetName("SuccessPanel")
                .SetFlexDirection(FlexDirection.Row)
                .SetFlexShrink(0)
                .SetBackgroundColor(SuccessBackground);
        }

        public UIElement InfoPanel()
        {
            return Panel()
                .SetName("InfoPanel")
                .SetFlexDirection(FlexDirection.Row)
                .SetFlexShrink(0)
                .SetBackgroundColor(InfoBackground);
        }

        public UIElement WarningPanel()
        {
            return Panel()
                .SetName("WarningPanel")
                .SetFlexDirection(FlexDirection.Row)
                .SetFlexShrink(0)
                .SetBackgroundColor(WarningBackground);
        }

        public UIElement Highlight()
        {
            return Panel()
                .SetName("Highlight")
                .SetFlexDirection(FlexDirection.Row)
                .SetFlexShrink(0)
                .SetBackgroundColor(HighlightBackground);
        }

        public UIElement Block()
        {
            return Element()
                .SetName("Block")
                .SetHorizontal()
                .SetBorderRadius(3)
                .SetBorderWidth(1)
                .SetMargin(1)
                .SetBorderColor(BlockBorder)
                .SetPadding(5)
                .SetCustomStyle(
                    normal: element => element.SetBackgroundColor(BlockBackground),
                    hover: element => element.SetBackgroundColor(BlockHoverBackground));
        }

        public UIScope HorizontalScope()
        {
            return HorizontalElement()
                .SetName("HorizontalScope")
                .Scope();
        }

        public UIScope VerticalScope()
        {
            return VerticalElement()
                .SetName("VerticalScope")
                .Scope();
        }

        public UIScope EnabledScope()
        {
            return HorizontalElement()
                .SetName("EnabledScope")
                .SetEnabled()
                .Scope();
        }

        public UIScope DisabledScope()
        {
            return HorizontalElement()
                .SetName("DisabledScope")
                .SetDisabled()
                .Scope();
        }

        public UIScope EnabledScope(Func<bool> enabled)
        {
            return HorizontalElement()
                .SetName("EnabledScope")
                .BindEnable(BindReadOnly(enabled))
                .Scope();
        }

        public UISelectionGrid SelectionGrid(string[] selections)
        {
            return Add(new UISelectionGrid(this, selections)
                .SetName("SelectionGrid"));
        }

        public UISelectionGrid<T> SelectionGrid<T>() where T : Enum
        {
            return Add(new UISelectionGrid<T>(this)
                .SetName($"SelectionGrid<{typeof(T).Name}>"));
        }
    }

    class UISelectionGrid<T> : UIEditorElement<VisualElement> where T : Enum
    {
        public event Action<T> SelectionChange;
        public T Current { get; private set; }

        internal UISelectionGrid(UIEditorService ui) : base(ui)
        {
            var selections = (T[])Enum.GetValues(typeof(T));

            using (Scope())
            {
                for (int i = 0; i != selections.Length; ++i)
                {
                    var selection = selections[i];
                    var selectionString = selection.ToString();
                    var button = UI.Button()
                        .SetName(selectionString)
                        .BindEnable(UI.BindReadOnly(() => !Current.Equals(selection)))
                        .SetText(selectionString)
                        .SetClickCallback(() => SetSelection(selection));
                }
            }
        }

        public UISelectionGrid<T> RegisterSelectionCallback(Action<T> action)
        {
            SelectionChange += action;
            return this;
        }

        protected UISelectionGrid<T> SetSelection(T selection)
        {
            if (!Current.Equals(selection))
            {
                Current = selection;
                SelectionChange?.Invoke(Current);
            }

            return this;
        }

        public UISelectionGrid<T> BindValue(IUIBinding<T> binding)
        {
            return BindingUtils.Bind(this, (value) => SetSelection(value), (value) => SelectionChange += value, binding);
        }
    }

    class UISelectionGrid : UIEditorElement<VisualElement>
    {
        public event Action<int> ActiveChange;
        public string[] Selections { get; }
        public int Current { get; private set; }

        internal UISelectionGrid(UIEditorService ui, string[] selections) : base(ui)
        {
            Selections = selections;

            using (Scope())
            {
                for (int i = 0; i != Selections.Length; ++i)
                {
                    var buttonIndex = i;
                    var button = UI.Button()
                        .SetName(Selections[i])
                        .BindEnable(UI.BindReadOnly(() => Current != i))
                        .SetText(Selections[i])
                        .SetClickCallback(() => SetIndex(buttonIndex));
                }
            }
        }

        public UISelectionGrid RegisterSelectionCallback(Action<int> action)
        {
            ActiveChange += action;
            return this;
        }

        protected virtual UISelectionGrid SetIndex(int index)
        {
            Current = index;
            ActiveChange?.Invoke(Current);
            return this;
        }

        public UISelectionGrid BindValue(IUIBinding<int> binding)
        {
            return BindingUtils.Bind(this, (value) => SetIndex(value), binding);
        }
    }
}
