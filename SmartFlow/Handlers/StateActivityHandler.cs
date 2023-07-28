using System;
using System.Linq;
using SmartFlow.Exceptions;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Handlers
{
    internal class StateActivityHandler : WorkflowHandler
    {
        public StateActivityHandler(IProcessRepository processRepository, IProcessService processService) : base(processRepository, processService)
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

                //var stateCurrent = new State
                //{
                //    Id = processStepContext.ProcessStepDetail.Transition.Actions.FirstOrDefault().Transition.CurrentStateId
                //};

                var activities = ProcessRepository.GetStateActivities(processStepContext.ProcessExecution.State, new Group()).Result;
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
                        //var currentActivity = activities.Find(a => a.ActivityTypeCode == ((Activity)activity).ActivityTypeCode);
                        //Guid LastProcessStepHistoryItemId = ProcessRepository.GetLastProcessStepHistoryItem(processStepContext.ProcessStepDetail.Entity.Id).Result.Id;
                        //ProcessRepository.AddProcessStepHistoryActivity(new ProcessStepHistoryActivity { ActivityId = currentActivity.Id, ActivityName = currentActivity.Name, StepType = 1, ProcessStepHistoryId = LastProcessStepHistoryItemId });
                    }
                }

                return NextHandler?.Handle(processStepContext) ?? result;
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
                //ProcessRepository.RemoveEntireFlow(processStepContext.ProcessStepDetail).GetAwaiter().GetResult();

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
