using SmartFlow.Builders;

namespace SmartFlow.Models.Flow
{
    public interface ISmartFlow
    {
        public string FlowKey { get; set; }
        public ISmartFlowBuilder Construct<T>(ISmartFlowBuilder action) where T : ISmartFlow, new();
    }
}
