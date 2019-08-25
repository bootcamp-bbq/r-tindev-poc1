using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Infrastructure
{
    public static class RequestPopulator
    {
        public static void PopulateRequest(IServiceRequest serviceRequest, HttpContext httpContext)
        {
            serviceRequest.Principal = httpContext.User;
        }
    }
}
