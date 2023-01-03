using SmartFlow.Core.Models;
using SmartFlow.Core;

namespace SampleWebApp.Activities
{
    public class GoodbyeWorld : IProcessActivity
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
