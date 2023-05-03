using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Services;

namespace SmartFlow.Operators
{
    public class SmartFlowHub
    {
        private readonly List<Process> _processHub;
        private readonly List<ProcessExecution> _processExecutionHub;
        public SmartFlowHub(ProcessExecutionService processExecutionService,
            SmartFlowSettings smartFlowSettings,
            IProcessService processService)
        {
            if (smartFlowSettings.PersistFlow)
            {
                _processHub = processService.Get().GetAwaiter().GetResult();
                _processExecutionHub = processExecutionService.Get().GetAwaiter().GetResult();
            }
            else
            {
                _processExecutionHub = new List<ProcessExecution>();
                _processHub = new List<Process>();
            }
        }

        public async Task RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : Process
        {
            await Task.Run(() =>
            {
                _processHub.Add(smartFlow);
            });
        }

        public async Task RegisterFlowExecution<T>(T processExecution) where T : ProcessExecution
        {
            await Task.Run(() =>
            {
                _processExecutionHub.Add(processExecution);
            });
        }

        public async Task<List<Process>> RetreiveFlow(string smartFlowKey)
        {
            return await Task.Run(() =>
            {
                return _processHub
                    .Where(x => x.FlowKey.Equals(smartFlowKey))
                    .ToList();
            });
        }

        public async Task<List<ProcessExecution>> RetreiveFlowExecution(string smartFlowKey)
        {
            return await Task.Run(() =>
            {
                return _processExecutionHub
                    .Where(x => x.Process?.FlowKey?.Equals(smartFlowKey) ?? false)
                    .ToList();
            });
        }
    }
}
