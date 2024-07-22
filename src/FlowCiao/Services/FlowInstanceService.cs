using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Services
{
    public class FlowInstanceService
    {
        private readonly IFlowInstanceRepository _flowInstanceRepository;
        private readonly FlowSettings _flowSettings;

        public FlowInstanceService(IFlowInstanceRepository flowInstanceRepository, FlowSettings flowSettings
            )
        {
            _flowSettings = flowSettings;
            _flowInstanceRepository = flowInstanceRepository;
        }

        public async Task<List<FlowInstance>> Get(Guid flowId = default)
        {
            return await _flowInstanceRepository.Get(flowId);
        }

        public async Task<FlowInstance> GetById(Guid id)
        {
            return await _flowInstanceRepository.GetById(id);
        }

        private FlowInstanceStep GenerateFlowStep(Flow flow, State state)
        {
            var flowStep = new FlowInstanceStep
            {
                CreatedOn = DateTime.Now,
                Details = flow.Transitions.Where(x => x.FromId.Equals(state.Id))
                    .Select(transition => new FlowInstanceStepDetail
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        Transition = transition
                    }).ToList()
            };

            return flowStep;
        }

        internal async Task<FlowInstance> InitializeFlowInstance(Flow flow)
        {
            var flowExecution = new FlowInstance
            {
                Id = Guid.NewGuid(),
                Flow = flow,
                CreatedOn = DateTime.Now,
                ExecutionState = FlowInstance.FlowExecutionState.Initial,
                InstanceSteps = new List<FlowInstanceStep>
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

        internal async Task<FlowInstance> Finalize(FlowInstance flowInstance, FlowStepContext flowStepContext)
        {
            flowInstance.InstanceSteps.Add(GenerateFlowStep(flowInstance.Flow,
                        flowStepContext.FlowInstanceStep.Details.First(x=>x.IsCompleted).Transition.To));

            if (_flowSettings.PersistFlow)
            {
                await Modify(flowInstance);
            }

            return flowInstance;
        }

        public async Task<Guid> Modify(FlowInstance entity)
        {
            return await _flowInstanceRepository.Modify(entity);
        }
    }
}
