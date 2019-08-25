using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Queries
{
    public class ListDevelopersQueryResponse
    {
        public IEnumerable<Developer> Items { get; set; }
    }
}
