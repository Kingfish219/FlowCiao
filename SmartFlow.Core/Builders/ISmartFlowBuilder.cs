﻿using SmartFlow.Core.Interfaces;
using System;

namespace SmartFlow.Core.Builders
{
    public interface ISmartFlowBuilder
    {
        public ISmartFlow Build<T>(Action<ISmartFlowBuilder> action) where T : ISmartFlow, new();
    }
}
