using FlowCiao.Interfaces;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Ticketing.Flow.Activities;

public class TicketRespondActivity : IFlowActivity
{
    public void Execute(FlowStepContext context)
    {
        Console.WriteLine("Ticket is responded");
    }
}