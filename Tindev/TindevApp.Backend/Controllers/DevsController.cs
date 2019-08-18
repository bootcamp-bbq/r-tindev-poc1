using Microsoft.AspNetCore.Mvc;
using TindevApp.Backend.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TindevApp.Backend.Controllers
{
    public class DevsController : ControllerBase
    {
        public async Task<IActionResult> Index([FromServices] IDevelopersRepository repo)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var devs = await repo.RetrieveAll(tokenSource.Token);
            return Ok(devs);
        }


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
