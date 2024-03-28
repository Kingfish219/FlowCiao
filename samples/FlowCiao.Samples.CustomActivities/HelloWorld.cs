using FlowCiao.Interfaces;
using FlowCiao.Models;

namespace FlowCiao.Samples.CustomActivities
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
