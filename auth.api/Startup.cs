using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using auth.api.Services;
using auth.api.Security;
using auth.api.Security.MyDb;
using Microsoft.AspNetCore.Http;
using WebApiContrib.Core;

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
                    .AddControllersFromRouteBranch(typeof(Startup).Assembly, "/api1")
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
                    .AddControllersFromRouteBranch(typeof(Startup).Assembly, "/api2")
                ;
            },
            (appBuilder) =>
            {
                appBuilder.UseDeveloperExceptionPage();
                appBuilder.UseMvc();
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Branches configured!");
            });
        }
    }
}
