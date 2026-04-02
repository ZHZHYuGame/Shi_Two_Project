namespace Unity.Services.Apis.Sample
{
    class AboutSectionView : SectionView
    {
        public override Section Section => Section.About;
        public override string Url => "https://docs.unity3d.com/Packages/com.unity.services.apis@0.0";
        public override string Info => "This package provides base access to Unity Gaming Services REST APIs.";

        AboutTabView TabView { get; }

        public AboutSectionView(App app) : base(app)
        {
            TabView = new AboutTabView(app, this);
        }

        protected override void CreateGUISection()
        {
            TabView.CreateGUI();
        }
    }
}
