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

        public async Task<Guid> Modify(Flow flow)
        {
            await _flowRepository.Modify(flow);

            return flow.Id;
        }

        public async Task<List<Flow>> Get()
        {
            return await _flowRepository.Get();
        }

        public async Task<Flow> GetByKey(Guid flowId = default, string key = default)
        {
            return await _flowRepository.GetByKey(flowId, key);
        }

        public FlowExecutionStep GenerateFlowStep(Flow flow, State state)
        {
            var flowExecutionStep = new FlowExecutionStep
            {
                CreatedOn = DateTime.Now,
                Details = flow.Transitions.Where(x => x.From.Code.Equals(state.Code))
                    .Select(transition => new FlowExecutionStepDetail
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        Transition = transition
                    }).ToList()
            };

            return flowExecutionStep;
        }

        public async Task<FlowExecution> Finalize(FlowExecution flowExecution)
        {
            await Task.CompletedTask;

            flowExecution.ExecutionSteps.Add(GenerateFlowStep(flowExecution.Flow, flowExecution.State));

            return flowExecution;
        }
    }
}