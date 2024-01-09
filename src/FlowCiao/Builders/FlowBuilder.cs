using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Flow;
using FlowCiao.Services;

namespace FlowCiao.Builders
{
    internal class FlowBuilder : IFlowBuilder
    {
        public List<IFlowStepBuilder> StepBuilders { get; set; }
        public IFlowStepBuilder InitialStepBuilder { get; set; }
        private readonly IProcessService _processService;

        public FlowBuilder(IProcessService processService)
        {
            StepBuilders = new List<IFlowStepBuilder>();
            _processService = processService;
        }

        public IFlowBuilder Initial(Action<IFlowStepBuilder> action)
        {
            var builder = new FlowStepBuilder(this);
            InitialStepBuilder = builder;
            action(InitialStepBuilder);
            StepBuilders.Add(builder);

            return this;
        }
        
        public IFlowBuilder NewStep(Action<IFlowStepBuilder> action)
        {
            var builder = new FlowStepBuilder(this);
            action(builder);
            StepBuilders.Add(builder);

            return this;
        }
        
        public Process Build<T>() where T : IFlow, new()
        {
            try
            {
                var flow = Activator.CreateInstance<T>();
                if (flow is null)
                {
                    throw new FlowCiaoException();
                }

                var process = _processService.Get(key: flow.Key).GetAwaiter().GetResult().FirstOrDefault();
                if (process != null)
                {
                    return process;
                }

                var constructor = flow.Construct<T>(this);
                if(constructor.InitialStepBuilder is null)
                {
                    throw new FlowCiaoException("Your flow should have an initial state, use Initial to declare one");
                }

                process = new Process
                {
                    Key = flow.Key
                };
                process.Transitions ??= new List<Transition>();
                constructor.InitialStepBuilder.InitialState.IsInitial = true;
                process.InitialState = constructor.InitialStepBuilder.InitialState;

                foreach (var builder in constructor.StepBuilders)
                {
                    foreach (var allowedTransition in builder.AllowedTransitionsBuilders)
                    {
                        var transition = new Transition();
                        allowedTransition(transition);
                        process.Transitions.Add(transition);
                    }
                }

                var result = _processService.Modify(process).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("Check your database connection!");
                }

                return process;
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public async Task<Process> Build(JsonFlow jsonFlow)
        {
            try
            {
                var process = (await _processService.Get(key: jsonFlow.Key)).FirstOrDefault();
                if (process != null)
                {
                    return process;
                }

                var states = jsonFlow.States.Select(jsonState => new State(jsonState.Code, jsonState.Name)).ToList();
                
                var builder = new FlowStepBuilder(this);
                InitialStepBuilder = builder;
                InitialStepBuilder.Build(states, jsonFlow.Initial);
                StepBuilders.Add(builder);
            
                jsonFlow.Steps.ForEach(jsonStep =>
                {
                    var stepBuilder = new FlowStepBuilder(this);
                    stepBuilder.Build(states, jsonStep);
                    StepBuilders.Add(stepBuilder);
                });
                
                process = new Process
                {
                    Key = jsonFlow.Key
                };
                process.Transitions ??= new List<Transition>();
                InitialStepBuilder.InitialState.IsInitial = true;
                process.InitialState = InitialStepBuilder.InitialState;

                foreach (var stepBuilder in StepBuilders)
                {
                    foreach (var allowedTransition in stepBuilder.AllowedTransitionsBuilders)
                    {
                        var transition = new Transition();
                        allowedTransition(transition);
                        process.Transitions.Add(transition);
                    }
                }

                var result = await _processService.Modify(process);
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("Check your database connection!");
                }

                return process;
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public Process Build<T>(Action<IFlowBuilder> constructor) where T : IFlow, new()
        {
            throw new NotImplementedException();

            //try
            //{
            //    var flow = Activator.CreateInstance<T>();
            //    constructor.Invoke(this);

            //    var process = _processService.Get(key: flow.Key).GetAwaiter().GetResult().FirstOrDefault();
            //    if (process != null)
            //    {
            //        return process;
            //    }

            //    foreach (var builder in StepBuilders)
            //    {
            //        builder.InitialState.Activities = new List<Activity>
            //        {
            //            new Activity
            //            {
            //                ProcessActivityExecutor = builder.OnEntryActivity
            //            }
            //        };

            //        foreach (var allowedTransition in builder.AllowedTransitions)
            //        {
            //            process.Transitions.Add(new Transition
            //            {
            //                From = builder.InitialState,
            //                To = allowedTransition.Item1,
            //                Activities = new List<Activity>
            //                {
            //                    new Activity
            //                    {
            //                        ProcessActivityExecutor = builder.OnExitActivity
            //                    }
            //                },
            //                Actions = allowedTransition.Item2
            //            });
            //        }
            //    }

            //    var result = _processService.Modify(process).GetAwaiter().GetResult();
            //    if (result == default)
            //    {
            //        throw new FlowCiaoPersistancyException("Check your database connection!");
            //    }

            //    return process;
            //}
            //catch (Exception)
            //{
            //    Rollback();

            //    throw;
            //}
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public IFlowStepBuilder Initial(IFlowStepBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
