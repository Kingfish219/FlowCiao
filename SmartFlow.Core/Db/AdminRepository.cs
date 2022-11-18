using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Db
{
    public class AdminRepository:IAdminRepository
    {
        private readonly string _ConnectionStreing;
        public AdminRepository(string connectionStreing)
        {
            _ConnectionStreing = connectionStreing;
        }
        public Task<FuncResult<List<EditWorkFlowProcess>>> GetAllProccess()
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Query<EditWorkFlowProcess>($@"select [Id], [Name], [Owner], [EntityType], [IsActive], [IsDefultProccess] from [dbo].[Process]").ToList();
                        return new FuncResult<List<EditWorkFlowProcess>>
                        {
                            Success = true,
                            Data = result
                        };
                    }
                    catch (Exception e)
                    {
                        return new FuncResult<List<EditWorkFlowProcess>>
                        {
                            Success = false,
                            Message = e.Message
                        };
                    }

                }
            });
        }
        public Task<FuncResult> CreateProccess(EditWorkFlowProcess workFlowProcessViewModel)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Execute($@"insert into [dbo].[Process] ([Id], [Name], [Owner], [EntityType], [IsActive], [IsDefultProccess])
                            values('{workFlowProcessViewModel.Id}','{workFlowProcessViewModel.Name}','{workFlowProcessViewModel.Owner}','{workFlowProcessViewModel.EntityType}','{workFlowProcessViewModel.IsActive}','{workFlowProcessViewModel.IsDefultProccess}')");
                        return new FuncResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception e)
                    {
                        return new FuncResult
                        {
                            Success = false,
                            Message = e.Message
                        };
                    }

                }
            });
        }
        public Task<FuncResult> UpdateProccess(EditWorkFlowProcess workFlowProcessViewModel)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Execute($@"update [dbo].[Process] set [Name]='{workFlowProcessViewModel.Name}' , [Owner]='{workFlowProcessViewModel.Owner}' , [EntityType]='{workFlowProcessViewModel.EntityType}' , [IsActive]='{workFlowProcessViewModel.IsActive}' , [IsDefultProccess]='{workFlowProcessViewModel.IsDefultProccess}' where [Id]='{workFlowProcessViewModel.Id}'");
                        return new FuncResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception e)
                    {
                        return new FuncResult
                        {
                            Success = false,
                            Message = e.Message
                        };
                    }
                }
            });
        }
        public Task<FuncResult> DeleteProccess(EditWorkFlowProcess workFlowProcessViewModel)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Execute($@"
                            BEGIN TRANSACTION
                            BEGIN TRY
		                            delete from [dbo].[Process] where id = '{workFlowProcessViewModel.Id}'
		                            delete from [dbo].[Transition] where ProcessId = '{workFlowProcessViewModel.Id}'
		                            delete from [dbo].[TransitionAction] where TransitionId in (select Id from [dbo].[Transition] where ProcessId = '{workFlowProcessViewModel.Id}')
                            COMMIT TRANSACTION                     
                            END TRY                           
                            BEGIN CATCH                          
                            THROW                           
                            ROLLBACK TRANSACTION                 
                            END CATCH                          
                        ");
                        return new FuncResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception e)
                    {
                        return new FuncResult
                        {
                            Success = false,
                            Message = e.Message
                        };
                    }
                }
            });
        }
        public Task<List<EditWorkFlowTransition>> GetProcessTransitions(Guid ProcessId)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    connection.Open();

                    var result = connection.Query<EditWorkFlowTransition, EditWorkFlowCurrentStatus, EditWorkFlowNextStatus, EditWorkFlowAction, EditWorkFlowTransition>($@"
                        select t.Id,t.CurrentStateId,t.NextStateId,t.ProcessId,ta.Id as TransitionActionId,cs.Id as CurrentStateId,cs.IsFinalResponse,cs.IsVisible,cs.Name,cs.Number,cs.RequestResponse,cs.ResponseActions,cs.ResponseController
                        ,ns.Id NextStateId,ns.IsFinalResponse,ns.IsVisible,ns.Name,ns.Number,ns.RequestResponse,ns.ResponseActions,ns.ResponseController,a.Id as ActionId,a.ActionTypeCode,a.Name
                        from Transition t join TransitionAction ta on t.Id=ta.TransitionId 
				                          join Status cs on cs.Id=t.CurrentStateId
				                          join Status ns on ns.Id=t.NextStateId
				                          join Action a on a.Id=ta.ActionId
                        where t.ProcessId='{ProcessId}' order by case cs.Id 
                                                                    when 'E444A62F-9C36-40E5-87B9-AB7E153B7D10' then 0 --شروع
                                                                    when 'D002E7AA-2599-4B3C-89E6-DE8A74023E6A' then 1 --تایید ساب هلدینگ
                                                                    when 'E9FFB96F-2770-4763-A58D-E363D0903FB4' then 2 --تایید گروه
                                                                    when '04BE6210-CBF9-4158-A464-7973F4FFDD6F' then 3 --اقدام نشده
                                                                    when 'DF0F6E06-2AEC-4731-86DC-DF25981B852E' then 3 --اقدام نشده *
                                                                    when '1A20087B-D8F1-437F-BC60-4723E5AFEB1A' then 4 --در جریان
                                                                    else 10
                                                                  end 
                    ", (workFlowTransition, currentState, nextState, action) =>
                    {
                        workFlowTransition.CurrentState = currentState;
                        workFlowTransition.NextState = nextState;
                        workFlowTransition.Action = action;
                        return workFlowTransition;
                    },
                        splitOn: "CurrentStateId,NextStateId,ActionId"
                    ).AsQueryable().ToList();

                    return result;
                }
            });
        }
        public Task<List<EditWorkFlowAction>> GetAllAction()
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    connection.Open();

                    var result = connection.Query<EditWorkFlowAction>($@"select [Id] as ActionId, [Name] from Action").ToList();

                    return result;
                }
            });
        }
        public Task<List<EditWorkFlowCurrentStatus>> GetAllCurrentState()
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    connection.Open();

                    var result = connection.Query<EditWorkFlowCurrentStatus>($@"select [Id] as CurrentStateId, [Name] from [dbo].[Status]").ToList();

                    return result;
                }
            });
        }
        public Task<List<EditWorkFlowNextStatus>> GetAllNextState()
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    connection.Open();

                    var result = connection.Query<EditWorkFlowNextStatus>($@"select [Id] as NextStateId, [Name] from [dbo].[Status]").ToList();

                    return result;
                }
            });
        }
        public Task<FuncResult> CreateProcessTransition(EditWorkFlowTransition workFlowTransition)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Execute($@"
                            BEGIN TRANSACTION
                            BEGIN TRY
	                            insert into [dbo].[Transition] ([Id], [ProcessId], [CurrentStateId], [NextStateId]) 
                                                        values ('{workFlowTransition.Id}','{workFlowTransition.ProcessId}','{workFlowTransition.CurrentState.CurrentStateId}','{workFlowTransition.NextState.NextStateId}')
	                            insert into [dbo].[TransitionAction] ([Id], [ActionId], [TransitionId], [Priority]) 
                                                        values (newid(),'{workFlowTransition.Action.ActionId}','{workFlowTransition.Id}',Null)
                                COMMIT TRANSACTION
                            END TRY
                            BEGIN CATCH
                                THROW
                                ROLLBACK TRANSACTION
                            END CATCH
                        ");
                        return new FuncResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception e)
                    {
                        return new FuncResult
                        {
                            Success = false,
                            Message = e.Message
                        };
                    }

                }
            });
        }
        public Task<FuncResult> UpdateProcessTransition(EditWorkFlowTransition workFlowTransition)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Execute($@"
                            BEGIN TRANSACTION
                            BEGIN TRY
	                            update [dbo].[Transition] set [ProcessId]='{workFlowTransition.ProcessId}', [CurrentStateId]='{workFlowTransition.CurrentState.CurrentStateId}', [NextStateId]='{workFlowTransition.NextState.NextStateId}' where [Id] ='{workFlowTransition.Id}' 
	                            update [dbo].[TransitionAction] set [ActionId]='{workFlowTransition.Action.ActionId}', [TransitionId]='{workFlowTransition.Id}',  where [Id] ='{workFlowTransition.TransitionActionId}'
                                COMMIT TRANSACTION
                            END TRY
                            BEGIN CATCH
                                THROW
                                ROLLBACK TRANSACTION
                            END CATCH
                        ");
                        return new FuncResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception e)
                    {
                        return new FuncResult
                        {
                            Success = false,
                            Message = e.Message
                        };
                    }

                }
            });
        }
        public Task<FuncResult> DeleteProcessTransition(EditWorkFlowTransition workFlowTransition)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Execute($@"
                            BEGIN TRANSACTION
                            BEGIN TRY
	                            delete from [dbo].[Transition] where [Id] ='{workFlowTransition.Id}' 
	                            delete from [dbo].[TransitionAction] where [Id] ='{workFlowTransition.TransitionActionId}'
                                COMMIT TRANSACTION
                            END TRY
                            BEGIN CATCH
                                THROW
                                ROLLBACK TRANSACTION
                            END CATCH
                        ");
                        return new FuncResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception e)
                    {
                        return new FuncResult
                        {
                            Success = false,
                            Message = e.Message
                        };
                    }

                }
            });
        }
        public Task<List<EditCompanyProcess>> GetAllCompanyProcess(Guid CompanyId)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    connection.Open();

                    var result = connection.Query<EditCompanyProcess, EditProcess, EditCompanyProcess>($@"
                        select 
                        rt.Name as RequestTypeName ,
                        cp.Id,   
                        p.Id as ProcessId,       
                        p.Name as ProcessName    
                        from CompanyProcess cp          
                            join Process p on cp.ProcessId=p.Id           
                            join RequestType rt on rt.Id = cp.RequestTypeId
                        where cp.CompanyId = '{CompanyId}'     
                    ", (companyProcess, EditProcess) =>
                    {
                        companyProcess.Process = EditProcess;
                        return companyProcess;
                    },
                        splitOn: "ProcessId"
                    ).AsQueryable().ToList();

                    return result;
                }
            });
        }
        public Task<List<EditProcess>> GetAllEditProcess()
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    connection.Open();

                    var result = connection.Query<EditProcess>($@"select id as ProcessId,Name as ProcessName from Process").ToList();

                    return result;
                }
            });
        }
        public Task<FuncResult> UpdateCompanyProcess(EditCompanyProcess companyProcess)
        {
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_ConnectionStreing))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Execute($@"
                                BEGIN TRANSACTION
                                BEGIN TRY
	                                update [dbo].[CompanyProcess] set ProcessId='{companyProcess.Process.ProcessId}' where Id='{companyProcess.Id}'
                                    COMMIT TRANSACTION
                                END TRY
                                BEGIN CATCH
                                    THROW
                                    ROLLBACK TRANSACTION
                                END CATCH
                        ");
                        return new FuncResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception e)
                    {
                        return new FuncResult
                        {
                            Success = false,
                            Message = e.Message
                        };
                    }

                }
            });
        }
    }
}
