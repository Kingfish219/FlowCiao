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
                //var stateCurrent = new State
                //{
                //    Id = processStepContext.ProcessStepDetail.Transition.Actions.FirstOrDefault().Transition.CurrentStateId
                //};

                //var activities = ProcessRepository.GetStateActivities(processStepContext.ProcessExecution.State, new Group()).Result;
                var activities = processStepContext.ProcessExecutionStepDetail.Transition.From.Activities;
                if (activities.Count == 0)
                {
                    return NextHandler?.Handle(processStepContext);
                }

                var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(type => type.GetTypes())
                        .Where(p => typeof(IProcessActivity).IsAssignableFrom(p) && !p.IsAbstract);

                foreach (var type in types)
                {
                    var activity = (IProcessActivity)Activator.CreateInstance(type);
                    if (activity is null ||
                            !activities.Exists(x => x.ProcessActivityExecutor.GetType().Equals(activity.GetType())))
                    {
                        continue;
                    }

                    var result = activity.Execute(processStepContext);
                    if (result.Status != ProcessResultStatus.Completed && result.Status != ProcessResultStatus.SetOwner)
                    {
                        throw new SmartFlowProcessExecutionException("Exception occured while invoking activities" + result.Message);
                    }
                }

                return NextHandler?.Handle(processStepContext);
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
