using FlowCiao.Builder.Serialization;
using FlowCiao.Builder.Serialization.Serializers;
using FlowCiao.Interfaces.Builder;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models.Builder.Serialized;
using FlowCiao.Models.Core;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowCiao.UnitTests.Builder.Serialization.Serializers
{
    public class FlowJsonSerializerTests
    {
        private readonly IFlowJsonSerializer _flowSerializer;
        private readonly SerializedFlow serializedFlow;
        private readonly Flow flow;
        public FlowJsonSerializerTests()
        {
            var mockActivityService = new Mock<IActivityService>();
            var mockFlowSerializerHelper = new Mock<IFlowSerializerHelper>();

            mockFlowSerializerHelper.Setup(service => service.CreateFlowPlanner(It.IsAny<SerializedFlow>()))
                .Returns((SerializedFlow serializedFlow) => new SerializerFlowPlanner(serializedFlow, mockActivityService.Object));

            flow = new Flow
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

            serializedFlow = new SerializedFlow
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
                Initial = new SerializedStep
                {
                    FromStateCode = flow.Transitions.First().From.Code,
                    Allows = flow.Transitions.Select(t => new SerializedTransition
                    {
                        AllowedStateCode = t.To.Code,
                        TriggerCode = t.Triggers.First().Code
                    }).ToList(),
                    OnEntry = flow.Transitions.First().From.Activities?
                        .Select(a => new SerializedActivity
                        {
                            Name = a.Name,
                            ActorName = a.ActorName
                        })
                        .FirstOrDefault(),
                    OnExit = flow.Transitions.First().From.Activities?
                        .Select(a => new SerializedActivity
                        {
                            Name = a.Name,
                            ActorName = a.ActorName
                        })
                        .FirstOrDefault()
                },
                Steps = flow.Transitions
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

            mockFlowSerializerHelper.Setup(service => service.CreateSerializedFlow(It.IsAny<Flow>()))
            .Returns((Flow flow) => serializedFlow);


            _flowSerializer = new FlowJsonSerializer(mockFlowSerializerHelper.Object);

        }

        [Fact]
        public void Import_ShouldWork()
        {
            var json = "{ \"Key\": \"phone\", \"Name\": \"phone\", \"States\": [ { \"Code\": \"2\", \"Name\": \"Idle\" }, { \"Code\": \"3\", \"Name\": \"Ringing\" }, { \"Code\": \"4\", \"Name\": \"Busy\" }, { \"Code\": \"5\", \"Name\": \"Offline\" } ], \"Triggers\": [ { \"Code\": \"3\", \"Name\": \"Ringing\" }, { \"Code\": \"4\", \"Name\": \"Busy\" }, { \"Code\": \"5\", \"Name\": \"PowerOff\" }, { \"Code\": \"6\", \"Name\": \"Pickup\" }, { \"Code\": \"7\", \"Name\": \"Hangup\" } ], \"Initial\": { \"fromStateCode\": \"2\", \"allows\": [ { \"allowedStateCode\": \"3\", \"triggerCode\": \"3\" }, { \"allowedStateCode\": \"4\", \"triggerCode\": \"4\" } ], \"onEntry\": { \"name\": \"PhoneOnEnterIdleActivity\", \"actorName\": \"FlowCiao.Samples.Phone.Flow.Activities.PhoneOnEnterIdleActivity\" }, \"onExit\": { \"name\": \"PhoneOnExitIdleActivity\", \"actorName\": \"FlowCiao.Samples.Phone.Flow.Activities.PhoneOnExitIdleActivity\" } }, \"Steps\": [ { \"fromStateCode\": \"3\", \"allows\": [ { \"allowedStateCode\": \"5\", \"triggerCode\": \"5\" }, { \"allowedStateCode\": \"4\", \"triggerCode\": \"6\" }, { \"allowedStateCode\": \"2\", \"triggerCode\": \"7\" } ], \"onExit\": { \"name\": \"PhoneOnExitRingingActivity\", \"actorName\": \"FlowCiao.Samples.Phone.Flow.Activities.PhoneOnExitRingingActivity\" } }, { \"fromStateCode\": \"4\" }, { \"fromStateCode\": \"5\" } ] }";
            var result = _flowSerializer.Import(json);
            Assert.Equal("phone", result.Key);
        }


        [Fact]
        public void Export_ShouldWork()
        {

            var result = _flowSerializer.Export(flow);

            var expectedJson = JsonConvert.SerializeObject(serializedFlow, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            Assert.Equal(expectedJson, result);
        }
    }
}
