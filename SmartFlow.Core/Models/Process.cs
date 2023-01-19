using System;
using System.Collections.Generic;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Interfaces;

namespace SmartFlow.Core.Models
{
    public class Process : IStateMachine
    {
        public Process()
        {
            Transitions = new List<Transition>();
        }

        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Transition> Transitions { get; set; }
        public IStateMachine Construct<T>(IStateMachineBuilder action) where T : IStateMachine, new()
        {
            throw new NotImplementedException();
        }
    }
}
