using SmartFlow.Builders;

namespace SmartFlow.Interfaces
{
    public interface ISmartFlow
    {
        public string FlowKey { get; set; }
        public ISmartFlowBuilder Construct<T>(ISmartFlowBuilder action) where T : ISmartFlow, new();
    }
}
