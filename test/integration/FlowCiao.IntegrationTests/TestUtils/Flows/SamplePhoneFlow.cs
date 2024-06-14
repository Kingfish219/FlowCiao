using FlowCiao.IntegrationTests.TestUtils.Models;
using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Builder;

namespace FlowCiao.IntegrationTests.TestUtils.Flows;

public class SamplePhoneFlow : IFlowPlanner
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
                    .Allow(States.Busy, Triggers.Call);
            })
            .NewStep(stepBuilder =>
            {
                stepBuilder
                    .For(States.Ringing)
                    .Allow(States.Busy, Triggers.Pickup)
                    .Allow(States.Idle, Triggers.Hangup)
                    .Allow(States.Offline, Triggers.PowerOff);
            });

        return builder;
    }
}