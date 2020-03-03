using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace auth.api.Extensions
{
    public class FilterProviderOption
    {
        public string RoutePrefix { get; set; }
        public AuthorizeFilter Filter { get; set; }
    }

    /// <summary>
    /// DefaultFilterProvider changed from public to internal in net core 3.1
    /// https://github.com/dotnet/aspnetcore/blob/a2568cbe1e8dd92d8a7976469100e564362f778e/src/Mvc/Mvc.Core/src/Filters/DefaultFilterProvider.cs#L10
    /// </summary>
    public class FilterProviderBase : IFilterProvider
    {
        public int Order => -1000;

        /// <inheritdoc />
        public virtual void OnProvidersExecuting(FilterProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ActionContext.ActionDescriptor.FilterDescriptors != null)
            {
                var results = context.Results;
                // Perf: Avoid allocating enumerator and read interface .Count once rather than per iteration
                var resultsCount = results.Count;
                for (var i = 0; i < resultsCount; i++)
                {
                    ProvideFilter(context, results[i]);
                }
            }
        }

        /// <inheritdoc />
        public virtual void OnProvidersExecuted(FilterProviderContext context)
        {
        }

        public virtual void ProvideFilter(FilterProviderContext context, FilterItem filterItem)
        {
            if (filterItem.Filter != null)
            {
                return;
            }

            var filter = filterItem.Descriptor.Filter;

            if (!(filter is IFilterFactory filterFactory))
            {
                filterItem.Filter = filter;
                filterItem.IsReusable = true;
            }
            else
            {
                var services = context.ActionContext.HttpContext.RequestServices;
                filterItem.Filter = filterFactory.CreateInstance(services);
                filterItem.IsReusable = filterFactory.IsReusable;

                if (filterItem.Filter == null)
                {
                    throw new InvalidOperationException(
                        $"CreateInstance({typeof(IFilterFactory).Name})");
                }

                ApplyFilterToContainer(filterItem.Filter, filterFactory);
            }
        }

        protected virtual void ApplyFilterToContainer(object actualFilter, IFilterMetadata filterMetadata)
        {
            if (actualFilter is IFilterContainer container)
            {
                container.FilterDefinition = filterMetadata;
            }
        }
    }

    public class AuthenticationFilterProvider : FilterProviderBase
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