using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class ProcessExecutionStep
    {
        public Process Process { get; set; }
        public List<ProcessExecutionStepDetail> Details { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime CompletedOn { get; set; }
    }

    public class ProcessExecutionStepDetail
    {
        public Guid Id { get; set; }
        public Transition Transition { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime CompletedOn { get; set; }
    }
}
