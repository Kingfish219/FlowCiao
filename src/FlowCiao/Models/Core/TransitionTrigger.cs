#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
     public class TransitionTrigger
    {
        [Key]
        public Guid Id { get; set; }
        
        [ForeignKey("Trigger")]
        public Guid TriggerId { get; set; }
        
        [ForeignKey("Transition")]
        public Guid TransitionId { get; set; }
        
        public Transition? Transition { get; set; } = null;
        public Trigger? Trigger { get; set; } = null;
    }
}
