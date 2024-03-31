using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlowCiao.Models.Core
{
    public class Transition
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid FlowId { get; set; }
        public Guid CurrentStateId { get; set; }
        public Guid NextStateId { get; set; }
        public State From { get; set; }
        public State To { get; set; }
        public List<Trigger> Triggers { get; set; }
        public List<Activity> Activities { get; set; }
        public Func<bool> Condition { get; set; }
    }

}
