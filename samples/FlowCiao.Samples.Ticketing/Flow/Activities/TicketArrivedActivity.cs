using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Ticketing.Flow.Activities;

public class TicketArrivedActivity : IFlowActivity
{
    public FlowResult Execute(FlowStepContext context)
    {
        Console.WriteLine("Ticket arrived");

        return new FlowResult();
    }
}