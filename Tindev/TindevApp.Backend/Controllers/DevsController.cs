using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Controllers
{
    public class DevsController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
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
