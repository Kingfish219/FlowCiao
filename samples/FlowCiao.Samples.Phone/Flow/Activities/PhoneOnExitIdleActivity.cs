using FlowCiao.Interfaces;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Phone.Flow.Activities
{
    public class PhoneOnExitIdleActivity : IFlowActivity
    {
        public void Execute(FlowStepContext context)
        {
            Console.WriteLine("Phone exited idle");
        }
    }
}
