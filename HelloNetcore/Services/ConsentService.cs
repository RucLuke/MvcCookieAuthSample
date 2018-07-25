using System.Linq;
using System.Threading.Tasks;
using HelloNetcore.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace HelloNetcore.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServer;

        public ConsentService(
            IClientStore clientStore,
            IResourceStore resourceStore,
            IIdentityServerInteractionService identityServer)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServer = identityServer;
        }

        public async Task<ConsentViewModel> BuildConsentViewModelAsync(string returnUrl, InputConsentViewModel inputViewModel = null)
        {
            var request = await _identityServer.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
            {
                return null;
            }

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            var vm = CreateConsentViewModel(client, resources, inputViewModel);
            vm.ReturnUrl = returnUrl;
            return vm;
        }

        public async Task<ProcessConsentResult> ProcessConsent(InputConsentViewModel viewModel)
        {
            ConsentResponse consentResponse = null;
            var result = new ProcessConsentResult();
            switch (viewModel.Button)
            {
                case "no":
                    consentResponse = ConsentResponse.Denied;
                    break;
                case "yes":
                    if (viewModel.ScopesConsented != null && viewModel.ScopesConsented.Any())
                    {
                        consentResponse = new ConsentResponse()
                        {
                            ScopesConsented = viewModel.ScopesConsented,
                            RememberConsent = viewModel.RememberConsent
                        };
                    }
                    else
                    {
                        result.ValidationError = "请至少选择一个信息！";
                    }
                    break;
            }

            if (consentResponse != null)
            {
                var request = await _identityServer.GetAuthorizationContextAsync(viewModel.ReturnUrl);
                await _identityServer.GrantConsentAsync(request, consentResponse);
                result.RedirectUrl = viewModel.ReturnUrl;
            }
            else
            {
                var consentViewModel = await BuildConsentViewModelAsync(viewModel.ReturnUrl, viewModel);
                result.ViewModel = consentViewModel;
            }


            return result;
        }

        #region private methods

        private ConsentViewModel CreateConsentViewModel(Client client, Resources resources, InputConsentViewModel model)
        {
            var selectedScopes = model?.ScopesConsented ?? Enumerable.Empty<string>();
            return new ConsentViewModel
            {
                ClientName = client.ClientName,
                ClientLogoUrl = client.LogoUri,
                ClientUrl = client.ClientUri,
                RememberConsent = model?.RememberConsent ?? client.AllowRememberConsent,
                IdentityScopes =
                    resources.IdentityResources.Select(i => CreateScopeViewModel(i, selectedScopes.Contains(i.Name))),
                ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes)
                    .Select(i => CreateScopeViewModel(i, selectedScopes.Contains(i.Name)))
            };
        }


        private static ScopeViewModel CreateScopeViewModel(IdentityResource resource, bool check)
        {
            return new ScopeViewModel()
            {
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Required = resource.Required,
                Checked = check || resource.Required,
                Emphasize = resource.Emphasize
            };
        }

        private static ScopeViewModel CreateScopeViewModel(Scope resource, bool check)
        {
            return new ScopeViewModel()
            {
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Required = resource.Required,
                Checked = check || resource.Required,
                Emphasize = resource.Emphasize
            };
        }

        #endregion

    }
}
