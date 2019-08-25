using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Models;
using TindevApp.Backend.Services;

namespace TindevApp.Backend.Repositories.Mongo
{
    public class MgDeveloperRepository : IDeveloperRepository
    {
        private readonly MongoDbOptions _mongoDbOptions;
        private readonly ILogger<MgDeveloperRepository> _logger;

        public MgDeveloperRepository(IOptions<MongoDbOptions> mongoDbOptions, ILogger<MgDeveloperRepository> logger)
        {
            _mongoDbOptions = mongoDbOptions?.Value ?? throw new ArgumentNullException(nameof(mongoDbOptions));
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

        public async Task<Developer> CreateOrUpdate(Developer developer, CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            var updateDefinition = Builders<Developer>.Update
                .Set(x => x.GithubUri, developer.GithubUri);

            if (!string.IsNullOrEmpty(developer.Avatar))
                updateDefinition.Set(x => x.Avatar, developer.Avatar);

            if (!string.IsNullOrEmpty(developer.Bio))
                updateDefinition.Set(x => x.Bio, developer.Bio);

            if (!string.IsNullOrEmpty(developer.Name))
                updateDefinition.Set(x => x.Name, developer.Name);

            if (!string.IsNullOrEmpty(developer.Username))
                updateDefinition.Set(x => x.Username, developer.Username);

            var filterDefinition = Builders<Developer>.Filter.Eq(x => x.Username, developer.Username);

            var options = new FindOneAndUpdateOptions<Developer, Developer>() { IsUpsert = true, ReturnDocument = ReturnDocument.After };

            var result = await collection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, options, cancellationToken);

            developer.Id = result.Id;

            return developer;
        }

        public Task<Developer> GetById(string id, CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            return collection.Find<Developer>(x => x.Id == id).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<Developer> GetByUsername(string username, CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            return collection.Find<Developer>(x => x.Username == username).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Developer>> ListAll(CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            return await collection.AsQueryable().ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Developer>> ListAllExcept(string username, CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            return await collection.Find(x => x.Username != username).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Developer>> ListAllExceptInLikeAndDeslike(string username, CancellationToken cancellationToken = default)
        {
            var collection = GetDatabase(_mongoDbOptions).GetDeveloperCollection();

            var filter = Builders<Developer>.Filter.And(
                Builders<Developer>.Filter.Ne(x => x.Username, username),
                Builders<Developer>.Filter.AnyNin(x => x.Likes, new string[] { username }),
                Builders<Developer>.Filter.AnyNin(x => x.Deslikes, new string[] { username }));

            return await collection.Find(filter).ToListAsync(cancellationToken);
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
