using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Services
{
    public class FlowService : IFlowService
    {
        private readonly IFlowRepository _flowRepository;
        private readonly TransitionService _transitionService;
        private readonly StateService _stateService;

        public FlowService(IFlowRepository flowRepository
            , TransitionService transitionService
            , StateService stateService
            )
        {
            _flowRepository = flowRepository;
            _transitionService = transitionService;
            _stateService = stateService;
        }

        public async Task<Guid> Modify(Flow flow)
        {
            var flowId = await _flowRepository.Modify(flow);
            if (flowId == default)
            {
                throw new FlowCiaoPersistencyException();
            }

            flow.Transitions?.ForEach(transition =>
            {
                transition.FlowId = flowId;
                transition.From.Id = _stateService.Modify(transition.From).GetAwaiter().GetResult();
                transition.To.Id = _stateService.Modify(transition.To).GetAwaiter().GetResult();
                transition.Id = _transitionService.Modify(transition).GetAwaiter().GetResult();
            });

            return flowId;
        }

        public async Task<List<Flow>> Get(Guid flowId = default, string key = default)
        {
            return await _flowRepository.Get(flowId, key);
        }

        public FlowExecutionStep GenerateFlowStep(Flow flow, State state)
        {
            var flowExecutionStep = new FlowExecutionStep
            {
                CreatedOn = DateTime.Now,
                Details = flow.Transitions.Where(x => x.From.Code.Equals(state.Code))
                    .Select(transition =>
                    {
                        return new FlowExecutionStepDetail
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            Transition = transition
                        };
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
