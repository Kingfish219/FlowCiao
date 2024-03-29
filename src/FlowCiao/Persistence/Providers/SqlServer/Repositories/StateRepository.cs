﻿using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using FlowCiao.Models;
using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class StateRepository : FlowSqlServerRepository, IStateRepository
    {
        public StateRepository(FlowSettings settings) : base(settings) { }

        public Task<Guid> Modify(State entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    entity.Name
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_State_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }

        public Task AssociateActivities(State entity, Activity activity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    StateId = entity.Id,
                    ActivityId = activity.Id
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.usp_StateActivity_Modify, toInsert, commandType: CommandType.StoredProcedure);

                return entity.Id;
            });
        }
    }
}
