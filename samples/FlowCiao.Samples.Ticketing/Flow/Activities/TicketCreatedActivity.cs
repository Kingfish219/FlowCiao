using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Ticketing.Flow.Activities;

public class TicketCreatedActivity : IFlowActivity
{
    public FlowResult Execute(FlowStepContext context)
    {
        Console.WriteLine("Ticket is created");

        return new FlowResult();
    }
}