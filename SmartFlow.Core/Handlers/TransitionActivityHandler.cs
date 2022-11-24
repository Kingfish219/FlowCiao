using SmartFlow.Core.Db;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Models;
using System;
using System.Linq;

namespace SmartFlow.Core.Handlers
{
    internal class TransitionActivityHandler : WorkflowHandler
    {
        public TransitionActivityHandler(IProcessRepository processRepository
            , IProcessStepManager processStepManager) : base(processRepository, processStepManager)
        {
        }

        //public TransitionActivityHandler(IProcessRepository processRepository, int actionCode
        //    , string connectionString, LogRepository logRepository) 
        //    : base(processRepository)
        //{
        //    _actionCode = actionCode;
        //    _connectionString = connectionString;
        //    _processRepository = processRepository;
        //    _logRepository = logRepository;
        //}

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                var result = new ProcessResult
                {
                    Status = ProcessResultStatus.Completed
                };

                var currentTransition = processStepContext.ProcessStep.TransitionActions.FirstOrDefault().Transition;
                var activities = ProcessRepository.GetTransitionActivities(currentTransition).Result;

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

                        result = activity.Invoke();
                        if (result.Status != ProcessResultStatus.Completed && result.Status != ProcessResultStatus.SetOwner)
                        {
                            throw new SmartFlowProcessException("Exception occured while invoking activities" + result.Message);
                        }

                        //log to ProcessStepHistoryActivity
                        var currentActivity = activities.Find(a => a.ActivityTypeCode == ((Activity)activity).ActivityTypeCode);
                        Guid LastProcessStepHistoryItemId = ProcessRepository.GetLastProcessStepHistoryItem(processStepContext.ProcessStep.Entity.Id).Result.Id;
                        ProcessRepository.AddProcessStepHistoryActivity(new ProcessStepHistoryActivity { ActivityId = currentActivity.Id, ActivityName = currentActivity.Name, StepType = 2, ProcessStepHistoryId = LastProcessStepHistoryItemId });
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
            return PreviousHandler?.RollBack(processStepContext) ?? new ProcessResult
            {
                Status= ProcessResultStatus.Failed
            };
        }
    }
}
