using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Services
{
    internal class FlowService : IFlowService
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

        public async Task<FuncResult> Deactivate(Flow flow)
        {
            flow.IsActive = false;
            await Modify(flow);

            return new FuncResult(true);
        }
    }
}