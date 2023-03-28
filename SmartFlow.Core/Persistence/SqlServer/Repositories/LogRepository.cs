using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using SmartFlow.Core.Models;
using SmartFlow.Core.Persistence.Interfaces;

namespace SmartFlow.Core.Persistence.SqlServer.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string _connectionString;
        public LogRepository(SmartFlowSettings smartFlowSettings)
        {
            _connectionString = smartFlowSettings.ConnectionString;
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
