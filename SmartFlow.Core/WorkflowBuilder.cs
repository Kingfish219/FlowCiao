using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core
{
    public class WorkflowBuilder
    {
        private List<ProcessStepBuilder> ProcessStepBuilders { get; set; }
        private readonly IProcessRepository _processRepository;

        public WorkflowBuilder(string connectionString)
        {
            ProcessStepBuilders = new List<ProcessStepBuilder>();
            _processRepository = new DefaultProcessRepository(connectionString);
        }

        public ProcessStepBuilder From(State state)
        {
            var processStepBuilder = new ProcessStepBuilder(state);

            return processStepBuilder;
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
