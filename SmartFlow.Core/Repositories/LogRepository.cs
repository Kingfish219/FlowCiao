using Dapper.Contrib.Extensions;
using SmartFlow.Core.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SmartFlow.Core.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string _connectionString;
        public LogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<bool> Create(Log log)
        {
            return Task.Run(() =>
            {
                try
                {
                    using var connection = new SqlConnection(_connectionString);
                    connection.Open();
                    connection.Insert(log);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            });
        }
    }
}
