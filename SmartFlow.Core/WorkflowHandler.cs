
using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    internal abstract class WorkflowHandler : IWorkflowHandler
    {
        internal IWorkflowHandler NextHandler { get; private set; }
        internal IWorkflowHandler PreviousHandler { get; private set; }
        protected readonly IProcessRepository ProcessRepository;

        internal WorkflowHandler(IProcessRepository processRepository)
        {
            ProcessRepository = processRepository;
        }

        public void SetNextHandler(IWorkflowHandler handler)
        {
            NextHandler = handler;
        }

        public void SetPreviousHandler(IWorkflowHandler handler)
        {
            PreviousHandler = handler;
        }

        public abstract ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext);
        public abstract ProcessResult RollBack(ProcessStep processStep);
    }
}
