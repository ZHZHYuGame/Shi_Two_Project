namespace Unity.Services.Apis.Sample
{
    class EconomySectionView : SectionView
    {
        public override Section Section => Section.Economy;
        public override string Url => "https://services.docs.unity.com/economy-admin/";
        public override string Info => "Configure your resources and see your players inventories";

        EconomyTabView TabView { get; }

        public EconomySectionView(App app) : base(app)
        {
            TabView = new EconomyTabView(app, this);
        }

        protected override void CreateGUISection()
        {
            TabView.CreateGUI();
        }
    }
}
