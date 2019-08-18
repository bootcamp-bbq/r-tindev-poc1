using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Queries
{
    public class ListDevelopersQueryRequest : IRequest<ListDevelopersQueryResponse>
    {
        public string Username { get; set; }
    }
}
