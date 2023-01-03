using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core
{
    public class WorkflowBuilder
    {
        private List<ProcessStepBuilder> ProcessStepBuilders { get; set; }
        private IProcessRepository _processRepository;

        public WorkflowBuilder()
        {
            ProcessStepBuilders = new List<ProcessStepBuilder>();
        }

        public ProcessStepBuilder NewStep()
        {
            var builder = new ProcessStepBuilder();
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

        public Guid Build()
        {
            try
            {
                //var process = _processRepository.Create();

                foreach (var processStepBuilder in ProcessStepBuilders)
                {
                    processStepBuilder.Build();
                }

                return Guid.NewGuid();
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
