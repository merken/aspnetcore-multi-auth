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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using auth.api.Services;
using Microsoft.AspNetCore.Authorization;
using auth.api.Security.AzureAd;
using auth.api.Security;
using Microsoft.AspNetCore.Mvc.Authorization;
using auth.api.Security.MyDb;
using Microsoft.AspNetCore.Http;
using WebApiContrib.Core;
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

        public void ConfigureServices(IServiceCollection services)
        {
            //Nothing to do here
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Configure the branches
            app.UseBranchWithServices("/api1", (services) =>
            {
                services
                    .AddSingleton<IConfiguration>(Configuration)
                    .AddTransient<ICustomAuthenticationService, MyDb1AuthenticationService>()
                    .AddMyDbAuthorization<MyDb1AuthenticationHandler>(Constants.MyDb1Scheme) // ==> Adds the Policy, Scheme and custom authentication using a ICustomAuthenticationService
                    .AddMvc()
                    .AddAllControllersOfAssemblyAsFeatureProvider(typeof(Startup).Assembly)
                ;
            },
            (appBuilder) =>
            {
                appBuilder.UseDeveloperExceptionPage();
                appBuilder.UseMvc();
            });

            app.UseBranchWithServices("/api2", (services) =>
            {
                services
                    .AddSingleton<IConfiguration>(Configuration)
                    .AddTransient<ICustomAuthenticationService, MyDb2AuthenticationService>()
                    .AddMyDbAuthorization<MyDb2AuthenticationHandler>(Constants.MyDb2Scheme) // ==> Adds the Policy, Scheme and custom authentication using a ICustomAuthenticationService
                    .AddMvc()
                    .AddAllControllersOfAssemblyAsFeatureProvider(typeof(Startup).Assembly)
                ;

            },
            (appBuilder) =>
            {
                appBuilder.UseDeveloperExceptionPage();
                appBuilder.UseMvc();
            });

            app.Run(async c =>
            {
                await c.Response.WriteAsync("Branches configured!");
            });
        }
    }
}
