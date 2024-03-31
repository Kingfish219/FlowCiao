using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
    public class Trigger
    {
        public Trigger(int code)
        {
            Code = code;
        }

        [Key]
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int TriggerType { get; set; }
        
        [ForeignKey("FlowId")]
        public Flow Flow { get; set; }

        public List<Transition> Transitions { get; set; }
        
        public List<TransitionTrigger> TransitionTriggers { get; set; }
        
        public int Priority { get; set; }
    }
}
