using FlowCiao.Builders;

namespace FlowCiao.Models.Flow
{
    public interface IFlow
    {
        public string FlowKey { get; set; }
        public IFlowBuilder Construct<T>(IFlowBuilder action) where T : IFlow, new();
    }
}
