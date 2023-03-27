using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Core.Handlers;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using SmartFlow.Core.Services;

namespace SmartFlow.Core.Operators
{
    public class SmartFlowOperator : ISmartFlowOperator
    {
        private readonly List<Process> _processHub;
        private readonly List<ProcessExecution> _processExecutionHub;
        private readonly LogRepository _logRepository;
        private readonly IProcessRepository _processRepository;
        private readonly IProcessStepService _processStepService;
        private readonly ProcessExecutionService _processExecutionService;
        private readonly ProcessHandlerFactory _processHandlerFactory;

        public SmartFlowOperator(LogRepository logRepository
            , IProcessRepository smartFlowRepository
            , ProcessHandlerFactory processHandlerFactory
            , IProcessStepService processStepService
            , ProcessExecutionService processExecutionService)
        {
            _logRepository = logRepository;
            _processRepository = smartFlowRepository;
            _processStepService = processStepService;
            _processHandlerFactory = processHandlerFactory;
            _processHub = new List<Process>();
            _processExecutionHub = new List<ProcessExecution>();
            _processExecutionService = processExecutionService;
        }

        public Task<bool> RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : Process, new()
        {
            return Task.Run(() =>
            {
                _processHub.Add(smartFlow);

                return true;
            });
        }

        public Task<ProcessResult> ExecuteAsync(ISmartFlow process)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessResult> AdvanceAsync(ProcessEntity entity, ProcessUser user, ProcessStepInput input, IEntityRepository entityRepository,
            EntityCommandType commandType = EntityCommandType.Update, Dictionary<string, object> parameters = null)
        {
            throw new NotImplementedException();
        }

        public ProcessResult Fire(string smartFlowKey, int action, Dictionary<string, object> data = null)
        {
            var processExecution = _processExecutionHub.SingleOrDefault(x => x.Process.FlowKey.Equals(smartFlowKey));

            try
            {
                if (processExecution is null)
                {
                    var process = _processHub.Single(x => x.FlowKey.Equals(smartFlowKey));
                    processExecution = _processExecutionService.InitializeProcessExecution(process);
                }

                var activeProcessStep = processExecution.ExecutionSteps
                    .SingleOrDefault(x => !x.IsCompleted);

                if (activeProcessStep is null)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Completed
                    };
                }

                var processStepContext = new ProcessStepContext
                {
                    ProcessStepDetail = activeProcessStep.Details.Single(x => x.Transition.Actions.FirstOrDefault().ActionTypeCode == action),
                    Data = data
                };

                var handlers = _processHandlerFactory.BuildHandlers(processStepContext);

                var result = handlers.Peek().Handle(processStepContext);

                return result;
            }
            catch (Exception exception)
            {
                return new ProcessResult
                {
                    Status = ProcessResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }
    }
}
