using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Handlers;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;
using SmartFlow.Services;

namespace SmartFlow.Operators
{
    public class SmartFlowOperator : ISmartFlowOperator
    {
        private readonly List<Process> _processHub;
        private readonly List<ProcessExecution> _processExecutionHub;
        private readonly ProcessExecutionService _processExecutionService;
        private readonly ProcessHandlerFactory _processHandlerFactory;

        public SmartFlowOperator(ProcessHandlerFactory processHandlerFactory,
            ProcessExecutionService processExecutionService,
            SmartFlowSettings smartFlowSettings,
            ProcessService processService)
        {
            _processHandlerFactory = processHandlerFactory;
            _processExecutionService = processExecutionService;
            if (smartFlowSettings.PersistFlow)
            {
                _processHub = processService.Get().GetAwaiter().GetResult();
                _processExecutionHub = _processExecutionService.Get().GetAwaiter().GetResult();
            }
            else
            {
                _processExecutionHub = new List<ProcessExecution>();
                _processHub = new List<Process>();
            }
        }

        public Task<bool> RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : Process, new()
        {
            return Task.Run(() =>
            {
                _processHub.Add(smartFlow);

                return true;
            });
        }

        public Task<ProcessResult> ExecuteAsync(ISmartFlow process)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessResult> AdvanceAsync(ProcessEntity entity, ProcessUser user, ProcessStepInput input, IEntityRepository entityRepository,
            EntityCommandType commandType = EntityCommandType.Update, Dictionary<string, object> parameters = null)
        {
            throw new NotImplementedException();
        }

        public ProcessResult Fire(string smartFlowKey,
            int action,
            Dictionary<string, object> data = null)
        {
            var processExecution = _processExecutionHub.SingleOrDefault(x => x.Process?.FlowKey?.Equals(smartFlowKey) ?? false);

            try
            {
                if (processExecution is null)
                {
                    var process = _processHub.Single(x => x.FlowKey.Equals(smartFlowKey));
                    processExecution = _processExecutionService.InitializeProcessExecution(process).GetAwaiter().GetResult();
                }

                var activeProcessStep = processExecution.ExecutionSteps
                    .SingleOrDefault(x => !x.IsCompleted);

                if (activeProcessStep is null)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Completed
                    };
                }

                if (activeProcessStep.Details
                        .SingleOrDefault(x => x.Transition.Actions.FirstOrDefault()!.Code == action) is null)
                {
                    throw new SmartFlowProcessExecutionException("Action not supported!");
                }

                var processStepContext = new ProcessStepContext
                {
                    ProcessExecution = processExecution,
                    ProcessExecutionStep = activeProcessStep,
                    ProcessExecutionStepDetail = activeProcessStep.Details.Single(x => x.Transition.Actions.FirstOrDefault().Code == action),
                    Data = data
                };

                var handlers = _processHandlerFactory.BuildHandlers(processStepContext);

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
        }
    }
}
