using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Commands
{
    public class AddLikeCmdRequest : ServiceRequest, IRequest<AddLikeCmdResponse>
    {
        public string TargetUsername { get; set; }
    }
}
