using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Persistence.Providers.Cache
{
    public class FlowHub
    {
        public List<Flow> Flows { get; set; }
        public List<FlowExecution> FlowExecutions { get; set; }
        public List<State> States { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Transition> Transitions { get; set; }
        public List<Trigger> Triggers { get; set; }

        public void Initiate(List<Flow> flows,
            List<FlowExecution> flowExecutions,
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

        public async Task DeleteFlow(Flow flow)
        {
            await Task.Run(() =>
            {
                Flows.RemoveAll(x => x.Id.Equals(flow.Id));
            });
        }

        public async Task ModifyFlow(Flow flow)
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

        public async Task DeleteFlowExecution(FlowExecution flowExecution)
        {
            await Task.Run(() =>
            {
                FlowExecutions.RemoveAll(x => x.Id.Equals(flowExecution.Id));
            });
        }

        public async Task ModifyFlowExecution(FlowExecution flowExecution)
        {
            await Task.Run(() =>
            {
                if (FlowExecutions.Exists(x => x.Id.Equals(flowExecution.Id)))
                {
                    return;
                }

                FlowExecutions.Add(flowExecution);
            });
        }

        public async Task ModifyState(State state)
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

        public async Task DeleteState(State state)
        {
            await Task.Run(() =>
            {
                States.RemoveAll(x => x.Id.Equals(state.Id));
            });
        }

        public async Task ModifyTransition(Transition transition)
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

        public async Task DeleteTransition(Transition transition)
        {
            await Task.Run(() =>
            {
                Transitions.RemoveAll(x => x.Id.Equals(transition.Id));
            });
        }

        public async Task ModifyActivity(Activity activity)
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

        public async Task DeleteActivity(Activity activity)
        {
            await Task.Run(() =>
            {
                Activities.RemoveAll(x => x.Id.Equals(activity.Id));
            });
        }

        public async Task ModifyTrigger(Trigger trigger)
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

        public async Task DeleteTrigger(Trigger trigger)
        {
            await Task.Run(() =>
            {
                Triggers.RemoveAll(x => x.Id.Equals(trigger.Id));
            });
        }
    }
}
