using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Infrastructure;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Controllers
{
    public static class RequestPopulateControllerExtensions
    {
        public static TRequest RequestFrom<TRequest>(this ControllerBase controllerBase)
            where TRequest : IServiceRequest, new()
        {
            var rq = new TRequest();

            PopulateRequest(controllerBase, rq);
            return rq;
        }

        public static void PopulateRequest(this ControllerBase controllerBase, IServiceRequest request)
        {
            RequestPopulator.PopulateRequest(request, controllerBase.HttpContext);
        }
    }
}
