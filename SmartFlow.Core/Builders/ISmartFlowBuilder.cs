using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public interface ISmartFlowBuilder
    {
        List<ISmartFlowStepBuilder> StepBuilders { get; set; }
        ISmartFlowStepBuilder InitialStepBuilder { get; set; }
        ISmartFlowStepBuilder Initial();
        ISmartFlowStepBuilder NewStep();
        ISmartFlowStepBuilder NewStep(ISmartFlowStepBuilder builder);
        Process Build<T>(Action<ISmartFlowBuilder> action) where T : ISmartFlow, new();
        Process Build<T>() where T : ISmartFlow, new();
    }
}
