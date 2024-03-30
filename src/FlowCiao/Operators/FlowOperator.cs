using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Handle;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using FlowCiao.Services;

namespace FlowCiao.Operators
{
    public class FlowOperator : IFlowOperator
    {
        private readonly ProcessExecutionService _processExecutionService;
        private readonly ProcessHandlerFactory _processHandlerFactory;
        private readonly IProcessService _processService;

        public FlowOperator(ProcessHandlerFactory processHandlerFactory,
            ProcessExecutionService processExecutionService,
            IProcessService processService)
        {
            _processHandlerFactory = processHandlerFactory;
            _processExecutionService = processExecutionService;
            _processService = processService;
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

        public async Task<ProcessResult> Fire(string key,
            int action,
            Dictionary<string, object> data = null)
        {
            var processExecution = (await _processExecutionService.Get()).SingleOrDefault();

            try
            {
                if (processExecution is null)
                {
                    var process = (await _processService.Get(key: key))
                        .SingleOrDefault();
                    if (process is null)
                    {
                        throw new FlowCiaoException("Invalid flow key!");
                    }

                    processExecution = await _processExecutionService.InitializeProcessExecution(process);
                }

                var activeProcessStep = processExecution.ActiveExecutionStep;
                if (activeProcessStep is null)
                {
                    throw new FlowCiaoProcessExecutionException("No active steps to fire");
                }

                if (activeProcessStep.Details
                        .SingleOrDefault(x => x.Transition.Actions.FirstOrDefault()!.Code == action) is null)
                {
                    throw new FlowCiaoProcessExecutionException("Action is invalid!");
                }

                var processStepContext = InitiateContext(action, data, processExecution, processExecution.ActiveExecutionStep);
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
        
        public async Task<State> GetFLowState(string key)
        {
            var processExecution = (await _processExecutionService.Get()).SingleOrDefault();
            if (processExecution is null)
            {
                var process = (await _processService.Get(key: key))
                        .SingleOrDefault();
                if (process is null)
                {
                    throw new FlowCiaoException("Invalid key!");
                }

                processExecution = await _processExecutionService.InitializeProcessExecution(process);
            }

            return processExecution.State;
        }
    }
}
