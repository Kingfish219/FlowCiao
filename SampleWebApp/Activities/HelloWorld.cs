using SmartFlow.Core;
using SmartFlow.Core.Models;

namespace SampleWebApp.Activities
{
    public class HelloWorld : IProcessActivity
    {
        public ProcessResult Execute(ProcessStepContext context)
        {
            Console.WriteLine("Hello world");

            return ProcessResult.SuccessResult();
        }

        public ProcessResult Execute()
        {
            throw new NotImplementedException();
        }
    }
}
