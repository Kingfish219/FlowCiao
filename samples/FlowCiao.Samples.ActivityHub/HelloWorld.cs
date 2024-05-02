using FlowCiao.Interfaces;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.ActivityHub
{
    public class HelloWorld : IFlowActivity
    {
        public void Execute(FlowStepContext context)
        {
            Console.WriteLine("Hello world");
        }
    }
}
