using Microsoft.AspNetCore.Authorization;

namespace auth.api.Security
{
    public class AuthorizeData : IAuthorizeData
    {
        public AuthorizeData(string policy)
        {
            this.Policy = policy;
        }
        public string Policy { get; set; }
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
    }
}