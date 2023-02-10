using Dapper;
using SmartFlow.Core.Db.SqlServer;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using Activity = SmartFlow.Core.Models.Activity;
using Process = SmartFlow.Core.Models.Process;

namespace SmartFlow.Core.Repositories
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly string _connectionString;

        public ProcessRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        public Task<bool> CompleteProgressAction(ProcessStep processStep, ProcessAction action)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var result = connection.Execute($@"UPDATE [ActiveProcessStep] SET [IsCompleted] = 1
                                                       WHERE [EntityId] = '{processStep.Entity.Id}' AND [ActionId] = '{action.Id}'");

                    return result > 0;
                }
            });
        }

        public Task<bool> CreateProcessStep(ProcessStep entity)
        {
            return Task.Run(() =>
            {
                var result = false;

                foreach (var transitionAction in entity.TransitionActions)
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        Guid userIdEmpty = Guid.Empty;
                        var inserted = connection.Execute($@"
                                    INSERT INTO [dbo].[ActiveProcessStep]
                                               ([Id]
                                               ,[ProcessId]
                                               ,[EntityId]
                                               ,[TransitionId]
                                               ,[ActionId]
                                               ,[IsCompleted]
                                               ,[UserIdAction]
                                               ,[EntityType])
                                         VALUES
                                               ('{Guid.NewGuid()}'
                                               ,'{entity.Process.Id}'
                                               ,'{entity.Entity.Id}'
                                               ,'{transitionAction.Transition.Id}'
                                               ,'{transitionAction.Action.Id}'
                                               ,0
                                               ,'{userIdEmpty}'
                                               ,{entity.EntityType})
                                    ");

                        result = inserted > 0;
                    }
                }

                return result;
            });
        }

        public Task<bool> RemoveActiveProcessStep(ProcessEntity entity)
        {
            return Task.Run(() =>
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                var result = connection.Execute($@"DELETE FROM [dbo].[ActiveProcessStep]
                                                WHERE [EntityId] = '{entity.Id}'
                                    ");

                return result > 0;
            });
        }

        public Task<List<Process>> Get<T>(Guid processId = default, string key = default) where T : Process
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"SELECT p.*
                                , t.Id TransitionId
                                , t.ProcessId
                                , s.Id StateId
                                , s.[Name]
                                , s1.Id StateId
                                , s1.[Name]
                                FROM [SmartFlow].Process p
                                JOIN SmartFlow.Transition t on p.Id = t.ProcessId
                                JOIN SmartFlow.[State] s on s.Id = t.CurrentStateId
                                JOIN SmartFlow.[State] s1 on s1.Id = t.NextStateId
                                WHERE
                                  (p.[Id] = @ProcessId OR ISNULL(@ProcessId, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER)) AND
                                  (p.[FlowKey] = @FlowKey OR ISNULL(@FlowKey, '') = '')";

                    var smartFlows = new List<Process>();
                    connection.Query<Process, Transition, State, State, Process>(sql,
                        (process, transition, currentState, nextState) =>
                        {
                            var smartFlow = smartFlows.FirstOrDefault(x => x.Id == process.Id);
                            if (smartFlow is null)
                            {
                                smartFlow = process;
                                smartFlows.Add(smartFlow);
                            }

                            if (smartFlow.Transitions is null)
                            {
                                smartFlow.Transitions = new List<Transition>();
                            }

                            transition.From = currentState;
                            transition.To = nextState;

                            smartFlow.Transitions.Add(transition);

                            return smartFlow;
                        }, splitOn: "TransitionId, StateId, StateId", param: new { ProcessId = processId, FlowKey = key }).ToList();

                    return smartFlows;
                }
            });
        }

        public Task<ProcessStep> GetActiveProcessStep(ProcessEntity entity)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var result = connection.QueryFirstOrDefault<ProcessStep>($"SELECT TOP 1 * FROM [ActiveProcessStep] WHERE [EntityId] = '{entity.Id}'");

                    return result;
                }
            });
        }
        /// <summary>
        ///It tells us what transitions can be selected from the Transition table for the ticket according to its status and
        ///according to the process assigned to it.
        ///Each transfer should be selected by what action on the user's side. (user actions are deny and confirm).
        ///What action should be taken for each transition is done by returning the ActivityCode        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public Task<List<TransitionAction>> GetActiveTransitions(ProcessEntity entity, Guid ProcessId)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var result = connection.Query<TransitionAction, Transition, ProcessAction, TransitionAction>($@"
                            SELECT ta.[Id], ta.TransitionId AS [TransitionId], t.CurrentStateId, t.NextStateId, t.ProcessId, ta.ActionId  AS [ActionId], a.ActionTypeCode, a.[Name], a.ProcessId FROM [ActiveProcessStep] p
                            INNER JOIN [TransitionAction] ta ON ta.[ActionId] = p.ActionId and ta.TransitionId = p.TransitionId
                            INNER JOIN [Transition] t ON ta.TransitionId = t.Id AND t.ProcessId = '{ProcessId}'
                            INNER JOIN [Action] a ON a.Id = ta.ActionId
                            WHERE p.EntityId = '{entity.Id}'
                    ", (a, s, r) =>
                        {
                            a.Transition = s;
                            a.Action = r;
                            return a;
                        },
                        splitOn: "TransitionId,ActionId"
                    ).AsQueryable().ToList();

                    return result;
                }
            });
        }

        public Task<List<Activity>> GetStateActivities(State state, Group group)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var result = connection.Query<Activity>($@"SELECT * FROM [Activity] A 
                                            INNER JOIN [StateActivity] S ON A.[Id] = S.[ActivityId] 
                                            WHERE S.[StateId] = '{state.Id}'").ToList();

                    return result;
                }
            });
        }
        public Task<List<Activity>> GetTransitionActivities(Transition transition)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var result = connection.Query<Activity>($@"
                          select [Id], [Name], [ActivityTypeCode], [Process] from Activity where 
                          ActivityTypeCode in 
		                        (select ta.[ActivityTypeCode] from TransitionActivity ta where ta.[TransitionId]='{transition.Id}')
                    ").ToList();

                    return result;
                }
            });
        }

        public Task<List<TransitionAction>> GetStateTransitions(Process process, State state)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    //var result = connection.Query<TransitionAction, Transition, Action, TransitionAction>($@"
                    //        SELECT ta.[Id], t.[Id] AS [TransitionId], t.[NextStateId],t.[ProcessId],a.[ProcessId] ,t.[CurrentStateId], a.[Id] AS [ActionId], a.[Name], a.[ActionTypeCode] FROM [TransitionAction] ta
                    //        INNER JOIN [Transition] t ON ta.[TransitionId] = t.[Id]
                    //        INNER JOIN [Action] a ON ta.[ActionId] = a.[Id]           
                    //        WHERE t.[ProcessId] = '{process.Id}' AND a.[ProcessId] = '{process.Id}' AND t.CurrentStateId = '{state.Id}'
                    //", (a, s, r) =>
                    //{
                    //    a.Transition = s;
                    //    a.Action = r;
                    //    return a;
                    //},
                    //    splitOn: "TransitionId,ActionId"
                    //).AsQueryable().ToList();
                    var result = connection.Query<TransitionAction, Transition, ProcessAction, TransitionAction>($@"
                            SELECT ta.[Id], t.[Id] AS [TransitionId], t.[NextStateId],t.[ProcessId],a.[ProcessId] ,t.[CurrentStateId], a.[Id] AS [ActionId], a.[Name], a.[ActionTypeCode] FROM [TransitionAction] ta
                            INNER JOIN [Transition] t ON ta.[TransitionId] = t.[Id]
                            INNER JOIN [Action] a ON ta.[ActionId] = a.[Id]           
                            WHERE t.[ProcessId] = '{process.Id}' AND t.CurrentStateId = '{state.Id}'
                    ", (a, s, r) =>
                    {
                        a.Transition = s;
                        a.Action = r;
                        return a;
                    },
                    splitOn: "TransitionId,ActionId"
                ).AsQueryable().ToList();

                    return result;
                }
            });
        }

        public Task<List<TransitionAction>> GetStepTransitions(Process process, State state)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var result = connection.Query<TransitionAction, Transition, ProcessAction, TransitionAction>($@"
                            SELECT ta.[Id], t.[Id] AS [TransitionId], t.[NextStateId], t.[CurrentStateId], a.[Id] AS [ActionId], a.[Name], a.[ActionTypeCode] FROM [TransitionAction] ta
                            INNER JOIN [Transition] t ON ta.[TransitionId] = t.[Id]
                            INNER JOIN [Action] a ON ta.[ActionId] = a.[Id]           
                            WHERE t.[ProcessId] = '{process.Id}' AND a.[ProcessId] = '{process.Id}'
                    ", (a, s, r) =>
                        {
                            a.Transition = s;
                            a.Action = r;
                            return a;
                        },
                        splitOn: "TransitionId,ActionId"
                    ).AsQueryable().ToList();

                    return result;
                }
            });
        }

        public Task<State> GetState(Guid stateId)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var result = connection.QueryFirstOrDefault<State>($@"SELECT * FROM [Status] WHERE Id = '{stateId}'");

                    return result;
                }
            });
        }
        public Guid GetUserIdManger(Guid processId, Guid userId, Guid requestId, Guid statusId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var result = connection.QueryFirst<Guid>($@"exec SP_DefaultProcess_GetUserIdManger '{processId}','{userId}','{requestId}','{statusId}'");
                    return result;
                }
            }
            catch (Exception)
            {
                return Guid.Empty;

            }
        }

        public Task<bool> UpdateActiveProcessStepUserActionId(Guid userId, Guid entityId)
        {
            return Task.Run(() =>
            {
                try
                {
                    using var connection = new SqlConnection(_connectionString);
                    connection.Open();
                    var result = connection.Execute($@"exec [dbo].[sp_DefaultProcess_UpdateActiveProcessStepUserActionId] '{entityId}','{userId}'");
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> SetToHistory(Guid entityId)
        {
            return Task.Run(() =>
            {
                try
                {
                    using var connection = new SqlConnection(_connectionString);
                    connection.Open();

                    connection.Execute($@"exec sp_ProcessStepHistory_GetbyId '{entityId}'");

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
        public Task<bool> SetToHistoryForGoToStep(Guid newId, Guid entityId, Guid targetStatus, Guid processGuid)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        connection.Execute($@"insert into ProcessStepHistory([Id], [ProcessId], [EntityId], [TransitionId], [ActionId], [IsCompleted], [CreatedOn], [UserIdAction], [EntityType],[OperationTypeCode]) values ('{newId}','{processGuid}','{entityId}',(select top 1 tr.Id from Transition tr where tr.NextStateId='{targetStatus}'),'26503E54-F151-4E39-B47A-77B22016E413', 1,getDate(),null,1,1)");

                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
        public Task<ProcessStep> GetLastProcessStepHistoryItem(Guid ticketId)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var result = connection.Query<ProcessStep>($@"
                         exec sp_ProcessStepHistory_GetLastProcessStepHistoryItem '{ticketId}'            
                    ").FirstOrDefault();
                    return result;
                }
            });
        }
        public Task<bool> AddProcessStepHistoryActivity(ProcessStepHistoryActivity processStepHistoryActivity)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        var result = connection.Query($@"exec sp_ProcessStepHistory_AddProcessStepHistoryActivity '{processStepHistoryActivity.ActivityId}',N'{processStepHistoryActivity.ActivityName}','{processStepHistoryActivity.StepType}','{processStepHistoryActivity.ProcessStepHistoryId}'");
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
        public Task<State> GetStatusById(Guid statusId)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        var result = connection.Query<State>($@"select [Id] ,[Name] ,[RequestResponse],[ResponseController],[ResponseActions],[IsFinalResponse],[Number]  from Status where id = '{statusId}'").FirstOrDefault();
                        return result;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<FuncResult> RemoveEntireFlow(ProcessStep processStep)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        connection.Execute($@"exec [usp_ProcessStep_DeleteEntireFlow] '{processStep.Id}'");

                        return new FuncResult
                        {
                            Success = true
                        };
                    }
                }
                catch (Exception e)
                {
                    return new FuncResult
                    {
                        Success = true,
                        Message = e.Message
                    };
                }
            });
        }

        public Task<bool> ProcessStepCommentRead(ProcessEntity entity)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@EntityId", entity.Id);

                        connection.Execute(@"[dbo].[usp_ProcessStepComment_Modify]"
                            , parameters
                            , commandType: CommandType.StoredProcedure);

                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> ApplyProcessStepCommentAttachment(Guid processStepCommentId, Guid attachmentId)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@AttachmentId", attachmentId);
                        parameters.Add("@ProcessStepId", processStepCommentId);

                        connection.Execute(@"[dbo].[usp_ProcessStep_ApplyCommentAttachment]"
                                        , parameters
                                        , commandType: CommandType.StoredProcedure);

                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<FuncResult> ApplyProcessStepComment(ProcessEntity entity, ProcessUser processUser, string comment, List<Guid> attachmentIds = default)
        {
            return Task.Run(() =>
            {
                try
                {
                    SqlConnection con = new SqlConnection(
                        _connectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ProcessStep_ApplyComment]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Comment", comment);
                    cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EntityId", entity.Id);
                    cmd.Parameters.AddWithValue("@UserId", processUser.Id);
                    cmd.Parameters.AddWithValue("@StatusId", entity.LastStatus);
                    cmd.Parameters.AddWithValue("@IsUser", true);
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier, 16);
                    cmd.Parameters["@Id"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    var guid = (Guid)cmd.Parameters["@Id"].Value;
                    con.Close();
                    return new FuncResult
                    {
                        Success = true,
                        Message = guid.ToString()
                    };

                    //using (var connection = new SqlConnection(_connectionString))
                    //{
                    //    var parameters = new DynamicParameters();
                    //    parameters.Add("@Comment", comment);
                    //    parameters.Add("@CreatedOn", DateTime.Now);
                    //    parameters.Add("@Id", null, direction: ParameterDirection.Output, size:16);
                    //    parameters.Add("@EntityId", entity.Id);
                    //    parameters.Add("@UserId", processUser.Id);
                    //    parameters.Add("@StatusId", entity.LastStatus);
                    //    parameters.Add("@IsUser", true);

                    //    var result = connection.Execute($@"[dbo].[usp_ProcessStep_ApplyComment]"
                    //                    , parameters
                    //                    , commandType: System.Data.CommandType.StoredProcedure);

                    //    var guid = parameters.Get<Guid>("Id");

                    //    return new FuncResult
                    //    {
                    //        Success = true,
                    //        Message = guid.ToString()
                    //    };

                    //    return new FuncResult
                    //    {
                    //        Success = true
                    //    };
                    //}
                }
                catch (Exception e)
                {
                    return new FuncResult
                    {
                        Success = true,
                        Message = e.Message
                    };
                }
            });
        }

        public State GetStartState()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                var result = connection.QueryFirstOrDefault<State>($@"SELECT * FROM [Status] WHERE IsStart = 1");

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<Guid> Create<T>(Process entity) where T : Process
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    entity.FlowKey
                };

                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_Process_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }

        public Task<Process> GetProcess(Guid userId, Guid requestTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
