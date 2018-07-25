using System.Collections.Generic;
namespace HelloNetcore.Models
{
    public class ConsentViewModel : InputConsentViewModel
    {
        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string ClientLogoUrl { get; set; }

        public string ClientUrl { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }
}
