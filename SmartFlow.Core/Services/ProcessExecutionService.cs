using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFlow.Core.Services
{
    public class ProcessExecutionService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IProcessStepService _processStepService;
        private readonly SmartFlowSettings _smartFlowSettings;

        public ProcessExecutionService(IProcessRepository processRepository
            , IProcessStepService processStepService
            , SmartFlowSettings smartFlowSettings)
        {
            _processRepository = processRepository;
            _processStepService = processStepService;
            _smartFlowSettings = smartFlowSettings;
        }

        public ProcessExecution InitializeProcessExecution(Process process)
        {
            var processExecution = new ProcessExecution
            {
                Id = Guid.NewGuid(),
                Process = process,
                CreatedOn = DateTime.Now,
                ExecutionSteps = new List<ProcessExecutionStep>
                {
                    _processStepService
                        .GenerateProcessStep(process,
                            process.Transitions.First(x => x.From.IsInitial).From)
                }
            };

            if (_smartFlowSettings.Persist)
            {

            }

            return processExecution;
        }
    }
}
