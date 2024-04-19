using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Phone.Flow.Activities
{
    public class PhoneOnExitIdleActivity : IFlowActivity
    {
        public FlowResult Execute(FlowStepContext context)
        {
            Console.WriteLine("Phone exited idle");

            return FlowResult.Success();
        }
    }
}
