using TindevApp.Backend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TindevApp.Backend.Data.Repository.Abstract
{
    public interface IDevelopersRepository
    {
        Task<Developer> Create(Developer dev, CancellationToken cancellationToken);
        Task<IEnumerable<Developer>> Retrieve(Developer parameters,CancellationToken cancellationToken);
        Task<IEnumerable<Developer>> RetrieveAll(CancellationToken cancellationToken);
        Task<IEnumerable<int>> RetrieveCountAll(CancellationToken cancellationToken);
        Task<Developer> Update(Developer dev, CancellationToken cancellationToken);
        Task<Developer> Delete(Developer dev, CancellationToken cancellationToken);
    }
}
