using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, CancellationToken cancellationToken = default);
    }
}
