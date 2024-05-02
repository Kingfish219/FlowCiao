using System.Collections.Generic;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;

namespace FlowCiao.Models.Builder;

public class FlowStep
{
    public State For { get; set; }
    public List<Transition> Allowed { get; } = new();
    public IFlowActivity OnEntry { get; set; }
    public IFlowActivity OnExit { get; set; }
}