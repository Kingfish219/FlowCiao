using System;
using FlowCiao.Models.Execution;

namespace FlowCiao.Exceptions
{
    public class FlowCiaoExecutionException : Exception
    {
        public FlowCiaoExecutionException()
        {
        }

        public FlowCiaoExecutionException(string message, FlowInstanceStep flowStep, [System.Runtime.CompilerServices.CallerFilePath] string handlerName = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
            : base(message)
        {
            try
            {
                //var handlerClassName = handlerName.Split('\\');
                //var log = new Log()
                //{
                //    Id = Guid.NewGuid(),
                //    ProcessStepId = flowStep.Id,
                //    FlowId = flowStep.Flow.Id,
                //    EntityId = flowStep.Entity.Id,
                //    CreateDate = DateTime.Now,
                //    Type = 1,
                //    Data = message + "- Line: " + line,
                //    Handler = (handlerClassName.Length > 1 ? handlerClassName[handlerClassName.Length - 1] : handlerName)
                //};
                //logRepository.Create(log).GetAwaiter().GetResult();
            }
            catch (Exception) { }
        }
        
        public FlowCiaoExecutionException(string message)
            : base(message)
        {
        }

        public FlowCiaoExecutionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
