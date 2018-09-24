using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace auth.api.Security.MyDb
{
     public class ValidateCredentialsContext : ResultContext<MyDbAuthenticationOptions>
    {
        public ValidateCredentialsContext(
            HttpContext context,
            AuthenticationScheme scheme,
            MyDbAuthenticationOptions options)
            : base(context, scheme, options)
        {
        }

        public string Username { get; set; }
        
        public string Password { get; set; }
    }
}