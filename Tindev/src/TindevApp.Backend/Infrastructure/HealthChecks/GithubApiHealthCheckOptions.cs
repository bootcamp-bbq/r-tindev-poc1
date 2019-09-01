using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Infrastructure.HealthChecks
{
    public class GithubApiHealthCheckOptions
    {
        public string UserAgent { get; set; }

        public string Token { get; set; }

        public System.Uri Uri { get; set; }
    }
}
