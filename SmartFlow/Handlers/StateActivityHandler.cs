using System;
using System.Linq;
using SmartFlow.Exceptions;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Persistence.Interfaces;
using SmartFlow.Services;

namespace SmartFlow.Handlers
{
    internal class StateActivityHandler : WorkflowHandler
    {
        public StateActivityHandler(IProcessRepository processRepository, IProcessService processService) : base(processRepository, processService)
        {
        }

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                var activities = processStepContext.ProcessExecution.State.Activities;
                if (activities is null || activities.Count == 0)
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
