﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Commands;
using TindevApp.Backend.Services;

namespace TindevApp.Backend.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Hello");
        }

        [HttpPost]
        public Task<IActionResult> Login([FromBody] CreateUserCmdRequest request, [FromServices] IMediator mediatorService, CancellationToken cancellationToken = default)
        {
            return this.Mediator(request, mediatorService, cancellationToken);
        }
    }
}
