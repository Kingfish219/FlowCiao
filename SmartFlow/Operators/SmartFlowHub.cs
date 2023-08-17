using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Models;
using SmartFlow.Models.Flow;

namespace SmartFlow.Operators
{
    public class SmartFlowHub
    {
        public List<Process> Processes { get; set; }
        public List<ProcessExecution> ProcessExecutions { get; set; }
        public bool IsInitiated { get; private set; }

        public async Task Initiate(List<Process> processes,
            List<ProcessExecution> processExecutions)
        {
            await Task.CompletedTask;
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

        public async Task<List<Process>> RetreiveFlow(Guid processId = default, string key = null)
        {
            return await Task.Run(() =>
            {
                return Processes?
                    .Where(x => x.FlowKey.Equals(smartFlowKey))
                    .ToList() ?? null;

                var result = from o in Processes
                             where (string.IsNullOrWhiteSpace(smartFlowKey) || o.FlowKey.Equals(smartFlowKey, System.StringComparison.InvariantCultureIgnoreCase))
                             && (State == null || o.State == State)
                             select o;
            });
        }

        public async Task<List<ProcessExecution>> RetreiveFlowExecution(string smartFlowKey)
        {
            return await Task.Run(() =>
            {
                return ProcessExecutions?
                    .Where(x => x.Process?.FlowKey?.Equals(smartFlowKey) ?? false)
                    .ToList() ?? null;
            });
        }
    }
}
