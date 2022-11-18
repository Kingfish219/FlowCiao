using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFlow.Core.Models
{
    public class ProcessStepHistoryActivity
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int StepType { get; set; }
        public Guid ProcessStepHistoryId { get; set; }
    }
}
