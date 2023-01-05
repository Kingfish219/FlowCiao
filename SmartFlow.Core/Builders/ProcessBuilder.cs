using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Models;
using SmartFlow.Core.Services;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public class ProcessBuilder
    {
        private List<ProcessStepBuilder> ProcessStepBuilders { get; set; }
        public string ProcessKey { get; set; }
        private ProcessService _processService;

        public ProcessBuilder(string processKey)
        {
            ProcessKey = processKey;
            ProcessStepBuilders = new List<ProcessStepBuilder>();
        }

        public ProcessStepBuilder NewStep()
        {
            var builder = new ProcessStepBuilder(this);
            ProcessStepBuilders.Add(builder);

            return builder;
        }

        public ProcessStepBuilder NewStep(ProcessStepBuilder builder)
        {
            ProcessStepBuilders.Add(builder);

            return builder;
        }

        public Process Build()
        {
            try
            {
                var process = _processService.GetProcess(key: ProcessKey).GetAwaiter().GetResult();
                if (process != null)
                {
                    return process;
                }

                process = new Process();
                foreach (var builder in ProcessStepBuilders)
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
    }
}
