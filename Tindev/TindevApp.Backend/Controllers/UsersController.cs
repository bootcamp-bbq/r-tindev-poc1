using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Models;
using TindevApp.Backend.Services;

namespace TindevApp.Backend.Controllers
{
    [Authorize]
    public class UsersController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost()]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Authenticate([FromForm]User userParam, [FromServices] IUserService userService, CancellationToken cancellationToken = default)
        {
            var user = await userService.Authenticate(userParam.Username, cancellationToken);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}
