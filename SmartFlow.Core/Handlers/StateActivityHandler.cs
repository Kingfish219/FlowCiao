using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;

namespace SmartFlow.Core.Handlers
{
    internal class StateActivityHandler : WorkflowHandler
    {
        public StateActivityHandler(IStateMachineRepository processRepository, IProcessStepService processStepManager) : base(processRepository, processStepManager)
        {
        }

        //internal StateActivityHandler(IProcessRepository processRepository, int actionCode, string connectionString, LogRepository logRepository) 
        //    : base(processRepository)
        //{
        //    _actionCode = actionCode;
        //    _ProcessRepository = processRepository;
        //    _connectionString = connectionString;
        //    _LogRepository = logRepository;
        //}

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                var result = new ProcessResult
                {
                    Status = ProcessResultStatus.Completed
                };

                var stateCurrent = new State
                {
                    Id = processStepContext.ProcessStep.TransitionActions.FirstOrDefault().Transition.CurrentStateId
                };

                var activities = ProcessRepository.GetStateActivities(stateCurrent, new Group()).Result;
                if (activities.Count > 0)
                {
                    var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(type => type.GetTypes())
                        .Where(p => typeof(Activity).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract && p.BaseType == typeof(Activity));

                    foreach (var type in types)
                    {
                        var activity = (IProcessActivity)Activator.CreateInstance(type, processStepContext);
                        if (!activities.Exists(x => x.ActivityTypeCode == ((Activity)activity).ActivityTypeCode))
                        {
                            continue;
                        }

                        result = activity.Execute();
                        if (result.Status != ProcessResultStatus.Completed && result.Status != ProcessResultStatus.SetOwner)
                        {
                            throw new SmartFlowProcessExecutionException("Exception occured while invoking activities" + result.Message);
                        }

                        //log to ProcessStepHistoryActivity
                        var currentActivity = activities.Find(a => a.ActivityTypeCode == ((Activity)activity).ActivityTypeCode);
                        Guid LastProcessStepHistoryItemId = ProcessRepository.GetLastProcessStepHistoryItem(processStepContext.ProcessStep.Entity.Id).Result.Id;
                        ProcessRepository.AddProcessStepHistoryActivity(new ProcessStepHistoryActivity { ActivityId = currentActivity.Id, ActivityName = currentActivity.Name, StepType = 1, ProcessStepHistoryId = LastProcessStepHistoryItemId });
                    }
                }

                if (NextHandler is null)
                {
                    return result;
                }

                return NextHandler.Handle(processStepContext);
            }
            catch (Exception exception)
            {
                return new ProcessResult
                {
                    Status = ProcessResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }

        public override ProcessResult RollBack(ProcessStepContext processStepContext)
        {
            try
            {
                ProcessRepository.RemoveEntireFlow(processStepContext.ProcessStep).GetAwaiter().GetResult();

                return PreviousHandler?.RollBack(processStepContext) ?? new ProcessResult
                {
                    Status = ProcessResultStatus.Failed
                };
            }
            catch (Exception exception)
            {
                return new ProcessResult
                {
                    Status = ProcessResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }
    }
}
