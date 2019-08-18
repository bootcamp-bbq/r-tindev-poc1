using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace TindevApp.Backend.Controllers
{
    public static class ControllerBaseExtensions
    {
        public static async Task<IActionResult> Mediator<TResponse>(this ControllerBase controllerBase, IRequest<TResponse> req, CancellationToken cancellationToken = default)
        {
            return controllerBase.Ok(await ExecuteMediator(controllerBase, req, cancellationToken));
        }

        public static async Task<IActionResult> Mediator<TResponse>(this ControllerBase controllerBase, IRequest<TResponse> req, IMediator mediator, CancellationToken cancellationToken = default)
        {
            return controllerBase.Ok(await ExecuteMediator(controllerBase, req, mediator, cancellationToken));
        }

        public static Task<TResponse> ExecuteMediator<TResponse>(this ControllerBase controllerBase, IRequest<TResponse> req, CancellationToken cancellationToken = default)
        {
            IMediator mediator = (IMediator)controllerBase.HttpContext.RequestServices.GetService(typeof(IMediator));

            return mediator.Send(req, cancellationToken);
        }

        public static Task<TResponse> ExecuteMediator<TResponse>(this ControllerBase controllerBase, IRequest<TResponse> req, IMediator mediator, CancellationToken cancellationToken = default)
        {
            return mediator.Send(req, cancellationToken);
        }
    }
}
