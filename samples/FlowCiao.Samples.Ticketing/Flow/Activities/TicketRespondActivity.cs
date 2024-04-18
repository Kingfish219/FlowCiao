using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Ticketing.Flow.Activities;

public class TicketRespondActivity : IFlowActivity
{
    public FlowResult Execute(FlowStepContext context)
    {
        Console.WriteLine("Ticket is responded");

        return new FlowResult();
    }
}