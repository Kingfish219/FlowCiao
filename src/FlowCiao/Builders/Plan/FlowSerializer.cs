using System;
using System.Collections.Generic;
using System.Linq;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;
using FlowCiao.Services;
using FlowCiao.Utils;
using Newtonsoft.Json;

namespace FlowCiao.Builders.Plan;

public class FlowSerializer
{
    private readonly IFlowBuilder _builder;
    private readonly ActivityService _activityService;

    public FlowSerializer(IFlowBuilder builder, ActivityService activityService)
    {
        _builder = builder;
        _activityService = activityService;
    }
    
    public IFlowPlanner ImportJson(string json)
    {
        var jsonFlow = JsonConvert.DeserializeObject<SerializedFlow>(json);

        return CreateFlowPlanner(jsonFlow);
    }

    private IFlowPlanner CreateFlowPlanner(SerializedFlow serializedFlow)
    {
        var planner = new DefaultFlowPlanner(serializedFlow.Key, () =>
        {
            var states = serializedFlow.States
                .Select(state => new State(state.Code, state.Name))
                .ToList();
            
            _builder.Initial(CreateStepBuilder(serializedFlow.Initial, states));

            if (serializedFlow.Steps.IsNullOrEmpty())
            {
                return _builder;
            }
            
            serializedFlow.Steps.ForEach(step =>
            {
                _builder.NewStep(CreateStepBuilder(step, states));
            });
            
            return _builder;
        });

        return planner;
    }

    private Action<IFlowStepBuilder> CreateStepBuilder(SerializedStep serializedStep, IReadOnlyCollection<State> states)
    {
        return stepBuilder =>
        {
            var fromState = states.Single(state => state.Code == serializedStep.FromStateCode);
            var allowedList = states
                .Where(state => serializedStep.Allows.Exists(allowed => allowed.AllowedStateCode == state.Code))
                .Select(state => new
                {
                    AllowedState = state,
                    AllowedTrigger = serializedStep.Allows.Single(x => x.AllowedStateCode == state.Code).TriggerCode
                })
                .ToList();
            
            stepBuilder.For(fromState);
            if (!allowedList.IsNullOrEmpty())
            {
                allowedList.ForEach(allowed =>
                {
                    stepBuilder.Allow(allowed.AllowedState, allowed.AllowedTrigger);
                });
            }
            
            // if (!string.IsNullOrWhiteSpace(serializedStep.OnEntry.Name))
            // {
            //     var flowActivity = TryGetActivity(serializedStep.OnEntry.Name);
            //     var metSum = typeof(IFlowStepBuilder).GetMethod(nameof(IFlowStepBuilder.OnEntry));
            //     var genMetSum = metSum.MakeGenericMethod(flowActivity.GetType());
            //     genMetSum.Invoke(serializedStep, null);
            // }
        };
    }
    
    // private IFlowActivity CreateActivity(string activityName)
    // {
    //     var flowActivity = TryGetActivity(activityName);
    //     var metSum = typeof(IFlowStepBuilder).GetMethod(nameof(IFlowStepBuilder.OnEntry));
    //     var genMetSum = metSum.MakeGenericMethod(flowActivity.GetType());
    //     var result = (int)genMetSum.Invoke(null, new object[] { 1, 2 });
    //
    //     Console.WriteLine(result); // Output: 3
    // }
    
    private IFlowActivity TryGetActivity(string activityName)
    {
        try
        {
            IFlowActivity flowActivity;

            var activityType = GeneralUtils.FindType(activityName, typeof(IFlowActivity));
            if (activityType != null)
            {
                flowActivity = (IFlowActivity)Activator.CreateInstance(activityType);
            }
            else
            {
                var storedActivity = _activityService.LoadActivity(activityName).GetAwaiter().GetResult();
                flowActivity = storedActivity;
            }

            return flowActivity;
        }
        catch (Exception)
        {
            return null;
        }
    }
}