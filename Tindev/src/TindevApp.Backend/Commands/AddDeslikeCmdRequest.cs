using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Commands
{
    public class AddDeslikeCmdRequest : ServiceRequest, IRequest<AddDeslikeCmdResponse>
    {
        public string TargetUsername { get; set; }
    }
}
