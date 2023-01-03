using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFlow.Core
{
    public class WorkflowBuilder
    {
        private List<ProcessStepBuilder> ProcessStepBuilders { get; set; }
        public string WorkflowKey { get; set; }
        private IProcessRepository _processRepository;

        public WorkflowBuilder(string workflowKey)
        {
            WorkflowKey = workflowKey;
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

        public WorkflowBuilder UseSqlServer(string connectionString)
        {
            _processRepository = new DefaultProcessRepository(connectionString);

            return this;
        }

        public Process Build()
        {
            try
            {
                var process = new Process();
                foreach (var builder in ProcessStepBuilders)
                {
                    foreach (var allowedTransition in builder.AllowedTransitions)
                    {
                        process.Transitions.Add(new Transition
                        {
                            From = builder.InitialState,
                            To = allowedTransition.Item1,
                            //Activities = new List<IProcessActivity>
                            //{
                            //    ProcessStepBuilders.Where(x=>x.AllowedTransitions == allowedTransition.Item1.Code).Select(y=> y.)
                            //}
                        });
                    }
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
