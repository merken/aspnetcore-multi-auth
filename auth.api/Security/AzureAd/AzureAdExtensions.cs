using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace auth.api.Security.AzureAd
{
    public static class AzureAdExtensions
    {
        public static IServiceCollection AddAzureAdAuthorization(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
            .AddAuthorization(options =>
                {
                    options.AddAzureAdPolicy();
                })
            .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = Constants.AzureAdScheme;
                    options.DefaultChallengeScheme = Constants.AzureAdScheme;
                })
            .AddJwtBearer(Constants.AzureAdScheme, options =>
                {
                    options.Authority = String.Format(configuration["Authentication:Authority"], configuration["Authentication:Tenant"]);
                    options.Audience = configuration["Authentication:ClientId"];
                });

            return serviceCollection;
        }

        public static AuthorizationOptions AddAzureAdPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(Constants.AzureAdScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Constants.AzureAdPolicy, policy);
            return options;
        }
    }
}