
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public interface IStateMachineBuilder : ISmartFlowBuilder
    {
        public List<IStateMachineStepBuilder> StateMachineStepBuilders { get; set; }
        public IStateMachineStepBuilder NewStep();
        public IStateMachineStepBuilder NewStep(IStateMachineStepBuilder builder);
    }
}
