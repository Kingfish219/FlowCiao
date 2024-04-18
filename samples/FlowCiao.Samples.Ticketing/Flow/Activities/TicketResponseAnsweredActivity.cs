using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Ticketing.Flow.Activities;

public class TicketResponseAnsweredActivity : IFlowActivity
{
    public FlowResult Execute(FlowStepContext context)
    {
        Console.WriteLine("Response is answered");

        return new FlowResult();
    }
}