namespace Unity.Services.Apis.Sample
{
    class AccessPolicySectionView : SectionView
    {
        public override Section Section => Section.AccessPolicy;
        public override string Url => "https://services.docs.unity.com/access-resource-policy/v1/";
        public override string Info => "Manage policies to your project and players. Improve security, apply sanctions, etc...";

        PolicyTabView TabView { get; }

        public AccessPolicySectionView(App app) : base(app)
        {
            TabView = new PolicyTabView(app, this);
        }

        protected override void CreateGUISection()
        {
            TabView.CreateGUI();
        }
    }
}
