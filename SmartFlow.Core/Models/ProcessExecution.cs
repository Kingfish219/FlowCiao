using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class ProcessExecution
    {
        public Guid Id { get; set; }
        public Process Process { get; set; }
        public ProcessExecutionState State { get; set; }
        public List<ProcessExecutionStep> ExecutionSteps { get; set; }
        public DateTime CreatedOn { get; set; }

        public enum ProcessExecutionState
        {
            Pending,
            Running,
            Suspended,
            Finished
        }
    }
}
