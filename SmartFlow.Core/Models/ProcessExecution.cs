using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    internal class ProcessExecution
    {
        public Process Process { get; set; }
        public ProcessExecutionState State { get; set; }
        public List<ProcessExecutionStep> ProcessStepHistory { get; set; }
        public ProcessExecutionStep ActiveProcessStep { get; set; }
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
