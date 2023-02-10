using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace SmartFlow.Core.Models
{
    [Table("Process")]
    public class Process
    {
        public Process()
        {
            Transitions = new List<Transition>();
        }

        [Key]
        public Guid Id { get; set; }
        public string FlowKey { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Transition> Transitions { get; set; }
        public List<ProcessStepHistory> MyProperty { get; set; }
    }
}
