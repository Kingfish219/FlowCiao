using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
     public class TransitionActivity
    {
        [ForeignKey("Activity")]
        public Guid ActivityId { get; set; }
        
        [ForeignKey("Transition")]
        public Guid TransitionId { get; set; }
        
        public Transition Transition { get; set; } = null;
        public Activity Activity { get; set; } = null;
    }
}
