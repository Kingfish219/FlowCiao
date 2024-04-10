using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
    public class StateActivity : BaseNavigationEntity
    {
        [ForeignKey("State")]
        public Guid StateId { get; set; }
        
        [ForeignKey("Activity")]
        public Guid ActivityId { get; set; }

        public Activity Activity { get; set; } = null;
        public State State { get; set; } = null;
    }
}
