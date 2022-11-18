using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Repositories;

namespace SmartFlow.Core.Handlers
{
    internal class GoToStateActivityHandler : WorkflowHandler
    {
        private string _comment;
        private string _connectionString;
        private IProcessRepository _ProcessRepository;
        private LogRepository _LogRepository;
        internal GoToStateActivityHandler(IProcessRepository processRepository , string comment, string connectionString, LogRepository logRepository) : base(processRepository)
        {
            _comment = comment;
            _ProcessRepository = processRepository;
            _connectionString = connectionString;
            _LogRepository = logRepository;
        }

        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
                var result = new ProcessResult
                {
                    Status = ProcessResultStatus.Completed
                };

                var stateCurrent = new State
                {
                    Id = processStep.TransitionActions.FirstOrDefault().Transition.CurrentStateId
                };

                var activities = ProcessRepository.GetStateActivities(stateCurrent, new Group()).Result;
                if (activities.Count > 0)
                {
                    var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(type => type.GetTypes())
                        .Where(p => typeof(Activity).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract && p.BaseType == typeof(Activity));

                    //  var context = ProcessStepContext.ReturnInstance();                  
                    //context.Context.Add("ProcessStep", processStep);
                    //context.Context.Add("Entity", processStep.Entity);
                    //context.Context.Add("User", user);
                    //context.Context.Add("ActionComment", _comment);
                    //context.Context.Add("CurrentState", stateCurrent);
                    //context.Context.Add("ConnectionString", _connectionString);
                    //context.Context.Add("ProcessRepoInstance", _ProcessRepository);

                    if (processStepContext.Context.ContainsKey("ProcessStep"))
                        processStepContext.Context["ProcessStep"] = processStep;
                    else processStepContext.Context.Add("ProcessStep", processStep);
                    if (processStepContext.Context.ContainsKey("Entity"))
                        processStepContext.Context["Entity"] = processStep.Entity;
                    else processStepContext.Context.Add("Entity", processStep.Entity);
                    if (processStepContext.Context.ContainsKey("User"))
                        processStepContext.Context["User"] = user;
                    else processStepContext.Context.Add("User", user);
                    if (processStepContext.Context.ContainsKey("ActionComment"))
                        processStepContext.Context["ActionComment"] = _comment;
                    else processStepContext.Context.Add("ActionComment", _comment);
                    if (processStepContext.Context.ContainsKey("CurrentState"))
                        processStepContext.Context["CurrentState"] = stateCurrent;
                    else processStepContext.Context.Add("CurrentState", stateCurrent);
                    if (processStepContext.Context.ContainsKey("ConnectionString"))
                        processStepContext.Context["ConnectionString"] = _connectionString;
                    else processStepContext.Context.Add("ConnectionString", _connectionString);
                    if (processStepContext.Context.ContainsKey("ProcessRepoInstance"))
                        processStepContext.Context["ProcessRepoInstance"] = _ProcessRepository;
                    else processStepContext.Context.Add("ProcessRepoInstance", _ProcessRepository);

                    foreach (var type in types)
                    {
                        var activity = (IProcessActivity)Activator.CreateInstance(type, processStepContext);
                        if (!activities.Exists(x => x.ActivityTypeCode == ((Activity)activity).ActivityTypeCode))
                        {
                            continue;
                        }

                        if (((Activity)activity).ActivityTypeCode == 2)
                        {
                            continue;
                        }

                        result = activity.Invoke();
                        if (result.Status != ProcessResultStatus.Completed && result.Status != ProcessResultStatus.SetOwner)
                        {
                            throw new SmartFlowProcessException("Exception occured while invoking activities" + result.Message, _LogRepository, processStep);
                        }
                        //log to ProcessStepHistoryActivity
                        var currentActivity = activities.Find(a => a.ActivityTypeCode == ((Activity)activity).ActivityTypeCode);
                        Guid LastProcessStepHistoryItemId = _ProcessRepository.GetLastProcessStepHistoryItem(processStep.Entity.Id).Result.Id;
                        _ProcessRepository.AddProcessStepHistoryActivity(new ProcessStepHistoryActivity { ActivityId = currentActivity.Id, ActivityName = currentActivity.Name, StepType = 1, ProcessStepHistoryId = LastProcessStepHistoryItemId });
                    }
                }

                if (NextHandler is null)
                {
                    return result;
                }

                return NextHandler.Handle(processStep, user, processStepContext);
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
