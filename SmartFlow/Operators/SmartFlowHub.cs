using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Models;
using SmartFlow.Models.Flow;

namespace SmartFlow.Operators
{
    public class SmartFlowHub
    {
        private List<Process> _processHub;
        private List<ProcessExecution> _processExecutionHub;

        public async Task Initiate(List<Process> processes,
            List<ProcessExecution> processExecutions)
        {
            await Task.CompletedTask;
            _processHub = processes;
            _processExecutionHub = processExecutions;
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
