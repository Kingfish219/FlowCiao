using FlowCiao.Builders;

namespace FlowCiao.Interfaces
{
    public interface IFlowPlanner
    {
        public string Key { get; set; }
        public IFlowBuilder Plan<T>(IFlowBuilder action) where T : IFlowPlanner, new();
    }
}
