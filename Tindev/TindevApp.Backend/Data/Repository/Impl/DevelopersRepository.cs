using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TindevApp.Backend.Data.Queries;
using TindevApp.Backend.Data.Repository.Abstract;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Data.Repository.Impl
{
    public sealed class DevelopersRepository : AbstractBaseRepository<Developer>, IDevelopersRepository
    {
        private readonly ILogger<DevelopersRepository> _logger;

        public DevelopersRepository(IConfiguration config, Db db, ILogger<DevelopersRepository> logger) : base(config, db)
        {
            _logger = logger;
        }

        public async Task<Developer> Create(Developer dev, CancellationToken cancellationToken = default)
        {
            if (await base.Create(DeveloperQueries.Create, dev, cancellationToken) > 0)
            {
                return dev;
            }
            throw new ArgumentException("Can't create dev!");
        }

        public async Task<IEnumerable<Developer>> Retrieve(Developer parameters, CancellationToken cancellationToken = default)
        {
            return await base.Retrieve(DeveloperQueries.FindById, parameters, cancellationToken);
        }

        public async Task<IEnumerable<Developer>> RetrieveAll(CancellationToken cancellationToken = default)
        {
            return await base.Retrieve(DeveloperQueries.All, null, cancellationToken);
        }
        
        public async Task<IEnumerable<int>> RetrieveCountAll(CancellationToken cancellationToken = default)
        {
            return await base.RetrieveCount(DeveloperQueries.CountAll, null, cancellationToken);
        }

        public async Task<Developer> Update(Developer dev, CancellationToken cancellationToken = default)
        {
            var affected = await base.Update(DeveloperQueries.UpdateById, dev, cancellationToken);
            if (affected > 0)
            {
                return dev;
            }
            throw new ArgumentException("Can't find dev to update!");
        }

        public async Task<Developer> Delete(Developer dev, CancellationToken cancellationToken = default)
        {
            var affected = await base.Update(DeveloperQueries.UpdateById, dev);
            if (affected > 0)
            {
                return dev;
            }
            throw new ArgumentException("Can't find dev to update!");
        }
    }
}
