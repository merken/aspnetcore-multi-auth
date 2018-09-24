using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth.api.Extensions;
using auth.api.Security;
using auth.api.Security.AzureAd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.api.Controllers
{
    [Route("api/users")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = Constants.AzureAdScheme)]
    // [AzureAdAuthFilter]
    public class UsersController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "maarten", HttpContext.User.Identity.Name };
        }
    }
}
