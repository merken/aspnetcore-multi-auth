using System;
using System.Security.Claims;
using auth.api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace auth.api.Security.MyDb
{
    public static class MyDbExtensions
    {
        public static IServiceCollection AddMyDbAuthorization(this IServiceCollection serviceCollection)
        {
            serviceCollection
            .AddAuthorization(options =>
                {
                    options.AddMyDbPolicy();
                })
            .AddAuthentication(Constants.MyDbScheme)
            .AddScheme<MyDbAuthenticationOptions, MyDbAuthenticationHandler>(Constants.MyDbScheme, options =>
                {
                    options.Events = new MyDbAuthenticationEvents
                    {
                        OnValidateCredentials = async (context, serviceProvider) =>
                        {
                            var authenticationService = serviceProvider.GetService<ICustomAuthenticationService>();

                            var isAuthenticated = await authenticationService.AuthenticateUserAsync(context.Username, context.Password);
                            if (isAuthenticated)
                            {
                                var claims = new[]
                                {
                                    new Claim(
                                        ClaimTypes.NameIdentifier,
                                        context.Username,
                                        ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer),

                                    new Claim(
                                        ClaimTypes.Name,
                                        context.Username,
                                        ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer)
                                };

                                context.Principal = new ClaimsPrincipal(
                                    new ClaimsIdentity(claims, context.Scheme.Name));

                                context.Success();
                            }

                        }
                    };
                });

            return serviceCollection;
        }

        public static AuthorizationOptions AddMyDbPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(Constants.MyDbScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Constants.MyDbPolicy, policy);
            return options;
        }
    }
}