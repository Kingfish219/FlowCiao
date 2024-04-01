using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;
using FlowCiao.Services;
using FlowCiao.Utils;
using Newtonsoft.Json;

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
            InitialStepBuilder.IsInitial();
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

                foreach (var flowStep in StepBuilders.Select(stepBuilder => stepBuilder.Build()))
                {
                    flow.States.Add(flowStep.For);
                    if (flowStep.Allowed.IsNullOrEmpty())
                    {
                        continue;
                    }
                    
                    flow.Transitions.AddRange(flowStep.Allowed);
                    flow.States.AddRange(flow.Transitions.Select(t => t.To));
                    flow.Triggers.AddRange(flow.Transitions.SelectMany(t => t.Triggers));
                }
                
                flow.States = flow.States.DistinctBy(s => s.Code).ToList();
                flow.Triggers = flow.Triggers.DistinctBy(s => s.Code).ToList();

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

        public async Task<Flow> BuildFromJsonAsync(string json)
        {
            try
            {
                var jsonFlow = JsonConvert.DeserializeObject<JsonFlow>(json);
                var flow = await _flowService.GetByKey(key: jsonFlow.Key);
                // if (flow != null)
                // {
                //     return flow;
                // }

                var states = jsonFlow.States
                    .Select(jsonState => new State(jsonState.Code, jsonState.Name))
                    .ToList();

                InitialStepBuilder = new FlowStepBuilder(_activityService, _stateService, _transitionService);
                //
                // var fromState = states.Single(state => state.Code == jsonStep.FromStateCode);
                // var allowedList = states
                //     .Where(state => jsonStep.Allows.Exists(allowed => allowed.AllowedStateCode == state.Code))
                //     .Select(state => new
                //     {
                //         AllowedState = state,
                //         AllowedTrigger = jsonStep.Allows.Single(x => x.AllowedStateCode == state.Code).TriggerCode
                //     })
                //     .ToList();
                //
                // flow = new Flow
                // {
                //     Key = jsonFlow.Key
                // };
                //
                // if (jsonFlow.Steps is { Count: > 0 })
                // {
                //     jsonFlow.Steps.ForEach(jsonStep =>
                //     {
                //         var stepBuilder = new FlowStepBuilder(_activityService, _stateService, _transitionService);
                //         stepBuilder.BuildAsync(states, jsonStep);
                //         StepBuilders.Add(stepBuilder);
                //     });
                //
                //     foreach (var stepBuilder in StepBuilders)
                //     {
                //         foreach (var allowedTransition in stepBuilder.AllowedBuilders)
                //         {
                //             var transition = new Transition();
                //             allowedTransition(transition);
                //             flow.Transitions.Add(transition);
                //         }
                //     }
                // }
                //
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