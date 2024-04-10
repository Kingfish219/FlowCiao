using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Services
{
    public class FlowService
    {
        private readonly IFlowRepository _flowRepository;

        public FlowService(IFlowRepository flowRepository)
        {
            _flowRepository = flowRepository;
        }

        public async Task<List<Flow>> Get()
        {
            return await _flowRepository.Get();
        }

        public async Task<Flow> GetByKey(Guid flowId = default, string key = default)
        {
            return await _flowRepository.GetByKey(flowId, key);
        }

        public async Task<Guid> Modify(Flow flow)
        {
            return await _flowRepository.Modify(flow);
        }

        public FlowInstanceStep GenerateFlowStep(Flow flow, State state)
        {
            var flowExecutionStep = new FlowInstanceStep
            {
                CreatedOn = DateTime.Now,
                Details = flow.Transitions.Where(x => x.From.Code.Equals(state.Code))
                    .Select(transition => new FlowInstanceStepDetail
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        Transition = transition
                    }).ToList()
            };

            return flowExecutionStep;
        }

        public async Task<FlowInstance> Finalize(FlowInstance flowInstance)
        {
            await Task.CompletedTask;

            flowInstance.InstanceSteps.Add(GenerateFlowStep(flowInstance.Flow, flowInstance.State));

            return flowInstance;
        }
    }
}