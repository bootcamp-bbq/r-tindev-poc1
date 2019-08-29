using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Commands;
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

        [HttpGet("{username}/friends")]
        public async Task<IActionResult> Friends([FromRoute] string username, CancellationToken cancellationToken = default)
        {
            var rq = this.RequestFrom<ListDeveloperFriendsQueryRequest>();

            rq.TargetUsername = username;

            return await this.Mediator(rq, cancellationToken);
        }

        [HttpGet("friends")]
        public async Task<IActionResult> Friends(CancellationToken cancellationToken = default)
        {
            var rq = this.RequestFrom<ListDeveloperFriendsQueryRequest>();

            rq.TargetUsername = rq.User.UserName();

            return await this.Mediator(rq, cancellationToken);
        }

        [HttpPost("{username}/like/add")]
        public async Task<IActionResult> Like([FromBody] AddLikeCmdRequest request, [FromRoute]string username, CancellationToken cancellationToken = default)
        {
            request.TargetUsername = username;
            return await this.Mediator(request, cancellationToken);
        }

        [HttpPost("{username}/deslike/add")]
        public async Task<IActionResult> Deslike([FromBody] AddDeslikeCmdRequest request, [FromRoute]string username, CancellationToken cancellationToken = default)
        {
            request.TargetUsername = username;
            return await this.Mediator(request, cancellationToken);
        }
    }
}
