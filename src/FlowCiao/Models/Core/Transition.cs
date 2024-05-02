using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
    public class Transition : BaseEntity
    {
        public string Name { get; set; }

        public Guid FlowId { get; set; }
        
        public Flow Flow { get; set; }

        [Required]
        public Guid FromId { get; set; }
        
        public State From { get; set; }
        
        [Required]
        public Guid ToId { get; set; }
        
        public State To { get; set; }

        public List<Trigger> Triggers { get; set; } = null!;
        
        public List<Activity> Activities { get; set; } = null!;
        
        public List<TransitionActivity> TransitionActivities { get; set; } = null!;
        
        [NotMapped]
        public Func<bool> Condition { get; set; }
    }
}
