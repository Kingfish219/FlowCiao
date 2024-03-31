using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Phone.FLowCiao.Activities
{
    public class HelloWorld : IFlowActivity
    {
        public FlowResult Execute(FlowStepContext context)
        {
            Console.WriteLine("Hello world");

            return FlowResult.Success();
        }
    }
}
