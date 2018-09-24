using System;
using System.Security.Claims;
using System.Threading.Tasks;
using auth.api.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace auth.api.Security
{
    public static class MvcExtensions
    {
        public static IMvcBuilder AddFilterProvider(this IMvcBuilder builder, Func<IServiceProvider, IFilterProvider> provideFilter)
        {
            builder.Services.Replace(
                ServiceDescriptor.Transient(provideFilter));

            return builder;
        }
    }
}