using FlowCiao.Interfaces;
using FlowCiao.Models;

namespace FlowCiao.Samples.Phone.SmartFLow.Activities
{
    public class HelloWorld : IProcessActivity
    {
        public ProcessResult Execute(ProcessStepContext context)
        {
            Console.WriteLine("Hello world");

            return ProcessResult.SuccessResult();
        }
    }
}
