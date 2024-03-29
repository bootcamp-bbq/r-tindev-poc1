﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Models
{
    public class Developer
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Bio { get; set; }

        public string Avatar { get; set; }

        public string GithubUri { get; set; }

        public List<string> Likes { get; set; }

        public List<string> Deslikes { get; set; }
    }
}
