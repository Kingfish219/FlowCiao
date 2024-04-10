using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;
using FlowCiao.Services;
using FlowCiao.Utils;

namespace FlowCiao.Builder
{
    internal class FlowBuilder : IFlowBuilder
    {
        private List<IFlowStepBuilder> StepBuilders { get; set; }
        private IFlowStepBuilder InitialStepBuilder { get; set; }
        private readonly FlowService _flowService;
        private readonly ActivityService _activityService;
        private readonly StateService _stateService;
        private readonly TransitionService _transitionService;

        public FlowBuilder(FlowService flowService, ActivityService activityService, StateService stateService,
            TransitionService transitionService)
        {
            StepBuilders = new List<IFlowStepBuilder>();
            _flowService = flowService;
            _activityService = activityService;
            _stateService = stateService;
            _transitionService = transitionService;
        }

        public IFlowBuilder Initial(Action<IFlowStepBuilder> action)
        {
            InitialStepBuilder = new FlowStepBuilder(_activityService, _stateService, _transitionService);
            action(InitialStepBuilder);
            InitialStepBuilder.AsInitialStep();
            StepBuilders.Add(InitialStepBuilder);

            return this;
        }

        public IFlowBuilder NewStep(Action<IFlowStepBuilder> action)
        {
            var builder = new FlowStepBuilder(_activityService, _stateService, _transitionService);
            action(builder);
            StepBuilders.Add(builder);

            return this;
        }

        public Flow Build(string flowKey, Action<IFlowBuilder> build)
        {
            return BuildAsync(flowKey, build).GetAwaiter().GetResult();
        }

        public async Task<Flow> BuildAsync(string flowKey, Action<IFlowBuilder> build)
        {
            try
            {
                build(this);

                var flow = await _flowService.GetByKey(key: flowKey) ?? new Flow
                {
                    Key = flowKey,
                    Name = flowKey,
                    IsActive = true
                };

                var result = await _flowService.Modify(flow);
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("Check your database connection!");
                }

                foreach (var flowStep in StepBuilders.Select(stepBuilder => stepBuilder.Build(flow.Id)))
                {
                    flow.States ??= new List<State>();
                    flow.States.Add(flowStep.For);
                    if (flowStep.Allowed.IsNullOrEmpty())
                    {
                        continue;
                    }

                    flow.Transitions ??= new List<Transition>();
                    flow.Transitions.AddRange(flowStep.Allowed);
                    
                    flow.States.AddRange(flow.Transitions.Select(t => t.To));
                    
                    flow.Triggers ??= new List<Trigger>();
                    flow.Triggers.AddRange(flow.Transitions.SelectMany(t => t.Triggers));
                }

                flow.States = flow.States.DistinctBy(s => s.Code).ToList();
                flow.Triggers = flow.Triggers.DistinctBy(s => s.Code).ToList();

                return flow;
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public Flow Build<T>() where T : IFlowPlanner, new()
        {
            var flowPlanner = Activator.CreateInstance<T>();
            return BuildAsync(flowPlanner.Key, builder => flowPlanner.Plan(builder)).GetAwaiter().GetResult();
        }

        public async Task<Flow> BuildAsync<T>() where T : IFlowPlanner, new()
        {
            var flowPlanner = Activator.CreateInstance<T>();
            return await BuildAsync(flowPlanner.Key, builder => flowPlanner.Plan(builder));
        }

        public Flow Build(IFlowPlanner flowPlanner)
        {
            return BuildAsync(flowPlanner.Key, builder => flowPlanner.Plan(builder)).GetAwaiter().GetResult();
        }

        public async Task<Flow> BuildAsync(IFlowPlanner flowPlanner)
        {
            return await BuildAsync(flowPlanner.Key, builder => flowPlanner.Plan(builder));
        }

        private void Rollback()
        {
            // ignored
        }
    }
}