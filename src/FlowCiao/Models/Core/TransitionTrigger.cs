using System;
using System.ComponentModel.DataAnnotations;

namespace FlowCiao.Models.Core
{
     public class TransitionTrigger
    {
        [Key]
        public Guid Id { get; set; }
        public Trigger Trigger { get; set; }
        public Transition Transition { get; set; }
    }
}
