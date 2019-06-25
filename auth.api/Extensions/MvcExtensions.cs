using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace auth.api.Extensions
{
    public static class MvcExtensions
    {
        public static IMvcBuilder AddAllControllersOfAssemblyAsFeatureProvider(this IMvcBuilder builder, Assembly Assembly)
        {
            return builder.ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Clear();
                manager.FeatureProviders.Add(new ControllerFeatureProvider());
            }).AddApplicationPart(Assembly);
        }
    }
}