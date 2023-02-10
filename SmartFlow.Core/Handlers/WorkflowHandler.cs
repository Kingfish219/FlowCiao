using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories.Interfaces;

namespace SmartFlow.Core.Handlers
{
    internal abstract class WorkflowHandler : IProcessHandler
    {
        internal IProcessHandler NextHandler { get; private set; }
        internal IProcessHandler PreviousHandler { get; private set; }

        protected readonly IStateMachineRepository ProcessRepository;
        protected readonly IProcessStepService ProcessStepManager;

        internal WorkflowHandler(IStateMachineRepository processRepository, IProcessStepService processStepManager)
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
