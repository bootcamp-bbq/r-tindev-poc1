using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Queries
{
    public class ListDeveloperFriendsQueryRequest : ServiceRequest, IRequest<ListDeveloperFriendsQueryResponse>
    {
        public string TargetUsername { get; set; }
    }
}
