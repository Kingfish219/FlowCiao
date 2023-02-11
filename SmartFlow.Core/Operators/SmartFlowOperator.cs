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
        private readonly List<Process> _flowHub;
        private readonly LogRepository _logRepository;
        private readonly IProcessRepository _processRepository;
        private readonly ProcessStepService _processStepService;
        private readonly ProcessHandlerFactory _processHandlerFactory;

        public SmartFlowOperator(LogRepository logRepository
            , IProcessRepository smartFlowRepository
            , ProcessHandlerFactory processHandlerFactory
            , ProcessStepService processStepService)
        {
            _flowHub = new List<Process>();
            _logRepository = logRepository;
            _processRepository = smartFlowRepository;
            _processStepService = processStepService;
            _processHandlerFactory = processHandlerFactory;
        }

        public Task<bool> RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : Process, new()
        {
            return Task.Run(() =>
            {
                _flowHub.Add(smartFlow);

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

        public ProcessResult Fire(ISmartFlow smartFlow, int action)
        {
            throw new NotImplementedException();
        }

        public ProcessResult Fire(string smartFlowKey, int action, Dictionary<string, object> data)
        {
            var process = _flowHub.Single(x => x.FlowKey.Equals(smartFlowKey));

            try
            {
                if (process.ActiveProcessStep is null)
                {
                    process.ActiveProcessStep = _processStepService.InitializeActiveProcessStep(process);
                }

                if (process.ActiveProcessStep is null)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Completed
                    };
                }

                var processStepContext = new ProcessStepContext
                {
                    ProcessStepDetail = process.ActiveProcessStep,
                    Data = data
                };

                var handlers = _processHandlerFactory.BuildHandlers(
                    processStepContext
                   );

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
