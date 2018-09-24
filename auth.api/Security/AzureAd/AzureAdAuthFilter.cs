using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace auth.api.Security.AzureAd
{
    public class AzureAdAuthFilterAttribute : TypeFilterAttribute
    {
        public AzureAdAuthFilterAttribute() : base(typeof(AzureAdAuthFilter)) { }
    }

    public class AzureAdAuthFilter : AuthorizeFilter
    {
        public AzureAdAuthFilter(IAuthorizationPolicyProvider provider)
            : base(provider, new[] { new AuthorizeData(Constants.AzureAdPolicy) }) { }
    }
}