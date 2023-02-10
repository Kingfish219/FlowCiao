using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public interface ISmartFlowBuilder
    {
        public List<ISmartFlowStepBuilder> StepBuilders { get; set; }
        public ISmartFlowStepBuilder Initial();
        public ISmartFlowStepBuilder NewStep();
        public ISmartFlowStepBuilder NewStep(ISmartFlowStepBuilder builder);
        public Process Build<T>(Action<ISmartFlowBuilder> action) where T : ISmartFlow, new();
        public Process Build<T>() where T : ISmartFlow, new();
    }
}
