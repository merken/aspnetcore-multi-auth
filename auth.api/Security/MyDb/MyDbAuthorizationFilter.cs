using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace auth.api.Security.MyDb
{
    public class MyDbAuthorizeFilterAttribute : TypeFilterAttribute
    {
        public MyDbAuthorizeFilterAttribute() : base(typeof(MyDbAuthorizeFilter)) { }
    }

    public class MyDbAuthorizeFilter : AuthorizeFilter
    {
        public MyDbAuthorizeFilter(IAuthorizationPolicyProvider provider)
            : base(provider, new[] { new AuthorizeData(Constants.MyDbPolicy) }) { }
    }
}