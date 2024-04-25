using System;
using System.Collections.Generic;
using FlowCiao.Models.Core;

namespace FlowCiao.Models.Execution
{
    internal class FlowInstanceStep
    {
        public List<FlowInstanceStepDetail> Details { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime CompletedOn { get; set; }
    }

    public class FlowInstanceStepDetail
    {
        public Guid Id { get; set; }
        public Transition Transition { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime CompletedOn { get; set; }
    }
}
