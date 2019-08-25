using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Infrastructure.Mvc
{
    public class RequestPopulatorActionFilter : IActionFilter
    {
        public static IActionFilter Instance { get; } = new RequestPopulatorActionFilter();

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.SingleOrDefault(x => x.Value is IServiceRequest);
            if (param.Value != null)
            {
                RequestPopulator.PopulateRequest((IServiceRequest)param.Value, context.HttpContext);
            }
        }
    }
}
