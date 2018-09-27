using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace auth.api.Security
{
    public static class AuthenticationExtensions
    {
        public static MvcOptions AddGlobalAzureAuthentication(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(AzureAdFilter());

            return mvcOptions;
        }

        private static AuthorizeFilter AzureAdFilter()
        {
            var policy = new AuthorizationPolicyBuilder()
               .AddAuthenticationSchemes(Constants.AzureAdScheme)
               .RequireAuthenticatedUser()
               .Build();

            return new AuthorizeFilter(policy);
        }
    }
}