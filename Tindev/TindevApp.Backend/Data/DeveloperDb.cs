using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TindevApp.Backend.Data
{
    public class DeveloperDb : Db
    {
        public DeveloperDb(string connectionString, ILogger<Db> logger) : base(connectionString, logger)
        {
        }


    }
}
