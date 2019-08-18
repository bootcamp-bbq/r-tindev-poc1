using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Queries;
using TindevApp.Backend.Data.Repository.Abstract;

namespace TindevApp.Backend.Controllers
{
    public class DevsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DevsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IActionResult> Index([FromServices] IDevelopersRepository repo)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var devs = await repo.RetrieveAll(tokenSource.Token);
            return Ok(devs);
        }

        [HttpGet]
        public Task<IActionResult> List([FromQuery] string username, CancellationToken cancellationToken = default)
        {
            var request = new ListDevelopersQueryRequest
            {
                Username = username
            };

            return this.Mediator(request, cancellationToken);
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
