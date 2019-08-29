using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Domains;
using TindevApp.Backend.Queries;

namespace TindevApp.Backend.RequestHandlers
{
    public class ListDeveloperFriendsRequestHandler : IRequestHandler<ListDeveloperFriendsQueryRequest, ListDeveloperFriendsQueryResponse>
    {
        private readonly DeveloperDomain _developerDomain;

        public ListDeveloperFriendsRequestHandler(DeveloperDomain developerDomain)
        {
            _developerDomain = developerDomain ?? throw new ArgumentNullException(nameof(developerDomain));
        }

        public async Task<ListDeveloperFriendsQueryResponse> Handle(ListDeveloperFriendsQueryRequest request, CancellationToken cancellationToken)
        {
            var friendsColl = await _developerDomain.GetDeveloperFriends(request.TargetUsername, cancellationToken);

            return new ListDeveloperFriendsQueryResponse
            {
                Items = friendsColl
            };
        }
    }
}
