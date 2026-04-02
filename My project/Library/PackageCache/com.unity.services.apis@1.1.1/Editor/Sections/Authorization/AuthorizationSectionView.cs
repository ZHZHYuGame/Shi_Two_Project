using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class AuthorizationSectionView : SectionView
    {
        public override Section Section => Section.Authorization;
        public override string Url => "https://services.docs.unity.com/auth";
        public override string Info => "Exchanges service account credentials for an access token to use game apis authoritatively.";

        AuthorizationController Authorization => App.Authorization;

        public AuthorizationSectionView(App app) : base(app)
        {
        }

        protected override void CreateGUISection()
        {
            UI.H5("Admin API");
            UI.Separator();
            UI.Label("Admin operations use a service account in the Authorization header of each request.");
            UI.Snippet().SetText(Snippets.AdminAuth).SetPadding(5, 0, 10, 0);

            UI.H5("Game API");
            UI.Separator();
            UI.Label("Game operations require a player access token obtained through player authentication.");
            UI.Snippet().SetText(Snippets.GameAuth).SetPadding(5, 0, 10, 0);

            UI.H5("Server API");
            UI.Separator();
            UI.Label("Server operations require an access token obtained from either a server or service account exchange.");
            UI.Space();
            UI.Label("Multiplay Server").SetFontStyleBold();
            UI.Snippet().SetText(Snippets.ServerMultiplayAuth).SetPadding(5, 0, 10, 0);
            UI.Label("Any Server").SetFontStyleBold();
            UI.Snippet().SetText(Snippets.ServerServiceAccountAuth).SetPadding(5, 0, 10, 0);

            UI.H5("Trusted API");
            UI.Separator();
            UI.Label("Trusted operations require an access token obtained from a service account exchange.");
            UI.Snippet().SetText(Snippets.TrustedAuth).SetPadding(5, 0, 10, 0);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Sign In", SignInServiceAccountAsync).SetWidth(200).SetMarginBottom(10);
                CreateGUIAccessToken();
            }
        }

        void CreateGUIAccessToken()
        {
            using (UI.Element()
                .SetName("AccessTokenContainer")
                .BindVisibility(UI.BindReadOnly(() => Authorization.Authorized), true)
                .Scope())
            {
                using (UI.HeaderPanel().Scope())
                {
                    UI.H5($"Service Access Token");
                    UI.Flex();
                    UI.EnumField()
                        .Init(Authorization.ViewType)
                        .BindValue(UI.BindTarget(
                            () => (Enum)Authorization.ViewType,
                            (value) => Authorization.ViewType = (AccessTokenViewType)value))
                        .SetWidth(100);
                }
                using (UI.ContentPanel(false).Scope())
                {
                    using (UI.Block().Scope())
                    {
                        UI.SelectableLabel()
                            .BindValue(UI.BindProperty(() => Authorization.ViewToken))
                            .SetMultiline(true)
                            .SetWhitespace(WhiteSpace.Normal);
                    }
                }
                using (UI.FooterPanel().Scope())
                {
                    UI.Button("Clear", ClearToken).SetWidth(100);
                }
            }
        }

        async Task SignInServiceAccountAsync()
        {
            try
            {
                var response = await Authorization.SignInWithServiceAccount();
                response.EnsureSuccessful();
                SetStatus(Status.Success, "SignInWithServiceAccount successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "SignInWithServiceAccount failed.", e);
            }
        }

        void ClearToken()
        {
            Authorization.ClearCredentials();
        }
    }
}
