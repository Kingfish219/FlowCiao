using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
    public class Transition
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        [ForeignKey("FlowId")]
        public Flow Flow { get; set; }
        
        [ForeignKey("FromStateId")]
        public State From { get; set; }
        
        [ForeignKey("ToStateId")]
        public State To { get; set; }
        
        public List<Trigger> Triggers { get; set; }
        
        public List<Activity> Activities { get; set; }
        
        [NotMapped]
        public Func<bool> Condition { get; set; }
    }
}
