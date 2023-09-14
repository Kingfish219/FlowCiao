﻿using System;
using System.Collections.Generic;
using System.Linq;
using FlowCiao.Exceptions;
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
                var smartFlow = Activator.CreateInstance<T>();
                if (smartFlow is null)
                {
                    throw new Exception();
                }

                var process = _processService.Get(key: smartFlow.FlowKey).GetAwaiter().GetResult().FirstOrDefault();
                if (process != null)
                {
                    return process;
                }

                var constructor = smartFlow.Construct<T>(this);
                if(constructor.InitialStepBuilder is null)
                {
                    throw new Exception("Your flow should have an initial state, use Initial to declare one");
                }

                process = new Process
                {
                    FlowKey = smartFlow.FlowKey
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

        public Process Build<T>(Action<IFlowBuilder> constructor) where T : IFlow, new()
        {
            throw new NotImplementedException();

            //try
            //{
            //    var smartFlow = Activator.CreateInstance<T>();
            //    constructor.Invoke(this);

            //    var process = _processService.Get(key: smartFlow.FlowKey).GetAwaiter().GetResult().FirstOrDefault();
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
            //                ProcessActivityExecutor = builder.OnEntryActivty
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
            //        throw new SmartFlowPersistencyException("Check your database connection!");
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

        }

        public IFlowStepBuilder Initial(IFlowStepBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}