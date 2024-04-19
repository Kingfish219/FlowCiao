using FlowCiao.Interfaces;
using FlowCiao.Models.Core;
using FlowCiao.Samples.Phone.Flow.Activities;
using FlowCiao.Samples.Phone.Flow.Models;

namespace FlowCiao.Samples.Phone.Flow
{
    public class PhoneFlow : IFlowPlanner
    {
        public string Key { get; set; } = "phone";

        public IFlowBuilder Plan(IFlowBuilder builder)
        {
            builder
                .Initial(stepBuilder =>
                {
                    stepBuilder
                        .For(States.Idle)
                        .Allow(States.Ringing, Triggers.Ring)
                        .Allow(States.Busy, Triggers.Call)
                        .OnEntry<PhoneOnEnterIdleActivity>()
                        .OnExit<PhoneOnExitIdleActivity>();
                })
                .NewStep(stepBuilder =>
                {
                    stepBuilder
                        .For(States.Ringing)
                        .Allow(States.Busy, Triggers.Pickup)
                        .Allow(States.Idle, Triggers.Hangup)
                        .Allow(States.Offline, Triggers.PowerOff)
                        .OnExit<PhoneOnExitIdleActivity>();
                });

            return builder;
        }
    }
}
