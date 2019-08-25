using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Models
{
    public class ServiceRequest : IServiceRequest
    {
        public ClaimsPrincipal Principal { get; set; }

        public string CallerIpAddress { get; set; }

        public string CorrelationId { get; set; }
    }
}
