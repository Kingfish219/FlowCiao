using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.SqlServer.Repositories
{
    public class ProcessExecutionRepository : IProcessExecutionRepository
    {
        private readonly string _connectionString;

        public ProcessExecutionRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        public Task<List<ProcessExecution>> Get(Guid id = default, Guid processId = default)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"SELECT pe.*,
	                                   p.Id ProcessId,
	                                   p.FlowKey
                                FROM [SmartFlow].ProcessExecution pe
                                JOIN SmartFlow.Process p ON p.Id = pe.ProcessId
                                WHERE
                                  (pe.[Id] = @Id OR ISNULL(@Id, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER)) AND
                                  (pe.[ProcessId] = @ProcessId OR ISNULL(@ProcessId, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER))";

                    var result = connection.Query<ProcessExecution, Process, ProcessExecution>(query,
                        (processExecution, process) =>
                        {
                            processExecution.Process = process;

                            return processExecution;
                        },
                        splitOn: "ProcessId",
                        param: new { ProcessId = processId, Id = id }).ToList();
                    return result;
                }
            });
        }

        public Task<Guid> Modify(ProcessExecution entity)
        {
            return Task.Run(() =>
            {
                var toModify = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    entity.CreatedOn,
                    ProcessId = entity.Process.Id,
                    entity.State,
                    entity.Progress
                };

                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_ProcessExecution_Modify, toModify, commandType: CommandType.StoredProcedure);
                entity.Id = toModify.Id;

                return entity.Id;
            });
        }
    }
}
