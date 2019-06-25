using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth.api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Constants.MyDbPolicy)]
    [RouteBranchAttribute("/api1")]
    public class DataController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "secret", "data", HttpContext.User.Identity.Name };
        }
    }
}
