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
        private readonly ProcessExecutionService _processExecutionService;
        private readonly ProcessHandlerFactory _processHandlerFactory;
        private readonly IProcessService _processService;

        public SmartFlowOperator(ProcessHandlerFactory processHandlerFactory,
            ProcessExecutionService processExecutionService,
            SmartFlowSettings smartFlowSettings,
            IProcessService processService)
        {
            _processHandlerFactory = processHandlerFactory;
            _processExecutionService = processExecutionService;
            _processService = processService;

            var processes = new List<Process>();
            var processExecutions = new List<ProcessExecution>();
            
            if (smartFlowSettings.PersistFlow)
            {
                processes = processService.Get().GetAwaiter().GetResult();
                processExecutions = processExecutionService.Get().GetAwaiter().GetResult();
            }


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
            var processExecution = _processExecutionService.Get().GetAwaiter().GetResult().SingleOrDefault();

            try
            {
                if (processExecution is null)
                {
                    var process = _processService.Get(key: smartFlowKey).GetAwaiter().GetResult().SingleOrDefault();
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
