using SmartFlow.Core.Db;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Handlers
{
    internal abstract class WorkflowHandler : IProcessHandler
    {
        internal IProcessHandler NextHandler { get; private set; }
        internal IProcessHandler PreviousHandler { get; private set; }

        protected readonly IProcessRepository ProcessRepository;
        protected readonly IProcessStepService ProcessStepManager;

        internal WorkflowHandler(IProcessRepository processRepository, IProcessStepService processStepManager)
        {
            ProcessRepository = processRepository;
            ProcessStepManager = processStepManager;
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
