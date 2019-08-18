﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Commands
{
    public class CreateUserCmdRequest : IRequest<CreateUserCmdResponse>
    {
        public string Username { get; set; }
    }
}
