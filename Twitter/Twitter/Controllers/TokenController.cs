using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Interfaces;
using Twitter.Model;

namespace Twitter.Controllers
{
    /// <summary>
    /// Token Controller
    /// </summary>
    [Route("api/v1/Token")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TokenController: Controller
    {
        [AllowAnonymous]
        [HttpPost, MapToApiVersion("1.0")]
        public IActionResult Authenticate([FromBody] User userParam,
            [FromServices] IUserService userService,
            [FromServices] TelemetryClient telemetry)
        {
            var data = new Dictionary<string, string>
            {
                { "user", string.Empty }
            };
            data.Add("Source", Helper.CheckHeader(Request));
            telemetry.TrackEvent(Request.Path.Value, data);
            if (userParam == null)
                return BadRequest(new { message = "User not supplied" });
            var user = userService.Authenticate(userParam.Username, userParam.Password, userParam.Env);

            if (user == null)
                return BadRequest(new { message = "User not found" });

            return Ok(user);
        }
    }
}
