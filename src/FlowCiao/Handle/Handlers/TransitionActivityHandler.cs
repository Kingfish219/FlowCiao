using System;
using System.Linq;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handle.Handlers
{
    internal class TransitionActivityHandler : FlowHandler
    {
        public TransitionActivityHandler(IFlowRepository flowRepository, IFlowService flowService) : base(flowRepository, flowService)
        {
        }

        public override FlowResult Handle(FlowStepContext flowStepContext)
        {
            try
            {
                var activities = flowStepContext.FlowExecutionStepDetail.Transition.Activities;
                if (activities is null || activities.Count == 0)
                {
                    return NextHandler.Handle(flowStepContext);
                }

                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(type => type.GetTypes())
                    .Where(p => typeof(IFlowActivity).IsAssignableFrom(p) && !p.IsAbstract);

                foreach (var type in types)
                {
                    var activity = (IFlowActivity)Activator.CreateInstance(type);
                    if (activity is null ||
                            !activities.Exists(x => x.Actor.GetType().Equals(activity.GetType())))
                    {
                        continue;
                    }

                    var result = activity.Execute(flowStepContext);
                    if (result.Status != FlowResultStatus.Completed && result.Status != FlowResultStatus.SetOwner)
                    {
                        throw new FlowExecutionException("Exception occured while invoking activities" + result.Message);
                    }
                }

                return NextHandler.Handle(flowStepContext);
            }
            catch (Exception exception)
            {
                return new FlowResult
                {
                    Status = FlowResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }

        public override FlowResult RollBack(FlowStepContext flowStepContext)
        {
            return PreviousHandler?.RollBack(flowStepContext) ?? new FlowResult
            {
                Status = FlowResultStatus.Failed
            };
        }
    }
}
