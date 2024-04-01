using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Services
{
    public class FlowExecutionService
    {
        private readonly IFlowExecutionRepository _flowExecutionRepository;
        private readonly FlowSettings _flowSettings;

        public FlowExecutionService(IFlowExecutionRepository flowExecutionRepository
            , FlowSettings flowSettings
            )
        {
            _flowSettings = flowSettings;
            _flowExecutionRepository = flowExecutionRepository;
        }

        public async Task<List<FlowExecution>> Get(Guid flowId = default)
        {
            return await _flowExecutionRepository.Get(flowId);
        }

        public async Task<FlowExecution> GetById(Guid id)
        {
            return await _flowExecutionRepository.GetById(id);
        }

        private FlowExecutionStep GenerateFlowStep(Flow flow, State state)
        {
            var flowStep = new FlowExecutionStep
            {
                CreatedOn = DateTime.Now,
                Details = flow.Transitions.Where(x => x.From.Id.Equals(state.Id))
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

            return flowStep;
        }

        public async Task<FlowExecution> InitializeFlowExecution(Flow flow)
        {
            var flowExecution = new FlowExecution
            {
                Id = Guid.NewGuid(),
                Flow = flow,
                CreatedOn = DateTime.Now,
                ExecutionState = FlowExecution.FlowExecutionState.Initial,
                ExecutionSteps = new List<FlowExecutionStep>
                {
                    GenerateFlowStep(flow,
                            flow.Transitions.First(x => x.From.IsInitial).From)
                }
            };

            if (_flowSettings.PersistFlow)
            {
                await Modify(flowExecution);
            }

            return flowExecution;
        }

        public async Task<FlowExecution> FinalizeFlowExecutionStep(Flow flow)
        {
            var flowExecution = new FlowExecution
            {
                Id = Guid.NewGuid(),
                Flow = flow,
                CreatedOn = DateTime.Now,
                ExecutionState = FlowExecution.FlowExecutionState.Initial,
                ExecutionSteps = new List<FlowExecutionStep>
                {
                    GenerateFlowStep(flow,
                        flow.Transitions.First(x => x.From.IsInitial).From)
                }
            };

            if (_flowSettings.PersistFlow)
            {
                await Modify(flowExecution);
            }

            return flowExecution;
        }

        public async Task<FlowExecution> Finalize(FlowExecution flowExecution)
        {
            flowExecution.ExecutionSteps.Add(GenerateFlowStep(flowExecution.Flow,
                        flowExecution.Flow.Transitions.First(x => x.From.IsInitial).From));

            if (_flowSettings.PersistFlow)
            {
                await Modify(flowExecution);
            }

            return flowExecution;
        }

        public async Task<Guid> Modify(FlowExecution entity)
        {
            var flowId = await _flowExecutionRepository.Modify(entity);
            if (flowId == default)
            {
                throw new FlowCiaoPersistencyException();
            }

            return flowId;
        }
    }
}
