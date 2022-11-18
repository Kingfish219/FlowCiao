
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Action = SmartFlow.Core.Models.Action;

namespace SmartFlow.Core.Db
{
    public interface IProcessRepository
    {
        Task<List<TransitionAction>> GetStateTransitions(Process process, State state);
        Task<List<TransitionAction>> GetActiveTransitions(Entity entityp,Guid ProcessId);
        Task<List<Activity>> GetStateActivities(State state, Group group);
        Task<bool> RemoveActiveProcessStep(Entity entity);
        Task<Process> GetProcess(Guid userId, Guid requestTypeId);
        Task<Process> GetProcess(Guid processId);
        Task<bool> CreateProcessStep(ProcessStep entity);
        Task<bool> CompleteProgressAction(ProcessStep processStep, Action action);
        Task<Status> GetState(Guid stateId);
        Task<ProcessStep> GetActiveProcessStep(Entity entity);
        Task<bool> SetToHistory(Guid EntityId);
        Task<bool> SetToHistoryForGoToStep(Guid newId, Guid entityId, Guid targetStatus, Guid processGuid);
        Task<ProcessStep> GetLastProcessStepHistoryItem(Guid ticketId);
        Task<bool> AddProcessStepHistoryActivity(ProcessStepHistoryActivity processStepHistoryActivity);
        Task<List<Activity>> GetTransitionActivities(Transition transition);
        Task<Status> GetStatusById(Guid statusId);
        Task<FuncResult> RemoveEntireFlow(ProcessStep processStep);
        Task<bool> ProcessStepCommentRead(Entity entity);
        Task<FuncResult> ApplyProcessStepComment(Entity entity, ProcessUser processUser, string comment, List<Guid> attachmentIds = default);
        Task<bool> ApplyProcessStepCommentAttachment(Guid processStepCommentId, Guid attachmentId);
        State GetStartState();
    }
}