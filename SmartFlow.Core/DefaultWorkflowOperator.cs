using SmartFlow.Core.Db;
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

        public DefaultWorkflowOperator()
        {
        }

        public Task<ProcessResult> AdvanceAsync(Entity entity
            , ProcessUser user
            , ProcessStepInput input
            , IEntityRepository entityRepository
            , EntityCommandType commandType = EntityCommandType.Update
            , Dictionary<string, object> parameters = null)
        {
            return Task.Run(() =>
            {
                try
                {
                    var logRepository = new LogRepository(_connectionString);
                    var processRepository = new DefaultProcessRepository(_connectionString);
                    var processStepManager = new DefaultProcessStepManager(processRepository);
                    ProcessStep processStep;
                    if (commandType == EntityCommandType.Create)
                    {
                        processStep = processStepManager.InitializeActiveProcessStep(user.Id, entity, true);
                        input.ActionCode = 1;
                    }
                    else
                    {
                        processStep = processStepManager.GetActiveProcessStep(user.Id, entity);
                    }

                    if (processStep is null)
                    {
                        return new ProcessResult
                        {
                            Status = ProcessResultStatus.Completed
                        };
                    }

                    var processStepContext = new ProcessStepContext
                    {
                        ProcessStep = processStep,
                        ProcessUser = user,
                        ProcessStepInput = input,
                        Data = parameters,
                        EntityCommandType = commandType,
                    };

                    var handlers = WorkflowHandlerFactory.BuildHandlers(
                        processStepContext,
                        processRepository,
                        processStepManager,
                        entityRepository,
                        logRepository,
                        _connectionString
                       );

                    var result = handlers.Peek().Handle(processStepContext);

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

        public ProcessResult Start(Process process)
        {
            throw new NotImplementedException();
        }
    }
}
