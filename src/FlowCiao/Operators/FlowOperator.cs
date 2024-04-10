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
    internal class FlowOperator : IFlowOperator
    {
        private readonly FlowInstanceService _flowInstanceService;
        private readonly FlowHandlerFactory _flowHandlerFactory;
        private readonly FlowService _flowService;

        public FlowOperator(FlowHandlerFactory flowHandlerFactory,
            FlowInstanceService flowInstanceService,
            FlowService flowService)
        {
            _flowHandlerFactory = flowHandlerFactory;
            _flowInstanceService = flowInstanceService;
            _flowService = flowService;
        }

        private static FlowStepContext InstantiateContext(int trigger,
            Dictionary<object, object> data,
            FlowInstance flowInstance)
        {
            return new FlowStepContext
            {
                FlowInstance = flowInstance,
                FlowInstanceStep = flowInstance.ActiveInstanceStep,
                FlowInstanceStepDetail =
                    flowInstance.ActiveInstanceStep.Details.Single(x =>
                        x.Transition.Triggers.FirstOrDefault()!.Code == trigger),
                Data = data
            };
        }

        public async Task<FlowInstance> Ciao(Flow flow)
        {
            var flowInstance = await _flowInstanceService.InitializeFlowInstance(flow);

            return flowInstance;
        }

        public async Task<FlowInstance> Ciao(string flowKey)
        {
            var flow = await _flowService.GetByKey(key: flowKey);

            return await Ciao(flow);
        }

        public async Task<FlowResult> FireAsync(FlowInstance flowInstance, int triggerCode,
            Dictionary<object, object> data = null)
        {
            try
            {
                await Task.CompletedTask;
                ValidateFireable(flowInstance, triggerCode);

                var flowStepContext = InstantiateContext(triggerCode, data, flowInstance);
                var result = _flowHandlerFactory
                    .BuildHandlers()
                    .Peek()
                    .Handle(flowStepContext);
                
                result.Message = result.Status == FlowResultStatus.Completed
                    ? "Flow fired successfully"
                    : "Firing flow occurred error and operations would be rolled back";

                return result;
            }
            catch (Exception exception)
            {
                return new FlowResult
                {
                    Status = FlowResultStatus.Failed,
                    Message = $"Firing flow occurred error and operations would be rolled back: {exception.Message}"
                };
            }
        }

        public async Task<FlowResult> FireAsync(Guid flowInstanceId, int triggerCode,
            Dictionary<object, object> data = null)
        {
            var flowInstance = await _flowInstanceService.GetById(id: flowInstanceId);

            return await FireAsync(flowInstance, triggerCode, data);
        }

        public async Task<FlowResult> CiaoAndFireAsync(string flowKey,
            int trigger,
            Dictionary<object, object> data = null)
        {
            var flowInstance = await Ciao(flowKey);

            return await FireAsync(flowInstance, trigger, data);
        }

        private static void ValidateFireable(FlowInstance flowInstance, int triggerCode)
        {
            if (flowInstance is null)
            {
                throw new FlowCiaoException("Invalid ProcessInstance id!");
            }

            if (flowInstance.ActiveInstanceStep is null)
            {
                throw new FlowCiaoExecutionException("No active steps to fire");
            }

            if (flowInstance.ActiveInstanceStep.Details
                    .SingleOrDefault(x => x.Transition.Triggers.FirstOrDefault()!.Code == triggerCode) is null)
            {
                throw new FlowCiaoExecutionException("Trigger is invalid!");
            }
        }
    }
}