using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Handlers;
using SmartFlow.Interfaces;
using SmartFlow.Models;
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
            IProcessService processService)
        {
            _processHandlerFactory = processHandlerFactory;
            _processExecutionService = processExecutionService;
            _processService = processService;
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
                    var process = _processService.Get(key: smartFlowKey)
                        .GetAwaiter().GetResult()
                        .SingleOrDefault();
                    if (process is null)
                    {
                        throw new Exception("Invalid Smartflow key!");
                    }

                    processExecution = _processExecutionService.InitializeProcessExecution(process)
                        .GetAwaiter().GetResult();
                }

                var activeProcessStep = processExecution.ExecutionSteps
                    .SingleOrDefault(x => !x.IsCompleted);
                if (activeProcessStep is null)
                {
                    throw new SmartFlowProcessExecutionException("No active steps to fire");
                }

                if (activeProcessStep.Details
                        .SingleOrDefault(x => x.Transition.Actions.FirstOrDefault()!.Code == action) is null)
                {
                    throw new SmartFlowProcessExecutionException("Action is invalid!");
                }

                var processStepContext = InitiateContext(action, data, processExecution, activeProcessStep);
                var handlers = _processHandlerFactory.BuildHandlers();
                
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

        private static ProcessStepContext InitiateContext(int action,
            Dictionary<string, object> data,
            ProcessExecution processExecution,
            ProcessExecutionStep activeProcessStep)
        {
            return new ProcessStepContext
            {
                ProcessExecution = processExecution,
                ProcessExecutionStep = activeProcessStep,
                ProcessExecutionStepDetail = activeProcessStep.Details.Single(x => x.Transition.Actions.FirstOrDefault().Code == action),
                Data = data
            };
        }
    }
}
