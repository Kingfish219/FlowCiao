using System;
using System.Collections.Generic;
using System.Linq;
using SmartFlow.Interfaces;
using SmartFlow.Models.Flow;

namespace SmartFlow.Builders
{
    internal class SmartFlowStepBuilder : ISmartFlowStepBuilder
    {
        public ISmartFlowBuilder SmartFlowBuilder { get; set; }

        public SmartFlowStepBuilder(ISmartFlowBuilder smartFlowBuilder)
        {
            SmartFlowBuilder = smartFlowBuilder;
            AllowedTransitionsBuilders = new List<Action<Transition>>();
        }

        public State InitialState { get; set; }
        public List<Action<Transition>> AllowedTransitionsBuilders { get; set; }
        public IProcessActivity OnEntryActivty { get; set; }
        public IProcessActivity OnExitActivity { get; set; }

        public ISmartFlowStepBuilder From(State state)
        {
            InitialState = state;

            return this;
        }

        public ISmartFlowStepBuilder Allow(State state, List<int> actions)
        {
            AllowedTransitionsBuilders.Add((transition) =>
            {
                transition.From = InitialState;
                transition.To = state;
                transition.Activities = OnExitActivity != null ? new List<Activity>
                            {
                                new()
                                {
                                    ProcessActivityExecutor = OnExitActivity
                                }
                            } : new List<Activity>();
                transition.Actions = actions.Select(action => new ProcessAction(action)).ToList();
            });

            return this;
        }

        public ISmartFlowStepBuilder Allow(State state, int action, Func<bool> condition = null)
        {
            AllowedTransitionsBuilders.Add((transition) =>
            {
                transition.From = InitialState;
                transition.To = state;
                transition.Activities = OnExitActivity != null ? new List<Activity>
                            {
                                new()
                                {
                                    ProcessActivityExecutor = OnExitActivity
                                }
                            } : new List<Activity>();
                transition.Actions = new List<ProcessAction>
                                {
                                    new ProcessAction(action)
                                };
                transition.Condition = condition;
            });
            
            return this;
        }

        public ISmartFlowStepBuilder AllowSelf(List<int> actions)
        {
            throw new NotImplementedException();
        }

        public ISmartFlowStepBuilder OnEntry<TA>() where TA : IProcessActivity, new()
        {
            OnEntryActivty = (TA)Activator.CreateInstance(typeof(TA));
            var activity = new Activity
            {
                ProcessActivityExecutor = OnEntryActivty
            };
            InitialState.Activities ??= new List<Activity>();
            InitialState.Activities.Add(activity);

            return this;
        }

        public ISmartFlowStepBuilder OnExit<TA>() where TA : IProcessActivity, new()
        {
            OnExitActivity = (TA)Activator.CreateInstance(typeof(TA));
            //var activity = new Activity
            //{
            //    ProcessActivityExecutor = OnExitActivity
            //};
            //InitialState.Activities ??= new List<Activity>();
            //InitialState.Activities.Add(activity);

            return this;
        }

        public ISmartFlowStepBuilder AssignToUser(Func<string> userId)
        {
            InitialState.OwnerId = userId();

            return this;
        }

        public void Rollback()
        {
            // ignored
        }
    }
}
