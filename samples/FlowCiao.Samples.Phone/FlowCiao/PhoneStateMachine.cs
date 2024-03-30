using FlowCiao.Builders;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;
using FlowCiao.Samples.Phone.FLowCiao.Activities;

namespace FlowCiao.Samples.Phone.FLowCiao
{
    public class PhoneStateMachine : IFlow
    {
        public string Key { get; set; } = "phone";
        private enum Actions
        {
            Ring = 1,
            Call = 2,
            Pickup = 3,
            Hangup = 4,
            PowerOff = 5
        }

        public IFlowBuilder Construct<T>(IFlowBuilder builder) where T : IFlow, new()
        {
            var idle = new State(1, "idle");
            var ringing = new State(2, "ringing");
            var busy = new State(3, "busy");
            var offline = new State(4, "offline");

            builder
                .Initial(stepBuilder =>
                {
                    stepBuilder
                        .For(idle)
                        .Allow(ringing, (int)Actions.Ring)
                        .Allow(busy, (int)Actions.Call)
                        .Allow(offline, (int)Actions.PowerOff)
                        .OnEntry<HelloWorld>()
                        .OnExit<GoodbyeWorld>();
                })
                .NewStep(stepBuilder =>
                {
                    stepBuilder
                        .For(ringing)
                        .Allow(offline, (int)Actions.PowerOff)
                        .Allow(idle, (int)Actions.Hangup)
                        .Allow(busy, (int)Actions.Pickup)
                        .OnExit<GoodbyeWorld>();
                });

            return builder;
        }
    }
}
