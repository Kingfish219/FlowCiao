using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SmartFlow.Models.Flow;

namespace SmartFlow.Models
{
    public class ProcessExecution
    {
        public Guid Id { get; set; }
        public Process Process { get; set; }
        public ProcessExecutionState ExecutionState { get; set; }
        public List<ProcessExecutionStep> ExecutionSteps { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Progress
        {
            get => JsonConvert.SerializeObject(ExecutionSteps);
            set => ExecutionSteps = JsonConvert.DeserializeObject<List<ProcessExecutionStep>>(value);
        }
        public State State { get; set; }

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
