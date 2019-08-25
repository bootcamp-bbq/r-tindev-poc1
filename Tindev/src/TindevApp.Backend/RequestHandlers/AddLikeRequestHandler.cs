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
    public class AddLikeRequestHandler : IRequestHandler<AddLikeCmdRequest, AddLikeCmdResponse>
    {
        private readonly DeveloperDomain _developerDomain;

        public AddLikeRequestHandler(DeveloperDomain developerDomain)
        {
            _developerDomain = developerDomain ?? throw new ArgumentNullException(nameof(developerDomain));
        }

        public async Task<AddLikeCmdResponse> Handle(AddLikeCmdRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (cancellationToken.IsCancellationRequested)
                return null;

            var match = await _developerDomain.AddLike(request.User.UserName(), request.TargetUsername, cancellationToken);

            return new AddLikeCmdResponse
            {
                Match = match
            };
        }
    }
}
