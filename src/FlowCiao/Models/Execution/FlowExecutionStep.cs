using System;
using System.Collections.Generic;
using FlowCiao.Models.Core;

namespace FlowCiao.Models.Execution
{
    public class FlowExecutionStep
    {
        public List<FlowExecutionStepDetail> Details { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime CompletedOn { get; set; }
    }

    public class FlowExecutionStepDetail
    {
        public Guid Id { get; set; }
        public Transition Transition { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime CompletedOn { get; set; }
    }
}
