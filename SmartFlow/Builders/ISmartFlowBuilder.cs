using System;
using System.Collections.Generic;
using SmartFlow.Models.Flow;

namespace SmartFlow.Builders
{
    public interface ISmartFlowBuilder
    {
        List<ISmartFlowStepBuilder> StepBuilders { get; set; }
        ISmartFlowStepBuilder InitialStepBuilder { get; set; }
        ISmartFlowBuilder Initial(Action<ISmartFlowStepBuilder> action);
        ISmartFlowBuilder NewStep(Action<ISmartFlowStepBuilder> action);
        Process Build<T>(Action<ISmartFlowBuilder> action) where T : ISmartFlow, new();
        Process Build<T>() where T : ISmartFlow, new();
    }
}
