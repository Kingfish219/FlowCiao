using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using SmartFlow.Core.Services;

namespace SmartFlow.Core.Operators
{
    public class SmartFlowOperator : ISmartFlowOperator
    {
        private readonly List<ISmartFlow> _flows;
        private readonly LogRepository _logRepository;
        private readonly ISmartFlowRepository _smartFlowRepository;
        private readonly ProcessStepService _processStepService;

        public SmartFlowOperator(LogRepository logRepository
            , ISmartFlowRepository smartFlowRepository)
        {
            _flows = new List<ISmartFlow>();
            _logRepository = logRepository;
            _smartFlowRepository = smartFlowRepository;
            _processStepService = new ProcessStepService(_smartFlowRepository);
        }

        public Task<bool> RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : ISmartFlow, new()
        {
            return Task.Run(() =>
            {
                _flows.Add(smartFlow);

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
            var smartFlow = _flows.Single(x => x.FlowKey.Equals(smartFlowKey));

            try
            {
                var logRepository = new LogRepository(_settings.ConnectionString);
                var processRepository = new ProcessRepository(_settings.ConnectionString);
                var processStepManager = new ProcessStepService(processRepository);
                ProcessStep processStep;
                if (commandType == EntityCommandType.Create)
                {
                    processStep = processStepManager.InitializeActiveProcessStep(user.Id, entity, true);
                    input.ActionCode = 1;
                }
                else
                {
                    processStep = processStepManager.GetActiveProcessStep(user.Id, entity);
                }

                if (processStep is null)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Completed
                    };
                }

                var processStepContext = new ProcessStepContext
                {
                    ProcessStep = processStep,
                    ProcessUser = user,
                    ProcessStepInput = input,
                    Data = parameters,
                    EntityCommandType = commandType,
                };

                var handlers = ProcessHandlerFactory.BuildHandlers(
                    processStepContext,
                    processRepository,
                    processStepManager,
                    entityRepository,
                    logRepository,
                    _settings.ConnectionString
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
