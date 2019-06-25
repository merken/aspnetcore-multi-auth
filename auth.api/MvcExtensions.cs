using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace auth.api
{
    public static class MvcExtensions
    {
        public static IMvcBuilder AddControllersFromRouteBranch(this IMvcBuilder builder, Assembly Assembly, string route)
        {
            return builder.ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Clear();
                manager.FeatureProviders.Add(new RouteBranchControllerFeatureProvider(route));
            }).AddApplicationPart(Assembly);
        }
    }
}