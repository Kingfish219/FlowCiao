using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Repositories;

namespace SmartFlow.Core.Handlers
{
    internal class GoToStateHandler : WorkflowHandler
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IProcessRepository _processRepository;
        private readonly string _actionType;
        private readonly string _comment;
        private readonly string _ticketNumber;
        private readonly string _connectionString;
        private readonly Guid _targetStatus;
        private readonly LogRepository _logRepository;

        internal GoToStateHandler(IProcessRepository processRepository, IEntityRepository entityRepository,string actionType, string comment, string ticketNumber, string connectionString,Guid targetStatus, LogRepository logRepository) : base(processRepository)
        {
            _processRepository = processRepository;
            _entityRepository = entityRepository;
            _comment = comment;
            _ticketNumber = ticketNumber;
            _connectionString = connectionString;
            _targetStatus = targetStatus;
            _actionType = actionType;
            _logRepository = logRepository;
        }

        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
                if (_processRepository.GetState(_targetStatus).Result.Id.Equals(Guid.Empty))
                {
                    throw new SmartFlowProcessException("Exception occured while status id is not defined", _logRepository, processStep);
                }

                var result = _entityRepository.ChangeState(processStep.Entity, _targetStatus);
                if (result.Status != ProcessResultStatus.Completed)
                {
                    throw new SmartFlowProcessException("Exception occured while changing entity state", _logRepository, processStep);
                }

                Guid pastStatus = processStep.Entity.LastStatus;
                processStep.Entity.LastStatus = _targetStatus;
                
                #region transition activity

                    if (pastStatus == new Guid("A4E6D31F-ECD1-4288-863F-D4D5449C19D3")
                        || pastStatus == new Guid("EB1566A1-35FD-4C8F-AC27-DB7B02D68299"))
                    {
                        var stateCurrent = new State
                        {
                            Id = pastStatus
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
                        if (processStepContext.Context.ContainsKey("ActionComment"))
                            processStepContext.Context["ActionComment"] = _comment;
                        else processStepContext.Context.Add("ActionComment", _comment);
                        if (processStepContext.Context.ContainsKey("TicketNumber"))
                            processStepContext.Context["TicketNumber"] = _ticketNumber;
                        else processStepContext.Context.Add("TicketNumber", _ticketNumber);
                        if (processStepContext.Context.ContainsKey("ProcessRepoInstance"))
                            processStepContext.Context["ProcessRepoInstance"] = _processRepository;
                        else processStepContext.Context.Add("ProcessRepoInstance", _processRepository);
                         if (processStepContext.Context.ContainsKey("OccuredAction"))
                            processStepContext.Context["OccuredAction"] = new Models.Action
                            {
                                ActionTypeCode = Convert.ToInt32(_actionType)
                            };
                        else processStepContext.Context.Add("OccuredAction", new Models.Action
                        {
                            ActionTypeCode = Convert.ToInt32(_actionType)
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
