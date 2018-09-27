using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;

namespace auth.api.Extensions
{
    public class FilterProviderOption
    {
        public string RoutePrefix { get; set; }
        public AuthorizeFilter Filter { get; set; }
    }

    public class AuthenticationFilterProvider : DefaultFilterProvider
    {
        private readonly FilterProviderOption[] options;

        public AuthenticationFilterProvider(params FilterProviderOption[] options)
        {
            this.options = options;
        }

        public override void ProvideFilter(FilterProviderContext context, FilterItem filterItem)
        {
            var route = context.ActionContext.ActionDescriptor.AttributeRouteInfo.Template;

            var filter = options.FirstOrDefault(option => route.StartsWith(option.RoutePrefix))?.Filter;
            if (filter != null)
            {
                if (context.Results.All(r => r.Descriptor.Filter != filter))
                {
                    context.Results.Add(new FilterItem(new FilterDescriptor(filter, (int)FilterScope.Controller)));
                }
            }

            base.ProvideFilter(context, filterItem);
        }
    }
}