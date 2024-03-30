using FlowCiao.Builders;

namespace FlowCiao.Interfaces
{
    public interface IFlow
    {
        public string Key { get; set; }
        public IFlowBuilder Plan<T>(IFlowBuilder action) where T : IFlow, new();
    }
}
