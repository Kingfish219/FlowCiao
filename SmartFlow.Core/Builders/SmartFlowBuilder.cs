﻿using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFlow.Core.Builders
{
    public class SmartFlowBuilder : ISmartFlowBuilder
    {
        public List<ISmartFlowStepBuilder> StepBuilders { get; set; }
        public ISmartFlowStepBuilder InitialStepBuilder { get; set; }
        private readonly SmartFlowService _processService;

        public SmartFlowBuilder(SmartFlowService processService)
        {
            StepBuilders = new List<ISmartFlowStepBuilder>();
            _processService = processService;
        }

        public ISmartFlowStepBuilder Initial()
        {
            InitialStepBuilder = new SmartFlowStepBuilder(this);

            return InitialStepBuilder;
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
                var smartFlow = Activator.CreateInstance<T>();
                if (smartFlow is null)
                {
                    throw new Exception();
                }

                smartFlow.Construct<T>(this);

                var process = _processService.Get<T>(key: smartFlow.FlowKey).GetAwaiter().GetResult().FirstOrDefault();
                if (process != null)
                {
                    return process;
                }

                smartFlow.Transitions ??= new List<Transition>();

                InitialStepBuilder.InitialState.IsInitial = true;
                foreach (var allowedTransition in InitialStepBuilder.AllowedTransitions)
                {
                    var transition = new Transition
                    {
                        From = InitialStepBuilder.InitialState,
                        To = allowedTransition.Item1,
                        Activities = InitialStepBuilder.OnExitActivity != null ? new List<Activity>
                            {
                                new()
                                {
                                    ProcessActivityExecutor = InitialStepBuilder.OnExitActivity
                                }
                            } : new List<Activity>(),
                        Actions = allowedTransition.Item2
                    };

                    smartFlow.Transitions.Add(transition);
                }

                foreach (var builder in StepBuilders)
                {
                    builder.InitialState.IsInitial = false;

                    foreach (var allowedTransition in builder.AllowedTransitions)
                    {
                        var transition = new Transition
                        {
                            From = builder.InitialState,
                            To = allowedTransition.Item1,
                            Activities = builder.OnExitActivity != null ? new List<Activity>
                            {
                                new()
                                {
                                    ProcessActivityExecutor = builder.OnExitActivity
                                }
                            } : new List<Activity>(),
                            Actions = allowedTransition.Item2
                        };

                        smartFlow.Transitions.Add(transition);
                    }
                }

                var result = _processService.Modify(smartFlow).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("Check your database connection!");
                }

                return smartFlow;
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
                var smartFlow = Activator.CreateInstance<T>();
                constructor.Invoke(this);

                var process = _processService.Get<T>(key: smartFlow.FlowKey).GetAwaiter().GetResult().FirstOrDefault();
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

                var result = _processService.Modify(smartFlow).GetAwaiter().GetResult();
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