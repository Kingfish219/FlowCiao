using System;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using SmartFlow.Models;
using SmartFlow.Persistence.Cache;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.SqlServer.Repositories
{
    public class LogCacheRepository : SmartFlowCacheRepository, ILogRepository
    {
        public LogCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public Task<bool> Create(Log log)
        {
            return Task.Run(() =>
            {
                try
                {
                    using var connection = GetDbConnection();
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
