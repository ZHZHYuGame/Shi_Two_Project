namespace Unity.Services.Apis.Sample
{
    abstract class EditorView
    {
        internal App App { get; }
        internal UIController UI => App.UI;

        public EditorView(App app)
        {
            App = app;
        }

        public virtual void Initialize() { }
        public abstract void CreateGUI();
    }
}
