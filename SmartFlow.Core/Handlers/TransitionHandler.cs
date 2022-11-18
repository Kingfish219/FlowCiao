
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Repositories;

namespace SmartFlow.Core.Handlers
{
    internal class TransitionHandler : WorkflowHandler
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IProcessRepository _processRepository;
        private readonly int _actionCode;
        private readonly string _connectionString;
        private readonly LogRepository _logRepository;
        
        internal TransitionHandler(IProcessRepository processRepository, IEntityRepository entityRepository, int actionCode, string connectionString, LogRepository logRepository) : base(processRepository)
        {
            _processRepository = processRepository;
            _entityRepository = entityRepository;
            _actionCode = actionCode;
            _connectionString = connectionString;
            _logRepository = logRepository;
        }

        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
                var transition = processStep.TransitionActions.FirstOrDefault(x => x.Action.ActionTypeCode == _actionCode).Transition;
                if (transition is null)
                {
                    throw new SmartFlowProcessException("Exception occured while completing progress transition", _logRepository, processStep);
                }

                if (!processStep.IsCompleted)
                {
                    throw new SmartFlowProcessException("Exception occured while completing progress transition, process step action is not yet completed", _logRepository, processStep);
                }
                var result = _entityRepository.ChangeState(processStep.Entity, transition.NextStateId);
                if (result.Status != ProcessResultStatus.Completed)
                {
                    throw new SmartFlowProcessException("Exception occured while changing entity state", _logRepository, processStep);
                }

                processStep.Entity.LastStatus = transition.NextStateId;

                #region transition activity

                if (transition.CurrentStateId == new Guid("A4E6D31F-ECD1-4288-863F-D4D5449C19D3")
                    || transition.CurrentStateId == new Guid("EB1566A1-35FD-4C8F-AC27-DB7B02D68299"))
                {
                    var stateCurrent = new State
                    {
                        Id = transition.CurrentStateId
                    };

                    var activities = ProcessRepository.GetStateActivities(stateCurrent, new Group()).Result;
                    if (activities.Count > 0)
                    {
                        var types = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(type => type.GetTypes())
                            .Where(p => typeof(Activity).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract && p.BaseType == typeof(Activity));

                        //var context = ProcessStepContext.ReturnInstance();
                        //context.Context.Add("ProcessStep", processStep);
                        //context.Context.Add("Entity", processStep.Entity);
                        //context.Context.Add("User", user);
                        //context.Context.Add("CurrentState", stateCurrent);
                        //context.Context.Add("ActionComment", _comment);
                        //context.Context.Add("TicketNumber", _ticketNumber);
                        //context.Context.Add("ProcessRepoInstance", _processRepository);
                        //context.Context.Add("OccuredAction", new Models.Action
                        //{
                        //    ActionTypeCode = Convert.ToInt32(_actionType)
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
                            processStepContext.Context["CurrentState"] = stateCurrent;
                        else processStepContext.Context.Add("CurrentState", stateCurrent);
                        if (processStepContext.Context.ContainsKey("ProcessRepoInstance"))
                            processStepContext.Context["ProcessRepoInstance"] = _processRepository;
                        else processStepContext.Context.Add("ProcessRepoInstance", _processRepository);
                        if (processStepContext.Context.ContainsKey("OccuredAction"))
                            processStepContext.Context["OccuredAction"] = new Models.Action
                            {
                                ActionTypeCode = _actionCode
                            };
                        else processStepContext.Context.Add("OccuredAction", new Models.Action
                        {
                            ActionTypeCode = _actionCode
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
                        }
                    }
                }

                #endregion

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
