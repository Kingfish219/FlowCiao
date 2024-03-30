using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace FlowCiao.Models.Core
{
    [Table("Transition")]
    public class Transition
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProcessId { get; set; }
        public Guid CurrentStateId { get; set; }
        public Guid NextStateId { get; set; }
        public State From { get; set; }
        public State To { get; set; }
        public List<ProcessAction> Actions { get; set; }
        public List<Activity> Activities { get; set; }
        public Func<bool> Condition { get; set; }
    }

}
