using System.Linq;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Serialized;
using FlowCiao.Models.Core;
using FlowCiao.Services;
using FlowCiao.Utils;

namespace FlowCiao.Builder.Serialization;

public class FlowSerializerHelper
{
    private readonly ActivityService _activityService;

    public FlowSerializerHelper(ActivityService activityService)
    {
        _activityService = activityService;
    }
    
    internal SerializedFlow CreateSerializedFlow(Flow flow)
    {
        var serializedFlow = new SerializedFlow
        {
            Key = flow.Key,
            Name = flow.Name,
            States = flow.States.Select(s => new SerializedState
            {
                Name = s.Name,
                Code = s.Code
            }).ToList(),
            Triggers = flow.Triggers.Select(t => new SerializedTrigger
            {
                Name = t.Name,
                Code = t.Code
            }).ToList(),
        };

        var initialTransitions = flow.Transitions.Where(t => t.From.IsInitial).ToList();
        if (initialTransitions.IsNullOrEmpty())
        {
            throw new FlowCiaoSerializationException("No initial state found");
        }

        serializedFlow.Initial = new SerializedStep
        {
            FromStateCode = initialTransitions.First().From.Code,
            Allows = initialTransitions.Select(t => new SerializedTransition
            {
                AllowedStateCode = t.To.Code,
                TriggerCode = t.Triggers.First().Code
            }).ToList(),
            OnEntry = initialTransitions.First().From.Activities
                .Select(a => new SerializedActivity
                {
                    Name = a.Name,
                    ActorName = a.ActorName
                })
                .FirstOrDefault(),
            OnExit = initialTransitions.First().Activities
                .Select(a => new SerializedActivity
                {
                    Name = a.Name,
                    ActorName = a.ActorName
                })
                .FirstOrDefault()
        };
        
        var otherTransitions = flow.Transitions
            .Where(t => t.From.Code != initialTransitions.First().From.Code)
            .GroupBy(t=>t.From.Code)
            .ToList();
        if (otherTransitions.IsNullOrEmpty())
        {
            return serializedFlow;
        }

        serializedFlow.Steps = otherTransitions.Select(transitionGroup => new SerializedStep
            {
                FromStateCode = transitionGroup.First().From.Code,
                Allows = transitionGroup.Select(t => new SerializedTransition
                {
                    AllowedStateCode = t.To.Code,
                    TriggerCode = t.Triggers.First().Code
                }).ToList(),
                OnEntry = transitionGroup.FirstOrDefault(g=> !g.From.Activities.IsNullOrEmpty())
                    ?.From.Activities
                    .Select(a => new SerializedActivity
                    {
                        Name = a.Name,
                        ActorName = a.ActorName
                    })
                    .FirstOrDefault(),
                OnExit = transitionGroup.FirstOrDefault(g=> !g.Activities.IsNullOrEmpty())
                    ?.Activities
                    .Select(a => new SerializedActivity
                    {
                        Name = a.Name,
                        ActorName = a.ActorName
                    })
                    .FirstOrDefault()
            })
            .ToList();

        return serializedFlow;
    }

    internal IFlowPlanner CreateFlowPlanner(SerializedFlow serializedFlow)
    {
        // var planner = new DefaultFlowPlanner(serializedFlow.Key, () =>
        // {
        //     var states = serializedFlow.States
        //         .Select(state => new State(state.Code, state.Name))
        //         .ToList();
        //
        //     _builder.Initial(CreateStepBuilder(serializedFlow.Initial, states));
        //
        //     if (serializedFlow.Steps.IsNullOrEmpty())
        //     {
        //         return _builder;
        //     }
        //
        //     serializedFlow.Steps.ForEach(step => { _builder.NewStep(CreateStepBuilder(step, states)); });
        //
        //     return _builder;
        // });

        var planner = new SerializerFlowPlanner(serializedFlow, _activityService);

        return planner;
    }

    // private Action<IFlowStepBuilder> CreateStepBuilder(SerializedStep serializedStep, IReadOnlyCollection<State> states)
    // {
    //     return stepBuilder =>
    //     {
    //         var fromState = states.Single(state => state.Code == serializedStep.FromStateCode);
    //         var allowedList = states
    //             .Where(state => serializedStep.Allows.Exists(allowed => allowed.AllowedStateCode == state.Code))
    //             .Select(state => new
    //             {
    //                 AllowedState = state,
    //                 AllowedTrigger = serializedStep.Allows.Single(x => x.AllowedStateCode == state.Code).TriggerCode
    //             })
    //             .ToList();
    //
    //         stepBuilder.For(fromState);
    //         if (!allowedList.IsNullOrEmpty())
    //         {
    //             allowedList.ForEach(allowed => { stepBuilder.Allow(allowed.AllowedState, allowed.AllowedTrigger); });
    //         }
    //
    //         if (!string.IsNullOrWhiteSpace(serializedStep.OnEntry?.ActorName))
    //         {
    //             CreateActivity(stepBuilder, serializedStep.OnEntry);
    //         }
    //
    //         if (!string.IsNullOrWhiteSpace(serializedStep.OnExit?.ActorName))
    //         {
    //             CreateActivity(stepBuilder, serializedStep.OnExit);
    //         }
    //     };
    // }
    //
    // private void CreateActivity(IFlowStepBuilder stepBuilder, SerializedActivity serializedActivity)
    // {
    //     var flowActivity = TryGetActivity(serializedActivity.ActorName);
    //     var methodInfo = typeof(IFlowStepBuilder).GetMethod(nameof(IFlowStepBuilder.OnEntry));
    //     var genericMethod = methodInfo!.MakeGenericMethod(flowActivity.GetType());
    //     genericMethod.Invoke(stepBuilder, null);
    // }
    //
    // private IFlowActivity TryGetActivity(string activityName)
    // {
    //     try
    //     {
    //         IFlowActivity flowActivity;
    //
    //         var activityType = GeneralUtils.FindType(activityName, typeof(IFlowActivity));
    //         if (activityType != null)
    //         {
    //             flowActivity = (IFlowActivity)Activator.CreateInstance(activityType);
    //         }
    //         else
    //         {
    //             var storedActivity = _activityService.LoadActivity(activityName).GetAwaiter().GetResult();
    //             flowActivity = storedActivity;
    //         }
    //
    //         return flowActivity;
    //     }
    //     catch (Exception)
    //     {
    //         return null;
    //     }
    // }
}