using System;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using SmartFlow.Models;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.Providers.Cache.Repositories
{
    public class LogCacheRepository : SmartFlowCacheRepository, ILogRepository
    {
        public LogCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public Task<bool> Create(Log log)
        {
            throw new NotImplementedException();

            //return Task.Run(() =>
            //{
            //    try
            //    {
            //        using var connection = GetDbConnection();
            //        connection.Open();
            //        connection.Insert(log);

            //        return true;
            //    }
            //    catch (Exception)
            //    {
            //        return false;
            //    }
            //});
        }
    }
}
