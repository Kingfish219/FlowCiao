﻿using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handlers
{
    internal abstract class WorkflowHandler : IProcessHandler
    {
        internal IProcessHandler NextHandler { get; private set; }
        internal IProcessHandler PreviousHandler { get; private set; }

        protected readonly IProcessRepository ProcessRepository;
        protected readonly IProcessService ProcessService;

        internal WorkflowHandler(IProcessRepository processRepository, IProcessService processStepManager)
        {
            ProcessRepository = processRepository;
            ProcessService = processStepManager;
        }

        public void SetNextHandler(IProcessHandler handler)
        {
            NextHandler = handler;
        }

        public void SetPreviousHandler(IProcessHandler handler)
        {
            PreviousHandler = handler;
        }

        public abstract ProcessResult Handle(ProcessStepContext processStepContext);
        public abstract ProcessResult RollBack(ProcessStepContext processStepContext);
    }
}
