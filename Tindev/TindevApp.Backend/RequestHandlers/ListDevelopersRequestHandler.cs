using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Queries;
using TindevApp.Backend.Services;

namespace TindevApp.Backend.RequestHandlers
{
    public class ListDevelopersRequestHandler : IRequestHandler<ListDevelopersQueryRequest, ListDevelopersQueryResponse>
    {
        private readonly ILogger<ListDevelopersRequestHandler> _logger;

        private readonly IGithubService _githubService;

        public ListDevelopersRequestHandler(ILogger<ListDevelopersRequestHandler> logger, IGithubService githubService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _githubService = githubService ?? throw new ArgumentNullException(nameof(githubService));
        }

        public async Task<ListDevelopersQueryResponse> Handle(ListDevelopersQueryRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var developerFollowers = await _githubService.GetFollowers(request.Username, cancellationToken);

            return new ListDevelopersQueryResponse
            {
                Items = developerFollowers
            };
        }
    }
}
