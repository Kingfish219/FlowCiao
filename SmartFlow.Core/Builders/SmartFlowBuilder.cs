using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Services;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public class SmartFlowBuilder : ISmartFlowBuilder
    {
        public List<ISmartFlowStepBuilder> StepBuilders { get; set; }
        private readonly SmartFlowService _processService;

        public SmartFlowBuilder(SmartFlowService processService)
        {
            StepBuilders = new List<ISmartFlowStepBuilder>();
            _processService = processService;
        }

        public ISmartFlowStepBuilder NewStep()
        {
            var builder = new SmartFlowStepBuilder(this);
            StepBuilders.Add(builder);

            return builder;
        }

        public ISmartFlowStepBuilder NewStep(ISmartFlowStepBuilder builder)
        {
            StepBuilders.Add(builder);

            return builder;
        }

        public ISmartFlow Build<T>() where T : ISmartFlow, new()
        {
            try
            {
                var stateMachine = Activator.CreateInstance<T>();
                if (stateMachine is null)
                {
                    throw new Exception();
                }

                stateMachine.Construct<T>(this);

                var process = _processService.GetProcess<T>(key: stateMachine.Key).GetAwaiter().GetResult();
                if (process != null)
                {
                    return process;
                }

                process = new T();
                foreach (var builder in StepBuilders)
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

                var result = _processService.Create<T>(process).GetAwaiter().GetResult();
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

        public ISmartFlow Build<T>(Action<ISmartFlowBuilder> constructor) where T : ISmartFlow, new()
        {
            try
            {
                var stateMachine = Activator.CreateInstance<T>();
                constructor.Invoke(this);

                var process = _processService.GetProcess<T>(key: stateMachine.Key).GetAwaiter().GetResult();
                if (process != null)
                {
                    return process;
                }

                foreach (var builder in StepBuilders)
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

                var result = _processService.Create<T>(process).GetAwaiter().GetResult();
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
    }
}
