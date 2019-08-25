using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Queries;

namespace TindevApp.Backend.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class DevsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var rq = this.RequestFrom<ListDevelopersQueryRequest>();

            return await this.Mediator(rq, cancellationToken);
        }

        [HttpGet]
        [Route("{username}/friends")]
        public async Task<IActionResult> Friends([FromRoute] string username, CancellationToken cancellationToken = default)
        {
            var rq = this.RequestFrom<ListDeveloperFriendsQueryRequest>();

            rq.TargetUsername = username;

            return await this.Mediator(rq, cancellationToken);
        }

        [HttpGet]
        [Route("friends")]
        public async Task<IActionResult> Friends(CancellationToken cancellationToken = default)
        {
            var rq = this.RequestFrom<ListDeveloperFriendsQueryRequest>();

            rq.TargetUsername = rq.Principal.UserName();

            return await this.Mediator(rq, cancellationToken);
        }

        [HttpPost]
        public IActionResult Like()
        {
            return Ok();
        }

        public IActionResult Deslike()
        {
            return Ok();
        }
    }
}
