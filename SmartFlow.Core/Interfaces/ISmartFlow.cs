using SmartFlow.Core.Builders;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Interfaces
{
    public interface ISmartFlow
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public List<Transition> Transitions { get; set; }
        public ISmartFlow Construct<T>(ISmartFlowBuilder action) where T : ISmartFlow, new();
    }
}
