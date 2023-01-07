using SmartFlow.Core.Builders;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Interfaces
{
    public interface IStateMachine : ISmartFlow
    {
        public List<Transition> Transitions { get; set; }
        public IStateMachine Construct<T>(IStateMachineBuilder action) where T : IStateMachine, new();
    }
}
