using System;
using System.Collections.Generic;
using SmartFlow.Models.Flow;

namespace SmartFlow.Models
{
    public class ProcessExecutionStep
    {
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
