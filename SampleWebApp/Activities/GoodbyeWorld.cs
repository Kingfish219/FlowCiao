using SmartFlow.Interfaces;
using SmartFlow.Models;

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
