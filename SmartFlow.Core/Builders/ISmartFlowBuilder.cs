
using SmartFlow.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public interface ISmartFlowBuilder
    {
        public List<ISmartFlowStepBuilder> StepBuilders { get; set; }
        public ISmartFlowStepBuilder NewStep();
        public ISmartFlowStepBuilder NewStep(ISmartFlowStepBuilder builder);
        public ISmartFlow Build<T>(Action<ISmartFlowBuilder> action) where T : ISmartFlow, new();
        public ISmartFlow Build<T>() where T : ISmartFlow, new();
    }
}
