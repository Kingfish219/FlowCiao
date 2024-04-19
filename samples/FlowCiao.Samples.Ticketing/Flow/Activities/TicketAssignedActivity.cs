using FlowCiao.Interfaces;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Ticketing.Flow.Activities;

public class TicketAssignedActivity : IFlowActivity
{
    public void Execute(FlowStepContext context)
    {
        Console.WriteLine("Ticket is assigned");
    }
}