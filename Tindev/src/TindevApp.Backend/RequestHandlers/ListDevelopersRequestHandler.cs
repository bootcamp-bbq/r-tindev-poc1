using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Queries;
using TindevApp.Backend.Repositories;
using TindevApp.Backend.Services;

namespace TindevApp.Backend.RequestHandlers
{
    public class ListDevelopersRequestHandler : IRequestHandler<ListDevelopersQueryRequest, ListDevelopersQueryResponse>
    {
        private readonly ILogger<ListDevelopersRequestHandler> _logger;

        private readonly IDeveloperRepository _developerRepository;

        public ListDevelopersRequestHandler(ILogger<ListDevelopersRequestHandler> logger, IDeveloperRepository developerRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _developerRepository = developerRepository ?? throw new ArgumentNullException(nameof(developerRepository));
        }

        public async Task<ListDevelopersQueryResponse> Handle(ListDevelopersQueryRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var developerFollowers = await _developerRepository.ListAllExcept(request.User.UserName(), cancellationToken);

            return new ListDevelopersQueryResponse
            {
                Items = developerFollowers
            };
        }
    }
}
