using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFlow.Core.Db
{
    public interface ISmartFlowRepository
    {
        Task<List<TransitionAction>> GetStateTransitions(Process process, State state);
        Task<List<TransitionAction>> GetActiveTransitions(ProcessEntity entityp, Guid ProcessId);
        Task<List<Activity>> GetStateActivities(State state, Group group);
        Task<bool> RemoveActiveProcessStep(ProcessEntity entity);
        Task<Process> GetProcess(Guid userId, Guid requestTypeId);
        Task<ISmartFlow> GetProcess<T>(Guid processId = default, string key = default) where T : ISmartFlow;
        Task<Guid> Create<T>(ISmartFlow entity) where T : ISmartFlow;
        Task<bool> CreateProcessStep(ProcessStep entity);
        Task<bool> CompleteProgressAction(ProcessStep processStep, ProcessAction action);
        Task<State> GetState(Guid stateId);
        Task<ProcessStep> GetActiveProcessStep(ProcessEntity entity);
        Task<bool> SetToHistory(Guid EntityId);
        Task<bool> SetToHistoryForGoToStep(Guid newId, Guid entityId, Guid targetStatus, Guid processGuid);
        Task<ProcessStep> GetLastProcessStepHistoryItem(Guid ticketId);
        Task<bool> AddProcessStepHistoryActivity(ProcessStepHistoryActivity processStepHistoryActivity);
        Task<List<Activity>> GetTransitionActivities(Transition transition);
        Task<State> GetStatusById(Guid statusId);
        Task<FuncResult> RemoveEntireFlow(ProcessStep processStep);
        Task<bool> ProcessStepCommentRead(ProcessEntity entity);
        Task<FuncResult> ApplyProcessStepComment(ProcessEntity entity, ProcessUser processUser, string comment, List<Guid> attachmentIds = default);
        Task<bool> ApplyProcessStepCommentAttachment(Guid processStepCommentId, Guid attachmentId);
        State GetStartState();
    }
}
