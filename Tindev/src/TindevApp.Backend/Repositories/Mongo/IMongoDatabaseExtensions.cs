using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Repositories.Mongo
{
    internal static class IMongoDatabaseExtensions
    {
        internal static IMongoCollection<Developer> GetDeveloperCollection(this IMongoDatabase mongoDatabase)
        {
            return mongoDatabase.GetCollection<Developer>("developer");
        }

        internal static IMongoCollection<BsonDocument> GetDeveloperBsonCollection(this IMongoDatabase mongoDatabase)
        {
            return mongoDatabase.GetCollection<BsonDocument>("developer");
        }
    }
}
