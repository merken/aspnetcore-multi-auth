using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace auth.api.Security.MyDb
{

    public class MyDb1AuthenticationHandler : MyDbAuthenticationHandler
    {
        protected override string GetScheme() => Constants.MyDb1Scheme;

        public MyDb1AuthenticationHandler(
            IOptionsMonitor<MyDbAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceProvider serviceProvider) : base(options, logger, encoder, clock, serviceProvider) { }
    }

    public class MyDb2AuthenticationHandler : MyDbAuthenticationHandler
    {
        protected override string GetScheme() => Constants.MyDb2Scheme;

        public MyDb2AuthenticationHandler(
            IOptionsMonitor<MyDbAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceProvider serviceProvider) : base(options, logger, encoder, clock, serviceProvider) { }
    }

    public abstract class MyDbAuthenticationHandler : AuthenticationHandler<MyDbAuthenticationOptions>
    {
        private readonly IServiceProvider serviceProvider;

        protected abstract string GetScheme();

        public MyDbAuthenticationHandler(
            IOptionsMonitor<MyDbAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceProvider serviceProvider) : base(options, logger, encoder, clock)
        {
            this.serviceProvider = serviceProvider;
        }

        protected new MyDbAuthenticationEvents Events
        {
            get { return (MyDbAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new MyDbAuthenticationEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }

            if (!authorizationHeader.StartsWith(GetScheme() + ' ', StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            string credentials = authorizationHeader.Substring(GetScheme().Length).Trim();

            if (string.IsNullOrEmpty(credentials))
            {
                return AuthenticateResult.Fail("Credentials not provided");
            }

            try
            {
                var username = credentials.Split(";")[0];
                var password = credentials.Split(";")[1];

                var validateCredentialsContext = new ValidateCredentialsContext(Context, Scheme, Options)
                {
                    Username = username,
                    Password = password
                };

                await Events.ValidateCredentials(validateCredentialsContext, serviceProvider);

                if (validateCredentialsContext.Result != null &&
                    validateCredentialsContext.Result.Succeeded)
                {
                    var ticket = new AuthenticationTicket(validateCredentialsContext.Principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }

                if (validateCredentialsContext.Result != null &&
                    validateCredentialsContext.Result.Failure != null)
                {
                    return AuthenticateResult.Fail(validateCredentialsContext.Result.Failure);
                }

                return AuthenticateResult.NoResult();
            }
            catch (Exception ex)
            {
                var authenticationFailedContext = new MyDbAuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = ex
                };

                await Events.AuthenticationFailed(authenticationFailedContext);

                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (!Request.IsHttps)
            {
                var insecureProtocolMessage = $"Request is HTTP, {GetScheme()} Authentication will not respond.";
                Logger.LogInformation(insecureProtocolMessage);
                Response.StatusCode = 500;
                var encodedResponseText = Encoding.UTF8.GetBytes(insecureProtocolMessage);
                Response.Body.Write(encodedResponseText, 0, encodedResponseText.Length);
            }
            else
            {
                Response.StatusCode = 401;

                var headerValue = GetScheme() + $" USERNAME;PASSWORD";
                Response.Headers.Append(HeaderNames.WWWAuthenticate, headerValue);
            }

            return Task.CompletedTask;
        }
    }
}