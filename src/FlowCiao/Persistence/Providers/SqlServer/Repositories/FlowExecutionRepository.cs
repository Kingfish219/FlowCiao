using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class FlowExecutionRepository : FlowSqlServerRepository, IFlowExecutionRepository
    {
        public FlowExecutionRepository(FlowSettings settings) : base(settings) { }

        public async Task<List<FlowExecution>> Get(Guid id = default, Guid flowId = default)
        {
                using var connection = GetDbConnection();

                connection.Open();
                const string query = @"SELECT pe.*,
	                                   p.Id FlowId,
	                                   p.[Key]
                                FROM [FlowCiao].FlowExecution pe
                                JOIN FlowCiao.Flow p ON p.Id = pe.FlowId
                                WHERE
                                  (pe.[Id] = @Id OR ISNULL(@Id, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER)) AND
                                  (pe.[FlowId] = @FlowId OR ISNULL(@FlowId, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER))";
                var result = (await connection.QueryAsync<FlowExecution, Flow, FlowExecution>(query,
                    (flowExecution, flow) =>
                    {
                        flowExecution.Flow = flow;

                        return flowExecution;
                    },
                    splitOn: "FlowId",
                    param: new { FlowId = flowId, Id = id }))?.ToList();

                return result;
        }

        public Task<Guid> Modify(FlowExecution entity)
        {
            return Task.Run(() =>
            {
                using var connection = GetDbConnection();

                var toModify = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    entity.CreatedOn,
                    ProcessId = entity.Flow.Id,
                    entity.ExecutionState,
                    State = entity.State.Id,
                    entity.Progress
                };

                connection.Open();
                connection.Execute(ConstantsProvider.Usp_ProcessExecution_Modify, toModify, commandType: CommandType.StoredProcedure);
                entity.Id = toModify.Id;

                return entity.Id;
            });
        }
    }
}
