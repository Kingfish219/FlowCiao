//using System;
//using System.Collections.Generic;
//using SmartFlow.Core.Models;

//namespace SmartFlow.Core.Builders
//{
//    public class StateMachineStepBuilder : IStateMachineStepBuilder
//    {
//        public IStateMachineBuilder ProcessBuilder { get; set; }

//        public StateMachineStepBuilder(IStateMachineBuilder processBuilder)
//        {
//            ProcessBuilder = processBuilder;
//            AllowedTransitions = new List<(State, List<ProcessAction>)>();
//        }

//        public State InitialState { get; set; }
//        public List<(State, List<ProcessAction>)> AllowedTransitions { get; set; }
//        public IProcessActivity OnEntryActivty { get; set; }
//        public IProcessActivity OnExitActivity { get; set; }

//        public IStateMachineStepBuilder From(State state)
//        {
//            InitialState = state;

//            return this;
//        }

//        public IStateMachineStepBuilder Allow(State state, List<ProcessAction> actions)
//        {
//            AllowedTransitions.Add((state, actions));

//            return this;
//        }

//        public IStateMachineStepBuilder Allow(State state, ProcessAction action)
//        {
//            var actions = new List<ProcessAction>
//            {
//                action
//            };

//            AllowedTransitions.Add((state, actions));

//            return this;
//        }

//        public IStateMachineStepBuilder AllowSelf(List<ProcessAction> actions)
//        {
//            throw new NotImplementedException();
//        }

//        public IStateMachineStepBuilder OnEntry<Activity>() where Activity : IProcessActivity, new()
//        {
//            OnEntryActivty = (Activity)Activator.CreateInstance(typeof(Activity));

//            return this;
//        }

//        public IStateMachineStepBuilder OnExit<Activity>() where Activity : IProcessActivity, new()
//        {
//            OnExitActivity = (Activity)Activator.CreateInstance(typeof(Activity));

//            return this;
//        }

//        public IStateMachineStepBuilder NewStep()
//        {
//            return ProcessBuilder.NewStep();
//        }

//        public IStateMachineStepBuilder AssignToUser(Func<string> userId)
//        {
//            InitialState.OwnerId = userId();

//            return this;
//        }

//        public void Rollback()
//        {

//        }
//    }
//}
