using System;
using System.ComponentModel.DataAnnotations;

namespace FlowCiao.Models.Core
{
    public class Trigger : BaseEntity
    {
        public Trigger(int code, string name = null)
        {
            Code = code;
            Name = name;
        }

        public int Code { get; set; }
        
        public string Name { get; set; }
        
        public int TriggerType { get; set; }

        public int Priority { get; set; }
        
        [Required]
        public Guid TransitionId { get; set; }
        
        public Transition Transition { get; set; }
    }
}
