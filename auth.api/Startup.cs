using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using auth.api.Services;
using Microsoft.AspNetCore.Authorization;
using auth.api.Security.AzureAd;
using auth.api.Security;
using Microsoft.AspNetCore.Mvc.Authorization;
using auth.api.Security.MyDb;
using auth.api.Extensions;

namespace auth.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICustomAuthenticationService, MyDbAuthenticationService>();

            services
                .AddMyDbAuthorization() // ==> Adds the Policy, Scheme and custom authentication using a ICustomAuthenticationService
                .AddAzureAdAuthorization(Configuration) // ==> Adds the Policy, custom Bearer Scheme using JWT
                .AddMvc()
                .AddFilterProvider((serviceProvider) =>
                {
                    var azureAdAuthorizeFilter = new AuthorizeFilter(new AuthorizeData[] { new AuthorizeData { AuthenticationSchemes = Constants.AzureAdScheme } });
                    var myAuthorizeFilter = new AuthorizeFilter(new AuthorizeData[] { new AuthorizeData { AuthenticationSchemes = Constants.MyDbScheme } });

                    var filterProviderOptions = new FilterProviderOption[]{
                        new FilterProviderOption{
                            RoutePrefix = "api/users",
                            Filter = azureAdAuthorizeFilter
                        },
                        new FilterProviderOption{
                            RoutePrefix = "api/data",
                            Filter = myAuthorizeFilter
                        }
                    };

                    return new AuthenticationFilterProvider(filterProviderOptions);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
