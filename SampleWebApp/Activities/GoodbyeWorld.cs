using SmartFlow.Interfaces;
using SmartFlow.Models;

namespace SampleWebApp.Activities
{
    public class GoodbyeWorld : IProcessActivity
    {
        public ProcessResult Execute(ProcessStepContext context)
        {
            Console.WriteLine("Goodbye world");

            return ProcessResult.SuccessResult();
        }
    }
}
