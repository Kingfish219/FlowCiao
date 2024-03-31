using System;
using Dapper.Contrib.Extensions;

namespace FlowCiao.Models.Core
{
    [Table("TransitionTrigger")]
     public class TransitionTrigger
    {
        [Key]
        public Guid Id { get; set; }
        public Trigger Trigger { get; set; }
        public Transition Transition { get; set; }
    }
}
