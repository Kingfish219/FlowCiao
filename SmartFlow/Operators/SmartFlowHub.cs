using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Models;
using SmartFlow.Models.Flow;

namespace SmartFlow.Operators
{
    public class SmartFlowHub
    {
        private List<Process> ProcessHub { get; set; }
        private List<ProcessExecution> ProcessExecutionHub { get; set; }
        public bool IsInitiated { get; private set; }

        public async Task Initiate(List<Process> processes,
            List<ProcessExecution> processExecutions)
        {
            await Task.CompletedTask;
            ProcessHub = processes;
            ProcessExecutionHub = processExecutions;
            IsInitiated = true;
        }

        public async Task RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : Process
        {
            await Task.Run(() =>
            {
                ProcessHub.Add(smartFlow);
            });
        }

        public async Task RegisterFlowExecution<T>(T processExecution) where T : ProcessExecution
        {
            await Task.Run(() =>
            {
                ProcessExecutionHub.Add(processExecution);
            });
        }

        public async Task<List<Process>> RetreiveFlow(string smartFlowKey)
        {
            return await Task.Run(() =>
            {
                return ProcessHub
                    .Where(x => x.FlowKey.Equals(smartFlowKey))
                    .ToList();
            });
        }

        public async Task<List<ProcessExecution>> RetreiveFlowExecution(string smartFlowKey)
        {
            return await Task.Run(() =>
            {
                return ProcessExecutionHub
                    .Where(x => x.Process?.FlowKey?.Equals(smartFlowKey) ?? false)
                    .ToList();
            });
        }
    }
}
