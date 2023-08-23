using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Models;
using SmartFlow.Models.Flow;

namespace SmartFlow.Persistence.Providers.Cache
{
    public class SmartFlowHub
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

        public async Task InsertProcess(Process process)
        {
            await Task.Run(() =>
            {
                if (Processes.Exists(x => x.Id.Equals(process.Id)))
                {
                    throw new SmartFlowPersistencyException("");
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

        public async Task InsertProcessExecution(ProcessExecution processExecution)
        {
            await Task.Run(() =>
            {
                if (ProcessExecutions.Exists(x => x.Id.Equals(processExecution.Id)))
                {
                    throw new SmartFlowPersistencyException("");
                }

                ProcessExecutions.Add(processExecution);
            });
        }

        public async Task InsertState(State state)
        {
            await Task.Run(() =>
            {
                if (States.Exists(x => x.Id.Equals(state.Id)))
                {
                    throw new SmartFlowPersistencyException("");
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

        public async Task InsertTransition(Transition transition)
        {
            await Task.Run(() =>
            {
                if (Transitions.Exists(x => x.Id.Equals(transition.Id)))
                {
                    throw new SmartFlowPersistencyException("");
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

        public async Task InsertActivity(Activity activity)
        {
            await Task.Run(() =>
            {
                if (Activities.Exists(x => x.Id.Equals(activity.Id)))
                {
                    throw new SmartFlowPersistencyException("");
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

        public async Task InsertAction(ProcessAction action)
        {
            await Task.Run(() =>
            {
                if (Actions.Exists(x => x.Id.Equals(action.Id)))
                {
                    throw new SmartFlowPersistencyException("");
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
