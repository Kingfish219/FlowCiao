using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Phone.FlowCiao.Activities
{
    public class GoodbyeWorld : IFlowActivity
    {
        public FlowResult Execute(FlowStepContext context)
        {
            Console.WriteLine("Goodbye world");

            return FlowResult.Success();
        }
    }
}
