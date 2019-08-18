using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Models
{
    public class Developer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Bio { get; set; }

        public string AvatarUri { get; set; }

        public string GithubUri { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string[] Likes { get; set; }

        public string[] Deslikes { get; set; }
    }
}
