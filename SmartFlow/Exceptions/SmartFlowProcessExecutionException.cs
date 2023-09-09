using System;
using SmartFlow.Models;
using SmartFlow.Persistence.Providers.SqlServer.Repositories;

namespace SmartFlow.Exceptions
{
    public class SmartFlowProcessExecutionException : Exception
    {
        public SmartFlowProcessExecutionException()
        {
        }

        public SmartFlowProcessExecutionException(string message, LogRepository logRepository, ProcessExecutionStep processStep, [System.Runtime.CompilerServices.CallerFilePath] string handlerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
            : base(message)
        {
            try
            {
                //var handlerClassName = handlerName.Split('\\');
                //var log = new Log()
                //{
                //    Id = Guid.NewGuid(),
                //    ProcessStepId = processStep.Id,
                //    ProcessId = processStep.Process.Id,
                //    EntityId = processStep.Entity.Id,
                //    CreateDate = DateTime.Now,
                //    Type = 1,
                //    Data = message + "- Line: " + line,
                //    Handler = (handlerClassName.Length > 1 ? handlerClassName[handlerClassName.Length - 1] : handlerName)
                //};
                //logRepository.Create(log).GetAwaiter().GetResult();
            }
            catch (Exception) { }
        }
        public SmartFlowProcessExecutionException(string message)
            : base(message)
        {
        }

        public SmartFlowProcessExecutionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
