using SampleWebApp.SmartFLow.Activities;
using SmartFlow.Builders;
using SmartFlow.Models.Flow;

namespace SampleWebApp.Flows
{
    public class PhoneStateMachine : ISmartFlow
    {
        public string FlowKey { get; set; } = "phone";

        public enum Actions
        {
            Ring = 1,
            Call = 2,
            Pickup = 3,
            Hangup = 4,
            PowerOff = 5
        }

        public ISmartFlowBuilder Construct<T>(ISmartFlowBuilder builder) where T : ISmartFlow, new()
        {
            var idle = new State(1, "idle");
            var ringing = new State(2, "ringing");
            var busy = new State(3, "busy");
            var offline = new State(4, "offline");

            builder
                .Initial(stepBuilder =>
                {
                    stepBuilder
                        .From(idle)
                        .Allow(ringing, (int)Actions.Ring, () =>
                        {
                            return false;
                        })
                        .Allow(busy, (int)Actions.Call)
                        .Allow(offline, (int)Actions.PowerOff)
                        .OnEntry<HelloWorld>()
                        .OnExit<GoodbyeWorld>();
                })
                .NewStep(stepBuilder =>
                {
                    stepBuilder
                        .From(ringing)
                        .Allow(offline, (int)Actions.PowerOff)
                        .Allow(idle, (int)Actions.Hangup)
                        .Allow(busy, (int)Actions.Pickup)
                        .OnExit<GoodbyeWorld>();
                });

            return builder;
        }
    }
}
