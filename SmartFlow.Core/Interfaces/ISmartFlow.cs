using SmartFlow.Core.Builders;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Interfaces
{
    public interface ISmartFlow
    {
        public string FlowKey { get; set; }
        public Process Construct<T>(ISmartFlowBuilder action) where T : ISmartFlow, new();
    }
}
