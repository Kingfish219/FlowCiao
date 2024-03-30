using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Persistence.Providers.Cache
{
    public class FlowHub
    {
        public List<Process> Processes { get; set; }
        public List<ProcessExecution> ProcessExecutions { get; set; }
        public List<State> States { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Transition> Transitions { get; set; }
        public List<ProcessAction> Actions { get; set; }

        public void Initiate(List<Process> processes,
            List<ProcessExecution> processExecutions,
            List<State> states,
            List<Transition> transitions,
            List<Activity> activities,
            List<ProcessAction> actions)
        {
            Processes = processes;
            ProcessExecutions = processExecutions;
            States = states;
            Transitions = transitions;
            Activities = activities;
            Actions = actions;
        }

        public async Task DeleteProcess(Process process)
        {
            await Task.Run(() =>
            {
                Processes.RemoveAll(x => x.Id.Equals(process.Id));
            });
        }

        public async Task ModifyProcess(Process process)
        {
            await Task.Run(() =>
            {
                if (Processes.Exists(x => x.Id.Equals(process.Id)))
                {
                    return;
                }

                Processes.Add(process);
            });
        }

        public async Task DeleteProcessExecution(ProcessExecution processExecution)
        {
            await Task.Run(() =>
            {
                ProcessExecutions.RemoveAll(x => x.Id.Equals(processExecution.Id));
            });
        }

        public async Task ModifyProcessExecution(ProcessExecution processExecution)
        {
            await Task.Run(() =>
            {
                if (ProcessExecutions.Exists(x => x.Id.Equals(processExecution.Id)))
                {
                    return;
                }

                ProcessExecutions.Add(processExecution);
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

        public async Task ModfiyActivity(Activity activity)
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

        public async Task ModifyAction(ProcessAction action)
        {
            await Task.Run(() =>
            {
                if (Actions.Exists(x => x.Id.Equals(action.Id)))
                {
                    return;
                }

                Actions.Add(action);
            });
        }

        public async Task DeleteAction(ProcessAction action)
        {
            await Task.Run(() =>
            {
                Actions.RemoveAll(x => x.Id.Equals(action.Id));
            });
        }
    }
}
