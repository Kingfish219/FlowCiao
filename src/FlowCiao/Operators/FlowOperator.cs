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
        private readonly FlowExecutionService _flowExecutionService;
        private readonly FlowHandlerFactory _flowHandlerFactory;
        private readonly IFlowService _flowService;

        public FlowOperator(FlowHandlerFactory flowHandlerFactory,
            FlowExecutionService flowExecutionService,
            IFlowService flowService)
        {
            _flowHandlerFactory = flowHandlerFactory;
            _flowExecutionService = flowExecutionService;
            _flowService = flowService;
        }

        private static FlowStepContext InstantiateContext(int trigger,
            Dictionary<object, object> data,
            FlowExecution flowExecution,
            FlowExecutionStep activeFlowStep)
        {
            return new FlowStepContext
            {
                FlowExecution = flowExecution,
                FlowExecutionStep = activeFlowStep,
                FlowExecutionStepDetail = activeFlowStep.Details.Single(x => x.Transition.Triggers.FirstOrDefault()!.Code == trigger),
                Data = data
            };
        }

        public async Task<FlowExecution> Instantiate(Flow flow)
        {
            var processExecution = await _flowExecutionService.InitializeFlowExecution(flow);

            return processExecution;
        }
        
        public async Task<FlowExecution> Instantiate(string flowKey)
        {
            var process = (await _flowService.Get(key: flowKey))
                .SingleOrDefault();
            if (process is null)
            {
                throw new FlowCiaoException("Invalid flow key!");
            }
            
            var processExecution = await _flowExecutionService.InitializeFlowExecution(process);

            return processExecution;
        }

        public async Task<FlowResult> FireAsync(Guid flowInstanceId, int trigger, Dictionary<object, object> data = null)
        {
            try
            {
                var processExecution = (await _flowExecutionService.Get(id: flowInstanceId)).SingleOrDefault();

                if (processExecution is null)
                {
                    throw new FlowCiaoException("Invalid ProcessInstance id!");
                }

                if (processExecution.ActiveExecutionStep is null)
                {
                    throw new FlowExecutionException("No active steps to fire");
                }

                if (processExecution.ActiveExecutionStep.Details
                        .SingleOrDefault(x => x.Transition.Triggers.FirstOrDefault()!.Code == trigger) is null)
                {
                    throw new FlowExecutionException("Trigger is invalid!");
                }

                var processStepContext = InstantiateContext(trigger, data, processExecution, processExecution.ActiveExecutionStep);
                var handlers = _flowHandlerFactory.BuildHandlers();

                var result = handlers.Peek().Handle(processStepContext);

                return result;
            }
            catch (Exception exception)
            {
                return new FlowResult
                {
                    Status = FlowResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }

        public async Task<FlowResult> FireAsync(FlowExecution flowExecution, int trigger, Dictionary<object, object> data = null)
        {
            try
            {
                await Task.CompletedTask;
                
                if (flowExecution is null)
                {
                    throw new FlowCiaoException("Invalid ProcessInstance id!");
                }

                if (flowExecution.ActiveExecutionStep is null)
                {
                    throw new FlowExecutionException("No active steps to fire");
                }

                if (flowExecution.ActiveExecutionStep.Details
                        .SingleOrDefault(x => x.Transition.Triggers.FirstOrDefault()!.Code == trigger) is null)
                {
                    throw new FlowExecutionException("Trigger is invalid!");
                }

                var processStepContext = InstantiateContext(trigger, data, flowExecution, flowExecution.ActiveExecutionStep);
                var handlers = _flowHandlerFactory.BuildHandlers();

                var result = handlers.Peek().Handle(processStepContext);

                return result;
            }
            catch (Exception exception)
            {
                return new FlowResult
                {
                    Status = FlowResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }

        public async Task<FlowResult> Fire(string key,
            int trigger,
            Dictionary<object, object> data = null)
        {
            try
            {
                var processExecution = (await _flowExecutionService.Get()).SingleOrDefault();

                if (processExecution is null)
                {
                    var process = (await _flowService.Get(key: key))
                        .SingleOrDefault();
                    if (process is null)
                    {
                        throw new FlowCiaoException("Invalid flow key!");
                    }

                    processExecution = await _flowExecutionService.InitializeFlowExecution(process);
                }

                if (processExecution.ActiveExecutionStep is null)
                {
                    throw new FlowExecutionException("No active steps to fire");
                }

                if (processExecution.ActiveExecutionStep.Details
                        .SingleOrDefault(x => x.Transition.Triggers.FirstOrDefault()!.Code == trigger) is null)
                {
                    throw new FlowExecutionException("Trigger is invalid!");
                }

                var processStepContext = InstantiateContext(trigger, data, processExecution, processExecution.ActiveExecutionStep);
                var handlers = _flowHandlerFactory.BuildHandlers();

                var result = handlers.Peek().Handle(processStepContext);

                return result;
            }
            catch (Exception exception)
            {
                return new FlowResult
                {
                    Status = FlowResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }
        
        public async Task<State> GetFLowState(string key)
        {
            var processExecution = (await _flowExecutionService.Get()).SingleOrDefault();
            if (processExecution is null)
            {
                var process = (await _flowService.Get(key: key))
                        .SingleOrDefault();
                if (process is null)
                {
                    throw new FlowCiaoException("Invalid key!");
                }

                processExecution = await _flowExecutionService.InitializeFlowExecution(process);
            }

            return processExecution.State;
        }
    }
}
