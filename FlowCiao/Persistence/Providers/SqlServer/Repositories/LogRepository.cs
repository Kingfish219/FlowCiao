using System;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using FlowCiao.Models;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class LogRepository : FlowSqlServerRepository, ILogRepository
    {
        public LogRepository(FlowSettings smartFlowSettings) : base(smartFlowSettings) { }

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
