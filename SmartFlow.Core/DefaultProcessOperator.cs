using SmartFlow.Core.Db;
using SmartFlow.Core.Handlers;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using SmartFlow.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFlow.Core
{
    public class DefaultProcessOperator : IProcessOperator
    {
        private readonly string _connectionString;

        public DefaultProcessOperator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DefaultProcessOperator()
        {
        }

        public Task<ProcessResult> AdvanceAsync(ProcessEntity entity
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
                    var processRepository = new ProcessRepository(_connectionString);
                    var processStepManager = new ProcessStepService(processRepository);
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

                    var handlers = ProcessHandlerFactory.BuildHandlers(
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
