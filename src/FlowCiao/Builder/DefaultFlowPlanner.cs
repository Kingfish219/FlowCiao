using System;
using FlowCiao.Interfaces;

namespace FlowCiao.Builder;

internal class DefaultFlowPlanner : IFlowPlanner
{
    public string Key { get; set; }

    private readonly Func<IFlowBuilder> _build;

    public DefaultFlowPlanner(string key, Func<IFlowBuilder> build)
    {
        Key = key;
        _build = build;
    }
    
    public IFlowBuilder Plan(IFlowBuilder builder)
    {
        return _build();
    }
}
