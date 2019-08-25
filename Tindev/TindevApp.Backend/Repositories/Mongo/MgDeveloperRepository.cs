using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Repositories.Mongo
{
    public class MgDeveloperRepository : IDeveloperRepository
    {
        private readonly MongoDbOptions _mongoDbOptions;
        private readonly ILogger<MgDeveloperRepository> _logger;

        public MgDeveloperRepository(MongoDbOptions mongoDbOptions, ILogger<MgDeveloperRepository> logger)
        {
            _mongoDbOptions = mongoDbOptions ?? throw new ArgumentNullException(nameof(mongoDbOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        static IMongoDatabase GetDatabase(MongoDbOptions dbOptions)
        {
            var client = new MongoClient(dbOptions.ConnectionString);
            return client.GetDatabase(dbOptions.Database);
        }

        public async Task<Developer> Create(Developer developer, CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            await collection.InsertOneAsync(developer, new InsertOneOptions(), cancellationToken)
                .ConfigureAwait(false);

            return developer;
        }

        public Task<Developer> GetById(string id, CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            return collection.Find<Developer>(x => x.Id == id).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Developer>> ListAll(CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            return await collection.AsQueryable().ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Developer> Update(Developer developer, CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            var result = await collection.ReplaceOneAsync<Developer>(x => x.Id == developer.Id, developer, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!result.IsAcknowledged)
                _logger.LogWarning("Something went wrong when updating a developer record with Id {ID}", developer.Id);

            return developer;
        }
    }
}
