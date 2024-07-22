using FlowCiao.Models.Builder.Serialized;
using FlowCiao.Models.Core;

namespace FlowCiao.UnitTests.TestUtils.Flows;

public static class Sample
{
    public static Flow Flow { get; } = new Flow
    {
        Key = "flowKey",
        Name = "flowName",
        IsActive = true,
        CreatedAt = DateTime.Now,
        Transitions = new List<Transition>
        {
            new Transition
            {
                From = new State(1, "State1") { IsInitial = true },
                To = new State(2, "State2"),
                Triggers = new List<Trigger> { new Trigger(1, "Trigger1") }
            }
        },
        States = new List<State>
        {
            new State(1, "State1") { IsInitial = true },
            new State(2, "State2")
        }
    };

    public static SerializedFlow SerializedFlow { get; } = new SerializedFlow
    {
        Key = Flow.Key,
        Name = Flow.Name,
        States = Flow.States.Select(s => new SerializedState
        {
            Name = s.Name,
            Code = s.Code
        }).ToList(),
        Triggers = Flow.Triggers.Select(t => new SerializedTrigger
        {
            Name = t.Name,
            Code = t.Code
        }).ToList(),
        Initial = new SerializedStep
        {
            FromStateCode = Flow.Transitions.First().From.Code,
            Allows = Flow.Transitions.Select(t => new SerializedTransition
            {
                AllowedStateCode = t.To.Code,
                TriggerCode = t.Triggers.First().Code
            }).ToList(),
            OnEntry = Flow.Transitions.First().From.Activities?
                .Select(a => new SerializedActivity
                {
                    Name = a.Name,
                    ActorName = a.ActorName
                })
                .FirstOrDefault(),
            OnExit = Flow.Transitions.First().From.Activities?
                .Select(a => new SerializedActivity
                {
                    Name = a.Name,
                    ActorName = a.ActorName
                })
                .FirstOrDefault()
        },
        Steps = Flow.Transitions
            .GroupBy(t => t.From.Code)
            .Select(group => new SerializedStep
            {
                FromStateCode = group.First().From.Code,
                Allows = group.Select(t => new SerializedTransition
                {
                    AllowedStateCode = t.To.Code,
                    TriggerCode = t.Triggers.First().Code
                }).ToList(),
                OnEntry = group.FirstOrDefault(t => t.From.Activities != null)?.From.Activities
                    .Select(a => new SerializedActivity
                    {
                        Name = a.Name,
                        ActorName = a.ActorName
                    }).FirstOrDefault(),
                OnExit = group.FirstOrDefault(t => t.Activities != null)?.Activities
                    .Select(a => new SerializedActivity
                    {
                        Name = a.Name,
                        ActorName = a.ActorName
                    }).FirstOrDefault()
            }).ToList()
    };
    
    public static string JsonFlow = "{ \"Key\": \"phone\", \"Name\": \"phone\", \"States\": [ { \"Code\": \"2\", \"Name\": \"Idle\" }, { \"Code\": \"3\", \"Name\": \"Ringing\" }, { \"Code\": \"4\", \"Name\": \"Busy\" }, { \"Code\": \"5\", \"Name\": \"Offline\" } ], \"Triggers\": [ { \"Code\": \"3\", \"Name\": \"Ringing\" }, { \"Code\": \"4\", \"Name\": \"Busy\" }, { \"Code\": \"5\", \"Name\": \"PowerOff\" }, { \"Code\": \"6\", \"Name\": \"Pickup\" }, { \"Code\": \"7\", \"Name\": \"Hangup\" } ], \"Initial\": { \"fromStateCode\": \"2\", \"allows\": [ { \"allowedStateCode\": \"3\", \"triggerCode\": \"3\" }, { \"allowedStateCode\": \"4\", \"triggerCode\": \"4\" } ], \"onEntry\": { \"name\": \"PhoneOnEnterIdleActivity\", \"actorName\": \"FlowCiao.Samples.Phone.Flow.Activities.PhoneOnEnterIdleActivity\" }, \"onExit\": { \"name\": \"PhoneOnExitIdleActivity\", \"actorName\": \"FlowCiao.Samples.Phone.Flow.Activities.PhoneOnExitIdleActivity\" } }, \"Steps\": [ { \"fromStateCode\": \"3\", \"allows\": [ { \"allowedStateCode\": \"5\", \"triggerCode\": \"5\" }, { \"allowedStateCode\": \"4\", \"triggerCode\": \"6\" }, { \"allowedStateCode\": \"2\", \"triggerCode\": \"7\" } ], \"onExit\": { \"name\": \"PhoneOnExitRingingActivity\", \"actorName\": \"FlowCiao.Samples.Phone.Flow.Activities.PhoneOnExitRingingActivity\" } }, { \"fromStateCode\": \"4\" }, { \"fromStateCode\": \"5\" } ] }";
}