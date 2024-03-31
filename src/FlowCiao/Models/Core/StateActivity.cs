#nullable enable
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
    internal class StateActivity
    {
        [ForeignKey("State")]
        public Guid StateId { get; set; }
        
        [ForeignKey("Activity")]
        public Guid ActivityId { get; set; }

        public Activity? Activity { get; set; } = null;
        public State? State { get; set; } = null;
    }
}
