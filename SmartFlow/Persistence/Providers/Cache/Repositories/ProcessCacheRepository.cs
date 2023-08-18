using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.Persistence.Providers.Cache.Repositories
{
    internal class ProcessCacheRepository : SmartFlowCacheRepository, IProcessRepository
    {
        public ProcessCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public Task<bool> AddProcessStepHistoryActivity(ProcessStepHistoryActivity processStepHistoryActivity)
        {
            throw new NotImplementedException();
        }

        public Task<FuncResult> ApplyProcessStepComment(ProcessEntity entity, ProcessUser processUser, string comment, List<Guid> attachmentIds = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApplyProcessStepCommentAttachment(Guid processStepCommentId, Guid attachmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Create(Process entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Process>> Get(Guid processId = default, string key = null)
        {
            var db = GetDbConnection();
            var result = (from o in db.Processes
                          where (string.IsNullOrWhiteSpace(key) || o.FlowKey.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                          && (processId == default || o.Id.Equals(processId))
                          select o).ToList();

            return result;
        }

        public Task<ProcessExecutionStep> GetActiveProcessStep(ProcessEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TransitionAction>> GetActiveTransitions(ProcessEntity entityp, Guid ProcessId)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessExecutionStep> GetLastProcessStepHistoryItem(Guid ticketId)
        {
            throw new NotImplementedException();
        }

        public Task<Process> GetProcess(Guid userId, Guid requestTypeId)
        {
            throw new NotImplementedException();
        }

        public State GetStartState()
        {
            throw new NotImplementedException();
        }

        public Task<State> GetState(Guid stateId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Activity>> GetStateActivities(State state, Group group)
        {
            throw new NotImplementedException();
        }

        public Task<List<TransitionAction>> GetStateTransitions(Process process, State state)
        {
            throw new NotImplementedException();
        }

        public Task<State> GetStatusById(Guid statusId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Activity>> GetTransitionActivities(Transition transition)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProcessStepCommentRead(ProcessEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveActiveProcessStep(ProcessEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<FuncResult> RemoveEntireFlow(ProcessExecutionStep processStep)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetToHistory(Guid EntityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetToHistoryForGoToStep(Guid newId, Guid entityId, Guid targetStatus, Guid processGuid)
        {
            throw new NotImplementedException();
        }
    }
}
