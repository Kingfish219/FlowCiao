using SmartFlow.Core.Db;
using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    internal abstract class WorkflowHandler : IWorkflowHandler
    {
        internal IWorkflowHandler NextHandler { get; private set; }
        internal IWorkflowHandler PreviousHandler { get; private set; }

        protected readonly IProcessRepository ProcessRepository;
        protected readonly IProcessStepManager ProcessStepManager;
        protected readonly ProcessStepContext ProcessStepContext;

        internal WorkflowHandler(IProcessRepository processRepository, IProcessStepManager processStepManager, ProcessStepContext processStepContext)
        {
            ProcessRepository = processRepository;
            ProcessStepContext = processStepContext;
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

        public abstract ProcessResult Handle();
        public abstract ProcessResult RollBack();
    }
}
