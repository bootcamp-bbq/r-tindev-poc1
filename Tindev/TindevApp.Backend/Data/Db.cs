using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TindevApp.Backend.Data
{
    public class Db
    {
        private readonly string _connectionString;
        private readonly ILogger<Db> _logger;

        public Db(string connectionString, ILogger<Db> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        internal async Task<int> ExecuteCmd(string sql, CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var affectedLines = await connection.ExecuteAsync(sql);

                    return affectedLines;
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

        internal async Task<IEnumerable<T>> ExecuteCmd<T>(string sql, CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryAsync<T>(sql);

                    return result;
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
