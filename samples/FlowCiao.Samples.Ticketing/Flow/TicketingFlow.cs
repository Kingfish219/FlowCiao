using FlowCiao.Interfaces;
using FlowCiao.Samples.Ticketing.Flow.Activities;
using FlowCiao.Samples.Ticketing.Flow.Models;

namespace FlowCiao.Samples.Ticketing.Flow;

public class TicketingFlow : IFlowPlanner
{
    public string Key { get; set; } = "ticketing";

    public IFlowBuilder Plan(IFlowBuilder builder)
    {
        builder.Initial(stepBuilder =>
            {
                stepBuilder.For(States.Start)
                    .Allow(States.New, Triggers.Created)
                    .OnEntry<TicketCreatedActivity>();
            })
            .NewStep(stepBuilder =>
            {
                stepBuilder.For(States.New)
                    .Allow(States.InProgress, Triggers.Assign)
                    .OnEntry<TicketArrivedActivity>();
            })
            .NewStep(stepBuilder =>
            {
                stepBuilder.For(States.InProgress)
                    .Allow(States.AwaitingApproval, Triggers.Respond)
                    .Allow(States.Start, Triggers.Assign)
                    .OnEntry<TicketAssignedActivity>()
                    .OnExit<TicketRespondActivity>();
            })
            .NewStep(stepBuilder =>
            {
                stepBuilder.For(States.AwaitingApproval)
                    .Allow(States.Done, Triggers.Accept)
                    .Allow(States.InProgress, Triggers.Reject)
                    .OnExit<TicketResponseAnsweredActivity>();
            });

        return builder;
    }
}