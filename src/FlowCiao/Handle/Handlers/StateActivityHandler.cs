using System;
using System.Linq;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Handle.Handlers
{
    internal class StateActivityHandler : FlowHandler
    {
        public StateActivityHandler(IFlowRepository flowRepository, IFlowService flowService) : base(flowRepository, flowService)
        {
        }

        public override FlowResult Handle(FlowStepContext flowStepContext)
        {
            try
            {
                var activities = flowStepContext.FlowInstance.State.Activities;
                if (activities is null || activities.Count == 0)
                {
                    return NextHandler?.Handle(flowStepContext);
                }

                var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(type => type.GetTypes())
                        .Where(p => typeof(IFlowActivity).IsAssignableFrom(p) && !p.IsAbstract);

                foreach (var type in types)
                {
                    if (!activities.Exists(x => x.ActorName.Equals(type.FullName, StringComparison.InvariantCulture)))
                    {
                        continue;
                    }
                    
                    var activity = (IFlowActivity)Activator.CreateInstance(type);
                    if (activity is null)
                    {
                        throw new FlowCiaoExecutionException($"Activity with type: {type.FullName} not found");
                    }
                    
                    activity.Execute(flowStepContext);
                }

                return NextHandler?.Handle(flowStepContext);
            }
            catch (Exception exception)
            {
                return new FlowResult(FlowResultStatus.Failed, message: exception.Message);
            }
        }

        public override FlowResult RollBack(FlowStepContext flowStepContext)
        {
            return PreviousHandler?.RollBack(flowStepContext) ?? new FlowResult(FlowResultStatus.Failed);
        }
    }
}
