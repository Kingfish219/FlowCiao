using System;
using System.Collections.Generic;
using FlowCiao.Models.Flow;

namespace FlowCiao.Models
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
