using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth.api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.api.Controllers
{
    [Route("api/data")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = Constants.MyDbScheme)]
    public class DataController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "secret", "data", HttpContext.User.Identity.Name };
        }
    }
}
