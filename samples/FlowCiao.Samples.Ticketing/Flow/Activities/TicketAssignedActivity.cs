using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Ticketing.Flow.Activities;

public class TicketAssignedActivity : IFlowActivity
{
    public FlowResult Execute(FlowStepContext context)
    {
        Console.WriteLine("Ticket is assigned");

        return new FlowResult();
    }
}