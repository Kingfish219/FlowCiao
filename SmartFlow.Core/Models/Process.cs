
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class Process
    {
        public Process()
        {
            Transitions = new List<Transition>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Transition> Transitions { get; set; }
    }
}
