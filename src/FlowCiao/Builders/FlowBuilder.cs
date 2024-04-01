using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;
using FlowCiao.Services;

namespace FlowCiao.Builders
{
    internal class FlowBuilder : IFlowBuilder
    {
        public List<IFlowStepBuilder> StepBuilders { get; set; }
        public IFlowStepBuilder InitialStepBuilder { get; set; }
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
            InitialStepBuilder.InitialState.IsInitial = true;
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

        public Flow Build<T>() where T : IFlowPlanner, new()
        {
            try
            {
                var flowPlanner = Activator.CreateInstance<T>();
                var flow = _flowService.GetByKey(key: flowPlanner.Key).GetAwaiter().GetResult();
                if (flow != null)
                {
                    return flow;
                }

                flowPlanner.Plan<T>(this);

                flow = new Flow
                {
                    Key = flowPlanner.Key
                };

                foreach (var stepBuilder in StepBuilders)
                {
                    foreach (var allowedTransitionBuilder in stepBuilder.AllowedBuilders)
                    {
                        var transition = new Transition();
                        allowedTransitionBuilder(transition);
                        flow.Transitions.Add(transition);
                    }
                }

                flow.States.AddRange(flow.Transitions.Select(t => t.From));
                flow.States.AddRange(flow.Transitions.Select(t => t.To));
                flow.States = flow.States.DistinctBy(s => s.Code).ToList();

                flow.Triggers.AddRange(flow.Transitions.SelectMany(t => t.Triggers));
                flow.Triggers.AddRange(flow.Transitions.SelectMany(t => t.Triggers));
                flow.Triggers = flow.Triggers.DistinctBy(s => s.Code).ToList();

                foreach (var transition in flow.Transitions)
                {
                    for (var i = 0; i < transition.Activities.Count; i++)
                    {
                        var activity = transition.Activities[i];
                        var existed = _activityService.GetByKey(activity.Id, activity.ActorName).GetAwaiter()
                            .GetResult();
                        if (existed != null)
                        {
                            transition.Activities[i] = existed;
                            continue;
                        }

                        var inserted = _activityService.Modify(activity).GetAwaiter().GetResult();
                        if (inserted == default)
                        {
                            throw new FlowCiaoPersistencyException("Could not modify activity");
                        }
                    }
                }

                foreach (var state in flow.States)
                {
                    for (var i = 0; i < state.Activities.Count; i++)
                    {
                        var activity = state.Activities[i];
                        var existed = _activityService.GetByKey(activity.Id, activity.ActorName).GetAwaiter()
                            .GetResult();
                        if (existed != null)
                        {
                            state.Activities[i] = existed;
                            continue;
                        }

                        var inserted = _activityService.Modify(activity).GetAwaiter().GetResult();
                        if (inserted == default)
                        {
                            throw new FlowCiaoPersistencyException("Could not modify activity");
                        }
                    }
                }

                var result = _flowService.Modify(flow).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("Check your database connection!");
                }

                return flow;
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public async Task<Flow> Build(JsonFlow jsonFlow)
        {
            try
            {
                var flow = await _flowService.GetByKey(key: jsonFlow.Key);
                if (flow != null)
                {
                    return flow;
                }

                var states = jsonFlow.States
                    .Select(jsonState => new State(jsonState.Code, jsonState.Name))
                    .ToList();

                InitialStepBuilder = new FlowStepBuilder(_activityService, _stateService, _transitionService);
                InitialStepBuilder.Build(states, jsonFlow.Initial);

                flow = new Flow
                {
                    Key = jsonFlow.Key
                };
                flow.Transitions ??= new List<Transition>();
                InitialStepBuilder.InitialState.IsInitial = true;

                InitialStepBuilder.AllowedBuilders.ForEach(allowedTransition =>
                {
                    var transition = new Transition();
                    allowedTransition(transition);
                    flow.Transitions.Add(transition);
                });

                if (jsonFlow.Steps is { Count: > 0 })
                {
                    jsonFlow.Steps.ForEach(jsonStep =>
                    {
                        var stepBuilder = new FlowStepBuilder(_activityService, _stateService, _transitionService);
                        stepBuilder.Build(states, jsonStep);
                        StepBuilders.Add(stepBuilder);
                    });

                    foreach (var stepBuilder in StepBuilders)
                    {
                        foreach (var allowedTransition in stepBuilder.AllowedBuilders)
                        {
                            var transition = new Transition();
                            allowedTransition(transition);
                            flow.Transitions.Add(transition);
                        }
                    }
                }

                var result = await _flowService.Modify(flow);
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("Check your database connection!");
                }

                return flow;
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public Flow Build<T>(Action<IFlowBuilder> constructor) where T : IFlowPlanner, new()
        {
            throw new NotImplementedException();
        }

        private void Rollback()
        {
            // ignored
        }
    }
}