using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Services
{
    public interface IDeveloperRepository
    {
        Task<Developer> Create(Developer developer, CancellationToken cancellationToken = default);

        Task<Developer> CreateOrUpdate(Developer developer, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Developer>> ListAll(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Developer>> ListAllExcept(string username, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Developer>> ListAllExceptInLikeAndDeslike(string username, CancellationToken cancellationToken = default);

        Task<Developer> GetById(string id, CancellationToken cancellationToken = default);

        Task<Developer> GetByUsername(string username, CancellationToken cancellationToken = default);

        Task<Developer> Update(Developer developer, CancellationToken cancellationToken = default);
    }
}
