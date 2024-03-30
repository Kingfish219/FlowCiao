using System;
using FlowCiao.Models;

namespace FlowCiao.Exceptions
{
    public class FlowCiaoProcessExecutionException : Exception
    {
        public FlowCiaoProcessExecutionException()
        {
        }

        public FlowCiaoProcessExecutionException(string message, ProcessExecutionStep processStep, [System.Runtime.CompilerServices.CallerFilePath] string handlerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
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
        public FlowCiaoProcessExecutionException(string message)
            : base(message)
        {
        }

        public FlowCiaoProcessExecutionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
