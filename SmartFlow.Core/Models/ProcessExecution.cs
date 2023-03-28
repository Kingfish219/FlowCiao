using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartFlow.Core.Models
{
    public class ProcessExecution
    {
        public Guid Id { get; set; }
        public Process Process { get; set; }
        public ProcessExecutionState State { get; set; }
        public List<ProcessExecutionStep> ExecutionSteps { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Progress
        {
            get => JsonConvert.SerializeObject(ExecutionSteps);
            set => ExecutionSteps = JsonConvert.DeserializeObject<List<ProcessExecutionStep>>(value);
        }

        public enum ProcessExecutionState
        {
            Initial,
            Pending,
            Running,
            Suspended,
            Finished
        }
    }
}
