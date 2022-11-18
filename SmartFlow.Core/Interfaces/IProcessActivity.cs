using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    public interface IProcessActivity
    {
        //ProcessResult Invoke(Entity entity, ProcessUser user);
        //ProcessResult Invoke(ProcessStepContext context);
        ProcessResult Invoke();
    }
}