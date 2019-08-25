using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Commands;
using TindevApp.Backend.Domains;

namespace TindevApp.Backend.RequestHandlers
{
    public class AddDeslikeRequestHandler : IRequestHandler<AddDeslikeCmdRequest, AddDeslikeCmdResponse>
    {
        private readonly DeveloperDomain _developerDomain;

        public AddDeslikeRequestHandler(DeveloperDomain developerDomain)
        {
            _developerDomain = developerDomain ?? throw new ArgumentNullException(nameof(developerDomain));
        }

        public async Task<AddDeslikeCmdResponse> Handle(AddDeslikeCmdRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (cancellationToken.IsCancellationRequested)
                return null;

            await _developerDomain.AddDeslike(request.User.UserName(), request.TargetUsername, cancellationToken);

            return new AddDeslikeCmdResponse();
        }
    }
}
