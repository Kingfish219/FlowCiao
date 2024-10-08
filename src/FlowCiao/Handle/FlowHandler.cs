﻿using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Services;

namespace FlowCiao.Handle
{
    internal abstract class FlowHandler : IFlowHandler
    {
        internal IFlowHandler NextHandler { get; private set; }
        internal IFlowHandler PreviousHandler { get; private set; }

        protected readonly IFlowRepository FlowRepository;
        protected readonly IFlowService FlowService;

        internal FlowHandler(IFlowRepository flowRepository, IFlowService flowService)
        {
            FlowRepository = flowRepository;
            FlowService = flowService;
        }

        public void SetNextHandler(IFlowHandler handler)
        {
            NextHandler = handler;
        }

        public void SetPreviousHandler(IFlowHandler handler)
        {
            PreviousHandler = handler;
        }

        public abstract FlowResult Handle(FlowStepContext flowStepContext);
        public abstract FlowResult RollBack(FlowStepContext flowStepContext);
    }
}
