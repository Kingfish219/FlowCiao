using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FlowCiao.Models;
using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;
using Process = FlowCiao.Models.Flow.Process;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class ProcessRepository : FlowSqlServerRepository, IProcessRepository
    {
        public ProcessRepository(FlowSettings settings) : base(settings) { }
        
        public Task<List<Process>> Get(Guid processId = default, string key = default)
        {
            return Task.Run(() =>
            {
                using (var connection = GetDbConnection())
                {
                    connection.Open();

                    var sql = @"SELECT p.*
                                , t.Id TransitionId
                                , t.ProcessId
                                , s.Id StateId
                                , s.[Name]
                                , s1.Id StateId
                                , s1.[Name]
                                FROM [FlowCiao].Process p
                                JOIN FlowCiao.Transition t on p.Id = t.ProcessId
                                JOIN FlowCiao.[State] s on s.Id = t.CurrentStateId
                                JOIN FlowCiao.[State] s1 on s1.Id = t.NextStateId
                                WHERE
                                  (p.[Id] = @ProcessId OR ISNULL(@ProcessId, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER)) AND
                                  (p.[Key] = @Key OR ISNULL(@Key, '') = '')";

                    var processes = new List<Process>();
                    _ = connection.Query<Process, Transition, State, State, Process>(sql,
                        (process, transition, currentState, nextState) =>
                        {
                            var selectedProcess = processes.FirstOrDefault(x => x.Id == process.Id);
                            if (selectedProcess is null)
                            {
                                selectedProcess = process;
                                processes.Add(selectedProcess);
                            }

                            selectedProcess.Transitions ??= new List<Transition>();

                            transition.From = currentState;
                            transition.To = nextState;

                            selectedProcess.Transitions.Add(transition);

                            return selectedProcess;
                        }, splitOn: "TransitionId, StateId, StateId", param: new { ProcessId = processId, Key = key }).ToList();

                    return processes;
                }
            });
        }

        public Task<Guid> Modify(Process entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id, entity.Key
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_Process_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }
    }
}
