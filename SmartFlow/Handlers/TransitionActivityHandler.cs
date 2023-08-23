using System;
using System.Linq;
using SmartFlow.Exceptions;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;
using SmartFlow.Services;

namespace SmartFlow.Handlers
{
    internal class TransitionActivityHandler : WorkflowHandler
    {
        public TransitionActivityHandler(IProcessRepository processRepository, IProcessService processService) : base(processRepository, processService)
        {
        }

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                var activities = processStepContext.ProcessExecutionStepDetail.Transition.Activities;
                if (activities.Count == 0)
                {
                    return NextHandler.Handle(processStepContext);
                }

                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(type => type.GetTypes())
                    .Where(p => typeof(Activity).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract && p.BaseType == typeof(Activity));

                foreach (var type in types)
                {
                    var activity = (IProcessActivity)Activator.CreateInstance(type, processStepContext);
                    if (activity is null ||
                        !activities.Exists(x => x.ActivityTypeCode == ((Activity)activity).ActivityTypeCode))
                    {
                        continue;
                    }

                    var result = activity.Execute();
                    if (result.Status != ProcessResultStatus.Completed && result.Status != ProcessResultStatus.SetOwner)
                    {
                        throw new SmartFlowProcessExecutionException("Exception occured while invoking activities" + result.Message);
                    }

                    //var currentActivity = activities.Find(a => a.ActivityTypeCode == ((Activity)activity).ActivityTypeCode);
                    //Guid LastProcessStepHistoryItemId = ProcessRepository.GetLastProcessStepHistoryItem(processStepContext.ProcessStepDetail.Entity.Id).Result.Id;
                    //ProcessRepository.AddProcessStepHistoryActivity(new ProcessStepHistoryActivity { ActivityId = currentActivity.Id, ActivityName = currentActivity.Name, StepType = 2, ProcessStepHistoryId = LastProcessStepHistoryItemId });
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
                Status = ProcessResultStatus.Failed
            };
        }
    }
}
