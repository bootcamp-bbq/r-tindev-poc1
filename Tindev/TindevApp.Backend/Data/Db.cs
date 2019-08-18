using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Infrastructure;

namespace TindevApp.Backend.Data
{
    public class Db
    {
        private readonly string _connectionString;
        private readonly ILogger<Db> _logger;

        public Db(ILogger<Db> logger, IOptions<DbConnectionOptions> dbConnectionOptions)
        {
            if (dbConnectionOptions?.Value == null)
                throw new ArgumentNullException(nameof(dbConnectionOptions));

            _connectionString = dbConnectionOptions.Value.ConnectionString;
            _logger = logger;
        }

        internal async Task<int> ExecuteAsync(string sql, Object param, CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteAsync(sql, param);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when executing query.");
                    throw;
                }
                finally
                {
                    if (connection.State != System.Data.ConnectionState.Closed)
                        connection.Close();
                }
            }
        }

        internal async Task<IEnumerable<T>> QueryAsync<T>(string sql, Object param, CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    return await connection.QueryAsync<T>(sql, param);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when executing query.");
                    throw;
                }
                finally
                {
                    if (connection.State != System.Data.ConnectionState.Closed)
                        connection.Close();
                }
            }
        }
    }
}
