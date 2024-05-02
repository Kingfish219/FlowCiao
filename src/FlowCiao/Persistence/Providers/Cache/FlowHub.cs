using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Persistence.Providers.Cache
{
    internal sealed class FlowHub
    {
        internal List<Flow> Flows { get; set; }
        internal List<FlowInstance> FlowExecutions { get; set; }
        internal List<State> States { get; set; }
        internal List<Activity> Activities { get; set; }
        internal List<Transition> Transitions { get; set; }
        internal List<Trigger> Triggers { get; set; }

        internal void Initiate(List<Flow> flows,
            List<FlowInstance> flowExecutions,
            List<State> states,
            List<Transition> transitions,
            List<Activity> activities,
            List<Trigger> triggers)
        {
            Flows = flows;
            FlowExecutions = flowExecutions;
            States = states;
            Transitions = transitions;
            Activities = activities;
            Triggers = triggers;
        }

        internal async Task DeleteFlow(Flow flow)
        {
            await Task.Run(() =>
            {
                Flows.RemoveAll(x => x.Id.Equals(flow.Id));
            });
        }

        internal async Task ModifyFlow(Flow flow)
        {
            await Task.Run(() =>
            {
                if (Flows.Exists(x => x.Id.Equals(flow.Id)))
                {
                    return;
                }

                Flows.Add(flow);
            });
        }

        internal async Task DeleteFlowExecution(FlowInstance flowInstance)
        {
            await Task.Run(() =>
            {
                FlowExecutions.RemoveAll(x => x.Id.Equals(flowInstance.Id));
            });
        }

        internal async Task ModifyFlowInstance(FlowInstance flowInstance)
        {
            await Task.Run(() =>
            {
                if (FlowExecutions.Exists(x => x.Id.Equals(flowInstance.Id)))
                {
                    return;
                }

                FlowExecutions.Add(flowInstance);
            });
        }

        internal async Task ModifyState(State state)
        {
            await Task.Run(() =>
            {
                if (States.Exists(x => x.Id.Equals(state.Id)))
                {
                    return;
                }

                States.Add(state);
            });
        }

        internal async Task DeleteState(State state)
        {
            await Task.Run(() =>
            {
                States.RemoveAll(x => x.Id.Equals(state.Id));
            });
        }

        internal async Task ModifyTransition(Transition transition)
        {
            await Task.Run(() =>
            {
                if (Transitions.Exists(x => x.Id.Equals(transition.Id)))
                {
                    return;
                }

                Transitions.Add(transition);
            });
        }

        internal async Task DeleteTransition(Transition transition)
        {
            await Task.Run(() =>
            {
                Transitions.RemoveAll(x => x.Id.Equals(transition.Id));
            });
        }

        internal async Task ModifyActivity(Activity activity)
        {
            await Task.Run(() =>
            {
                if (Activities.Exists(x => x.Id.Equals(activity.Id)))
                {
                    return;
                }

                Activities.Add(activity);
            });
        }

        internal async Task DeleteActivity(Activity activity)
        {
            await Task.Run(() =>
            {
                Activities.RemoveAll(x => x.Id.Equals(activity.Id));
            });
        }

        internal async Task ModifyTrigger(Trigger trigger)
        {
            await Task.Run(() =>
            {
                if (Triggers.Exists(x => x.Id.Equals(trigger.Id)))
                {
                    return;
                }

                Triggers.Add(trigger);
            });
        }

        internal async Task DeleteTrigger(Trigger trigger)
        {
            await Task.Run(() =>
            {
                Triggers.RemoveAll(x => x.Id.Equals(trigger.Id));
            });
        }
    }
}
