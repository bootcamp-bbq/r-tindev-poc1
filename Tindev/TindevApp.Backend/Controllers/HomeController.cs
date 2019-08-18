using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Services;

namespace TindevApp.Backend.Controllers
{
    public class HomeController : ControllerBase
    {
        public async Task<IActionResult> Index()
        {
            return Ok("Hello");
        }

        public async Task<IActionResult> Login([FromRoute]string username, [FromServices] IGithubService githubService, CancellationToken cancellationToken = default)
        {
            var result = await githubService.GetDeveloper(username, cancellationToken);

            return Ok(result);
        }
    }
}
