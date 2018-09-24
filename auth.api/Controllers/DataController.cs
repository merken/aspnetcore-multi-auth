using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth.api.Extensions;
using auth.api.Security;
using auth.api.Security.MyDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.api.Controllers
{
    [Route("api/data")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = Constants.MyDbScheme)]
    // [MyDbAuthorizeFilter]
    public class DataController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "secret", "data", HttpContext.User.Identity.Name };
        }
    }
}
