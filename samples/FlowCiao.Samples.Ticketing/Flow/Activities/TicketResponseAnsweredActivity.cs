using FlowCiao.Interfaces;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Ticketing.Flow.Activities;

public class TicketResponseAnsweredActivity : IFlowActivity
{
    public void Execute(FlowStepContext context)
    {
        Console.WriteLine("Response is answered");
    }
}