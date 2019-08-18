using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TindevApp.Backend.Data.Repository.Abstract
{
    public abstract class AbstractBaseRepository<TEntity> where TEntity : class
    {

        private readonly IConfiguration _config;
        private readonly Db _db;

        protected async Task<int> Create(string commandText, TEntity entity, CancellationToken cancellationToken = default)
        {
            return await _db.ExecuteAsync(commandText, entity, cancellationToken);
        }

        protected async Task<IEnumerable<TEntity>> Retrieve(string commandText, TEntity parameters, CancellationToken cancellationToken = default)
        {
            return await _db.QueryAsync<TEntity>(commandText, parameters, cancellationToken);
        }
        
        protected async Task<IEnumerable<int>> RetrieveCount(string commandText, TEntity parameters, CancellationToken cancellationToken = default)
        {
            return await _db.QueryAsync<int>(commandText, parameters, cancellationToken);
        }

        protected async Task<int> Update(string commandText, TEntity entity, CancellationToken cancellationToken = default)
        {
            return await _db.ExecuteAsync(commandText, entity, cancellationToken);
        }

        protected async Task<int> Delete(string commandText, TEntity entity, CancellationToken cancellationToken = default)
        {
            return await _db.ExecuteAsync(commandText, entity, cancellationToken);
        }

        #region [General Class Behavior]

        protected AbstractBaseRepository(IConfiguration config, Db db)
        {
            _config = config;
            _db = db;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
        }

        #endregion [General Class Behavior]

    }

}
