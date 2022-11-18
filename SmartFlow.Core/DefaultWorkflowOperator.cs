using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFlow.Core
{
    public class DefaultWorkflowOperator : IWorkflowOperator
    {
        private readonly string _connectionString;

        public DefaultWorkflowOperator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<ProcessResult> AdvanceAsync(Entity entity
            , ProcessUser user
            , ProcessStepInput input
            , IEntityRepository entityRepository
            , IEntityCreateHistory entityCreateHistory
            , EntityCommandType commandType = EntityCommandType.Update
            , Dictionary<string, object> parameters = null)
        {
            return Task.Run(() =>
            {
                try
                {
                    var logRepository = new LogRepository(_connectionString);
                    var processRepository = new DefaultProcessRepository(_connectionString);
                    var processStepManager = new DefaultProcessStepManager(processRepository, entityCreateHistory, entity);
                    ProcessStep processStep;
                    if (commandType == EntityCommandType.Create)
                    {
                        processStep = processStepManager.InitializeActiveProcessStep(user.Id, true);
                        input.ActionCode = 1;
                    }
                    else
                    {
                        processStep = processStepManager.GetActiveProcessStep(user.Id);
                    }

                    if (processStep is null)
                    {
                        return new ProcessResult
                        {
                            Status = ProcessResultStatus.Completed
                        };
                    }

                    var processStepContext = new ProcessStepContext();
                    foreach (var element in parameters)
                    {
                        processStepContext.Context.Add(element.Key, element.Value);
                    }

                    var handlers = WorkflowHandlerFactory.BuildHandlers(
                        commandType
                        , entity
                        , processRepository
                        , processStepManager
                        , entityRepository
                        , logRepository
                        , input
                        , user
                        , _connectionString
                       );

                    var result = handlers.Peek().Handle(processStep, user, processStepContext);

                    return result;
                }
                catch (Exception exception)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Failed,
                        Message = exception.Message
                    };
                }
            });
        }
    }
}
