using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Builder;
using FlowCiao.Models.Core;
using FlowCiao.Services.Interfaces;
using FlowCiao.Utils;

namespace FlowCiao.Builder
{
    internal class FlowStepBuilder : IFlowStepBuilder
    {
        private FlowStep FlowStep { get; }

        private readonly IStateService _stateService;
        private readonly ITransitionService _transitionService;

        public FlowStepBuilder(IStateService stateService, ITransitionService transitionService)
        {
            _stateService = stateService;
            _transitionService = transitionService;
            FlowStep = new FlowStep();
        }

        public void AsInitialStep()
        {
            FlowStep.For.IsInitial = true;
        }

        public IFlowStepBuilder For(State state)
        {
            FlowStep.For = state;

            return this;
        }

        public IFlowStepBuilder Allow(State state, int trigger, Func<bool> condition = null)
        {
            return Allow(state, new Trigger(trigger), condition);
        }

        public IFlowStepBuilder Allow(State state, Trigger trigger, Func<bool> condition = null)
        {
            FlowStep.Allowed.Add(new Transition
            {
                From = FlowStep.For,
                To = state,
                Activities = FlowStep.OnExit != null
                    ? new List<Activity>
                    {
                        new(FlowStep.OnExit)
                    }
                    : new List<Activity>(),
                Triggers = new List<Trigger>
                {
                    trigger
                },
                Condition = condition
            });

            return this;
        }

        public IFlowStepBuilder OnEntry<TA>() where TA : IFlowActivity, new()
        {
            FlowStep.OnEntry = (TA)Activator.CreateInstance(typeof(TA));
            var activity = new Activity(FlowStep.OnEntry);
            FlowStep.For.Activities ??= new List<Activity>();
            FlowStep.For.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder OnExit<TA>() where TA : IFlowActivity, new()
        {
            FlowStep.OnExit = (TA)Activator.CreateInstance(typeof(TA));
            if (FlowStep.Allowed.IsNullOrEmpty())
            {
                throw new FlowCiaoException("No allowed transitions found in order to apply OnExit");
            }

            FlowStep.Allowed.ForEach(t =>
            {
                t.Activities ??= new List<Activity>();
                t.Activities.Add(new Activity(FlowStep.OnExit));
            });

            return this;
        }

        public FlowStep Build(Guid flowId)
        {
            // var result = Persist(flowId).GetAwaiter().GetResult();
            // if (!result.Success)
            // {
            //     throw new FlowCiaoPersistencyException("Saving some data failed");
            // }

            return FlowStep;
        }

        public void Rollback()
        {
            // ignored
        }

        public async Task<FuncResult> Persist(Flow flow)
        {
            FlowStep.For.Flow = flow;
            FlowStep.For.FlowId = flow.Id;
            var forResult = await _stateService.Modify(FlowStep.For);
            if (forResult == default)
            {
                return new FuncResult(false, "Modifying State failed");
            }

            if (FlowStep.Allowed.IsNullOrEmpty())
            {
                return new FuncResult(true);
            }

            foreach (var transition in FlowStep.Allowed)
            {
                transition.From.FlowId = flow.Id;
                transition.From.Flow = flow;

                var stateResult = await _stateService.Modify(transition.From);
                if (!stateResult.Success || stateResult.Data == default)
                {
                    return new FuncResult(false, stateResult.Message ?? "Modifying State failed");
                }
                
                transition.FromId = stateResult.Data;
                transition.To.FlowId = flow.Id;
                transition.To.Flow = flow;

                stateResult = await _stateService.Modify(transition.To);
                if (!stateResult.Success || stateResult.Data == default)
                {
                    return new FuncResult(false, stateResult.Message ?? "Modifying State failed");
                }

                transition.ToId = stateResult.Data;
                transition.FlowId = flow.Id;
                transition.Flow = flow;
                var transitionResult = await _transitionService.Modify(transition);
                if (!transitionResult.Success || transitionResult.Data == default)
                {
                    return new FuncResult(false, transitionResult.Message ?? "Modifying Transition failed");
                }
            }

            return new FuncResult(true);
        }
    }
}