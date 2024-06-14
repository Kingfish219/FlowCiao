using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces.Builder;

public interface IFlowJsonSerializer
{
    IFlowPlanner Import(string json);
    string Export(Flow flow);
}