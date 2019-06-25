using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace auth.api
{
    public class RouteBranchControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly string route;
        public RouteBranchControllerFeatureProvider(string route)
        {
            this.route = route;
        }

        protected override bool IsController(System.Reflection.TypeInfo typeInfo)
        {
            if (!typeof(ControllerBase).GetTypeInfo().IsAssignableFrom(typeInfo)) return false;// This is not an API Controller

            var routeBranchAttribute = typeInfo.GetCustomAttributes(false).OfType<RouteBranchAttribute>().FirstOrDefault();
            if (routeBranchAttribute == null)
                return true; // This is not an API that supports branching, so it's added by default

            return route.StartsWith(routeBranchAttribute.Route); // Check wether branch
        }
    }
}