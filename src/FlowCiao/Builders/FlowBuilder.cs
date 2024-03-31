using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Builders
{
    internal class FlowBuilder : IFlowBuilder
    {
        public List<IFlowStepBuilder> StepBuilders { get; set; }
        public IFlowStepBuilder InitialStepBuilder { get; set; }
        private readonly IFlowService _flowService;
        private readonly IActivityRepository _activityRepository;

        public FlowBuilder(IFlowService flowService, IActivityRepository activityRepository)
        {
            StepBuilders = new List<IFlowStepBuilder>();
            _flowService = flowService;
            _activityRepository = activityRepository;
        }

        public IFlowBuilder Initial(Action<IFlowStepBuilder> action)
        {
            var builder = new FlowStepBuilder(this, _activityRepository);
            InitialStepBuilder = builder;
            action(InitialStepBuilder);
            InitialStepBuilder.InitialState.IsInitial = true;
            StepBuilders.Add(builder);

            return this;
        }
        
        public IFlowBuilder NewStep(Action<IFlowStepBuilder> action)
        {
            var builder = new FlowStepBuilder(this, _activityRepository);
            action(builder);
            StepBuilders.Add(builder);

            return this;
        }
        
        public Flow Build<T>() where T : IFlowPlanner, new()
        {
            try
            {
                var flowPlanner = Activator.CreateInstance<T>();
                if (flowPlanner is null)
                {
                    throw new FlowCiaoException();
                }

                var flow = _flowService.GetByKey(key: flowPlanner.Key).GetAwaiter().GetResult();
                if (flow != null)
                {
                    return flow;
                }

                flowPlanner.Plan<T>(this);
                
                flow = new Flow
                {
                    Key = flowPlanner.Key,
                    InitialState = InitialStepBuilder.InitialState
                };

                foreach (var stepBuilder in StepBuilders)
                {
                    foreach (var allowedTransitionBuilder in stepBuilder.AllowedTransitionsBuilders)
                    {
                        var transition = new Transition();
                        allowedTransitionBuilder(transition);
                        flow.Transitions.Add(transition);
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

                InitialStepBuilder = new FlowStepBuilder(this, _activityRepository);
                InitialStepBuilder.Build(states, jsonFlow.Initial);
            
                flow = new Flow
                {
                    Key = jsonFlow.Key
                };
                flow.Transitions ??= new List<Transition>();
                InitialStepBuilder.InitialState.IsInitial = true;
                flow.InitialState = InitialStepBuilder.InitialState;

                InitialStepBuilder.AllowedTransitionsBuilders.ForEach(allowedTransition =>
                {
                    var transition = new Transition();
                    allowedTransition(transition);
                    flow.Transitions.Add(transition);
                });
                
                if (jsonFlow.Steps is { Count: > 0 })
                {
                    jsonFlow.Steps.ForEach(jsonStep =>
                    {
                        var stepBuilder = new FlowStepBuilder(this, _activityRepository);
                        stepBuilder.Build(states, jsonStep);
                        StepBuilders.Add(stepBuilder);
                    });

                    foreach (var stepBuilder in StepBuilders)
                    {
                        foreach (var allowedTransition in stepBuilder.AllowedTransitionsBuilders)
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

            //try
            //{
            //    var flow = Activator.CreateInstance<T>();
            //    constructor.Invoke(this);

            //    var flow = _flowService.Get(key: flow.Key).GetAwaiter().GetResult().FirstOrDefault();
            //    if (flow != null)
            //    {
            //        return flow;
            //    }

            //    foreach (var builder in StepBuilders)
            //    {
            //        builder.InitialState.Activities = new List<Activity>
            //        {
            //            new Activity
            //            {
            //                Actor = builder.OnEntryActivity
            //            }
            //        };

            //        foreach (var allowedTransition in builder.AllowedTransitions)
            //        {
            //            flow.Transitions.Add(new Transition
            //            {
            //                From = builder.InitialState,
            //                To = allowedTransition.Item1,
            //                Activities = new List<Activity>
            //                {
            //                    new Activity
            //                    {
            //                        Actor = builder.OnExitActivity
            //                    }
            //                },
            //                Triggers = allowedTransition.Item2
            //            });
            //        }
            //    }

            //    var result = _flowService.Modify(flow).GetAwaiter().GetResult();
            //    if (result == default)
            //    {
            //        throw new FlowCiaoPersistancyException("Check your database connection!");
            //    }

            //    return flow;
            //}
            //catch (Exception)
            //{
            //    Rollback();

            //    throw;
            //}
        }

        private void Rollback()
        {
            // ignored
        }
    }
}
