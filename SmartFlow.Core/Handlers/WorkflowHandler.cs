using SmartFlow.Core.Db;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Handlers
{
    internal abstract class WorkflowHandler : IWorkflowHandler
    {
        internal IWorkflowHandler NextHandler { get; private set; }
        internal IWorkflowHandler PreviousHandler { get; private set; }

        protected readonly IProcessRepository ProcessRepository;
        protected readonly IProcessStepManager ProcessStepManager;

        internal WorkflowHandler(IProcessRepository processRepository, IProcessStepManager processStepManager)
        {
            ProcessRepository = processRepository;
            ProcessStepManager = processStepManager;
        }

        public void SetNextHandler(IWorkflowHandler handler)
        {
            NextHandler = handler;
        }

        public void SetPreviousHandler(IWorkflowHandler handler)
        {
            PreviousHandler = handler;
        }

        public abstract ProcessResult Handle(ProcessStepContext processStepContext);
        public abstract ProcessResult RollBack(ProcessStepContext processStepContext);
    }
}
