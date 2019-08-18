using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login(string username)
        {
            return Ok();
        }
    }
}
