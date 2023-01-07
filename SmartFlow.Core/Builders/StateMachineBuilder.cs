using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Services;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public class StateMachineBuilder : IStateMachineBuilder
    {
        public List<IStateMachineStepBuilder> StateMachineStepBuilders { get; set; }
        private readonly ProcessService _processService;

        public StateMachineBuilder(ProcessService processService)
        {
            StateMachineStepBuilders = new List<IStateMachineStepBuilder>();
            _processService = processService;
        }

        public IStateMachineStepBuilder NewStep()
        {
            var builder = new StateMachineStepBuilder(this);
            StateMachineStepBuilders.Add(builder);

            return builder;
        }

        public IStateMachineStepBuilder NewStep(IStateMachineStepBuilder builder)
        {
            StateMachineStepBuilders.Add(builder);

            return builder;
        }

        public IStateMachine Build<T>() where T : IStateMachine, new()
        {
            try
            {
                var stateMachine = (IStateMachine)Activator.CreateInstance(typeof(T));
                stateMachine.Construct<T>(this);

                var process = _processService.GetProcess(key: "").GetAwaiter().GetResult();
                if (process != null)
                {
                    return process;
                }

                process = new Process();
                foreach (var builder in StateMachineStepBuilders)
                {
                    builder.InitialState.Activities = new List<Activity>
                    {
                        new Activity
                        {
                            ProcessActivityExecutor = builder.OnEntryActivty
                        }
                    };

                    foreach (var allowedTransition in builder.AllowedTransitions)
                    {
                        process.Transitions.Add(new Transition
                        {
                            From = builder.InitialState,
                            To = allowedTransition.Item1,
                            Activities = new List<Activity>
                            {
                                new Activity
                                {
                                    ProcessActivityExecutor = builder.OnExitActivity
                                }
                            },
                            Actions = allowedTransition.Item2
                        });
                    }
                }

                var result = _processService.Create(process).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("Check your database connection!");
                }

                return process;
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public IStateMachine Build<T>(Action<IStateMachineBuilder> constructor) where T : IStateMachine, new()
        {
            try
            {
                var stateMachine = (IStateMachine)Activator.CreateInstance(typeof(T));
                constructor.Invoke(this);
                 
                var process = _processService.GetProcess(key: "").GetAwaiter().GetResult();
                if (process != null)
                {
                    return process;
                }

                process = new Process();
                foreach (var builder in StateMachineStepBuilders)
                {
                    builder.InitialState.Activities = new List<Activity>
                    {
                        new Activity
                        {
                            ProcessActivityExecutor = builder.OnEntryActivty
                        }
                    };

                    foreach (var allowedTransition in builder.AllowedTransitions)
                    {
                        process.Transitions.Add(new Transition
                        {
                            From = builder.InitialState,
                            To = allowedTransition.Item1,
                            Activities = new List<Activity>
                            {
                                new Activity
                                {
                                    ProcessActivityExecutor = builder.OnExitActivity
                                }
                            },
                            Actions = allowedTransition.Item2
                        });
                    }
                }

                var result = _processService.Create(process).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("Check your database connection!");
                }

                return process;
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public void Rollback()
        {

        }

        public ISmartFlow Build<T>(Action<ISmartFlowBuilder> action) where T : ISmartFlow, new()
        {
            throw new NotImplementedException();
        }
    }
}
