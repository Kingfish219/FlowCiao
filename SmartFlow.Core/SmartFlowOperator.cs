using SmartFlow.Core.Db;
using SmartFlow.Core.Handlers;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using SmartFlow.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Interfaces;

namespace SmartFlow.Core
{
    public class SmartFlowOperator : IStateMachineOperator
    {
        private readonly IStateMachineBuilder _builder;
        private readonly SmartFlowSettings _settings;

        public SmartFlowOperator(SmartFlowSettings settings, IStateMachineBuilder builder)
        {
            _settings = settings;
            _builder = builder;
        }

        public bool RegisterFlow<TFlow>() where TFlow : IStateMachine, new()
        {
            var stateMachine = _builder
                .Build<TFlow>();

            return stateMachine != null;
        }

        public Task<ProcessResult> ExecuteAsync(IStateMachine process)
        {
            throw new NotImplementedException();
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
                    var logRepository = new LogRepository(_settings.ConnectionString);
                    var processRepository = new ProcessRepository(_settings.ConnectionString);
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
                        _settings.ConnectionString
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

        public ProcessResult Execute(IStateMachine stateMachine)
        {
            throw new NotImplementedException();
        }
    }
}
