using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
     public class TransitionActivity : BaseNavigationEntity
    {
        [ForeignKey("Activity")]
        public Guid ActivityId { get; set; }
        
        [ForeignKey("Transition")]
        public Guid TransitionId { get; set; }
        
        public Transition Transition { get; set; }
        public Activity Activity { get; set; }
    }
}
