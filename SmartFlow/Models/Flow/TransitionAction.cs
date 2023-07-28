using System;
using Dapper.Contrib.Extensions;

namespace SmartFlow.Models.Flow
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
