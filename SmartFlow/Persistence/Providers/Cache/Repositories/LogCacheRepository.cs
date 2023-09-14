using System;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class LogCacheRepository : FlowCacheRepository, ILogRepository
    {
        public LogCacheRepository(FlowHub smartFlowHub) : base(smartFlowHub)
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
