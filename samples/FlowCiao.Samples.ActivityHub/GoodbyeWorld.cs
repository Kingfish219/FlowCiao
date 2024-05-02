using FlowCiao.Interfaces;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.ActivityHub
{
    public class GoodbyeWorld : IFlowActivity
    {
        public void Execute(FlowStepContext context)
        {
            Console.WriteLine("Goodbye world");
        }
    }
}
