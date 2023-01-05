
using System;
using Dapper.Contrib.Extensions;

namespace SmartFlow.Core.Models
{
    [Table("TransitionAction")]
     public class TransitionAction
    {
        [Key]
        public Guid Id { get; set; }
        public ProcessAction Action { get; set; }
        public Transition Transition { get; set; }
    }
}
