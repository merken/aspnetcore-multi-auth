using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace auth.api.Security.MyDb
{
    public class MyDbAuthenticationFailedContext : ResultContext<MyDbAuthenticationOptions>
    {
        public MyDbAuthenticationFailedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            MyDbAuthenticationOptions options)
            : base(context, scheme, options)
        {
        }

        public Exception Exception { get; set; }
    }
}