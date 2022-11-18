using SmartFlow.Core.Db;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System;
using System.Linq;

namespace SmartFlow.Core.Handlers
{
    internal class TransitionActivityHandler : WorkflowHandler
    {
        private int _actionCode;
        private string _connectionString;
        private IProcessRepository _processRepository;
        private LogRepository _logRepository;

        public TransitionActivityHandler(IProcessRepository processRepository, int actionCode
            , string connectionString, LogRepository logRepository) 
            : base(processRepository)
        {
            _actionCode = actionCode;
            _connectionString = connectionString;
            _processRepository = processRepository;
            _logRepository = logRepository;
        }

        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
                var result = new ProcessResult
                {
                    Status = ProcessResultStatus.Completed
                };

                var currentTransition = processStep.TransitionActions.FirstOrDefault().Transition;
                var activities = ProcessRepository.GetTransitionActivities(currentTransition).Result;

                if (activities.Count > 0)
                {
                    var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(type => type.GetTypes())
                        .Where(p => typeof(Activity).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract && p.BaseType == typeof(Activity));

                    //var context = ProcessStepContext.ReturnInstance();
                    //context.Context.Add("ProcessStep", processStep);
                    //context.Context.Add("Entity", processStep.Entity);
                    //context.Context.Add("User", user);
                    //context.Context.Add("CurrentState", new State
                    //{
                    //    Id = currentTransition.CurrentStateId
                    //});
                    //context.Context.Add("ActionComment", _comment);
                    //context.Context.Add("TicketNumber", _ticketNumber);
                    //context.Context.Add("ProcessRepoInstance", _processRepository);
                    //context.Context.Add("OccuredAction", new Models.Action
                    //{
                    //    ActionTypeCode = Convert.ToInt32(_actionCode)
                    //});
                    //context.Context.Add("ConnectionString", _connectionString);

                    if (processStepContext.Context.ContainsKey("ProcessStep"))
                        processStepContext.Context["ProcessStep"] = processStep;
                    else processStepContext.Context.Add("ProcessStep", processStep);
                    if (processStepContext.Context.ContainsKey("Entity"))
                        processStepContext.Context["Entity"] = processStep.Entity;
                    else processStepContext.Context.Add("Entity", processStep.Entity);
                    if (processStepContext.Context.ContainsKey("User"))
                        processStepContext.Context["User"] = user;
                    else processStepContext.Context.Add("User", user);
                    if (processStepContext.Context.ContainsKey("CurrentState"))
                        processStepContext.Context["CurrentState"] = new State
                        {
                            Id = currentTransition.CurrentStateId
                        };
                    else processStepContext.Context.Add("CurrentState", new State
                    {
                        Id = currentTransition.CurrentStateId
                    });
                    if (processStepContext.Context.ContainsKey("ProcessRepoInstance"))
                        processStepContext.Context["ProcessRepoInstance"] = _processRepository;
                    else processStepContext.Context.Add("ProcessRepoInstance", _processRepository);
                    if (processStepContext.Context.ContainsKey("OccuredAction"))
                        processStepContext.Context["OccuredAction"] = new Models.Action
                        {
                            ActionTypeCode = Convert.ToInt32(_actionCode)
                        };
                    else processStepContext.Context.Add("OccuredAction", new Models.Action
                    {
                        ActionTypeCode = Convert.ToInt32(_actionCode)
                    });
                    if (processStepContext.Context.ContainsKey("ConnectionString"))
                        processStepContext.Context["ConnectionString"] = _connectionString;
                    else processStepContext.Context.Add("ConnectionString", _connectionString);

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
                            throw new SmartFlowProcessException("Exception occured while invoking activities" + result.Message, _logRepository, processStep);
                        }
                        //log to ProcessStepHistoryActivity
                        var currentActivity = activities.Find(a => a.ActivityTypeCode == ((Activity)activity).ActivityTypeCode);
                        Guid LastProcessStepHistoryItemId = _processRepository.GetLastProcessStepHistoryItem(processStep.Entity.Id).Result.Id;
                        _processRepository.AddProcessStepHistoryActivity(new ProcessStepHistoryActivity { ActivityId = currentActivity.Id, ActivityName = currentActivity.Name, StepType = 2, ProcessStepHistoryId = LastProcessStepHistoryItemId });
                    }
                }
                if (NextHandler is null)
                {
                    return result;
                }
                return NextHandler.Handle(processStep, user,processStepContext);
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

        public override ProcessResult RollBack(ProcessStep processStep)
        {
            throw new NotImplementedException();
        }
    }
}
