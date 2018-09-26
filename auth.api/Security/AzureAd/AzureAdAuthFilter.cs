using System;
using System.Threading.Tasks;
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

        public override async Task OnAuthorizationAsync(Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext context)
        {
            await base.OnAuthorizationAsync(context);

            var username = context.HttpContext.User.Identity.Name;
            
            Console.WriteLine($"{username} just logged in!");
            // TODO Check our own database to see if this user has access to the resource
            // TODO Log out the username to a service
            // TODO Create the user in our own database on first visit
            // TODO Your own business logic
        }
    }
}