namespace FlowCiao.Interfaces
{
    public interface IFlowStepPlanner
    {
        public IFlowStepBuilder Plan<T>(IFlowStepBuilder action) where T : IFlowStepPlanner, new();
    }
}
