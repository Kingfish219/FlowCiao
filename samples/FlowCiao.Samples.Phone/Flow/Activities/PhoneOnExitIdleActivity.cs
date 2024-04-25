using FlowCiao.Interfaces;
using FlowCiao.Models.Execution;

namespace FlowCiao.Samples.Phone.Flow.Activities
{
    public class PhoneOnExitIdleActivity : IFlowActivity
    {
        public void Execute(FlowStepContext context)
        {
            Console.WriteLine($"Phone exited idle state: caller id: {context.Data["CallerId"]}");
            context.Data["Location"] = "USA";
        }
    }
}
