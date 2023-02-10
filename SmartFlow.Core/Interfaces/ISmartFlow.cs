using SmartFlow.Core.Builders;

namespace SmartFlow.Core.Interfaces
{
    public interface ISmartFlow
    {
        public string FlowKey { get; set; }
        public ISmartFlow Construct<T>(ISmartFlowBuilder action) where T : ISmartFlow, new();
    }
}
