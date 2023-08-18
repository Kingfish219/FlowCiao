using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Models;
using SmartFlow.Models.Flow;

namespace SmartFlow.Persistence.Providers.Cache
{
    public class SmartFlowHub
    {
        public List<Process> Processes { get; set; }
        public List<ProcessExecution> ProcessExecutions { get; set; }
        public bool IsInitiated { get; private set; }

        public void Initiate(List<Process> processes,
            List<ProcessExecution> processExecutions)
        {
            Processes = processes;
            ProcessExecutions = processExecutions;
            IsInitiated = true;
        }

        public async Task RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : Process
        {
            await Task.Run(() =>
            {
                Processes.Add(smartFlow);
            });
        }

        public async Task RegisterFlowExecution<T>(T processExecution) where T : ProcessExecution
        {
            await Task.Run(() =>
            {
                ProcessExecutions.Add(processExecution);
            });
        }
    }
}
