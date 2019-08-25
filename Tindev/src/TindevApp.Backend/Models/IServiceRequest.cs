using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Models
{
    public interface IServiceRequest
    {
        ClaimsPrincipal User { get; set; }

        string CallerIpAddress { get; set; }

        string CorrelationId { get; set; }
    }
}
