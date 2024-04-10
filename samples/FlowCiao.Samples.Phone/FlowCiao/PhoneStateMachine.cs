using FlowCiao.Interfaces;
using FlowCiao.Models.Core;
using FlowCiao.Samples.Phone.FlowCiao.Activities;

namespace FlowCiao.Samples.Phone.FlowCiao
{
    public class PhoneStateMachine : IFlowPlanner
    {
        public string Key { get; set; } = "phone";
        public enum Triggers
        {
            Ring = 1,
            Call = 2,
            Pickup = 3,
            Hangup = 4,
            PowerOff = 5
        }

        public IFlowBuilder Plan(IFlowBuilder builder)
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
                        .Allow(ringing, (int)Triggers.Ring)
                        .Allow(busy, (int)Triggers.Call)
                        //.Allow(offline, (int)Triggers.PowerOff)
                        .OnEntry<HelloWorld>()
                        .OnExit<GoodbyeWorld>();
                })
                .NewStep(stepBuilder =>
                {
                    stepBuilder
                        .For(ringing)
                        .Allow(offline, (int)Triggers.PowerOff)
                        .Allow(idle, (int)Triggers.Hangup)
                        .Allow(busy, (int)Triggers.Pickup)
                        .OnExit<GoodbyeWorld>();
                });

            return builder;
        }
    }
}
