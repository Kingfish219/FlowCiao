using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System;
using System.Linq;

namespace SmartFlow.Core.Services
{
    public class ProcessExecutionService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IProcessStepService _processStepService;

        public ProcessExecutionService(IProcessRepository processRepository
            , IProcessStepService processStepService)
        {
            _processRepository = processRepository;
            _processStepService = processStepService;
        }

        public ProcessExecution InitializeProcessExecution(Process process)
        {
            var processExecution = new ProcessExecution
            {
                Id = Guid.NewGuid(),
                Process = process,
                CreatedOn = DateTime.Now
            };

            processExecution.ActiveProcessStep = _processStepService.GenerateProcessStep(process, process.Transitions.Single(x => x.From.IsInitial).From);

            return processExecution;
        }
    }
}
