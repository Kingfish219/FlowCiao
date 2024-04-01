using System.Collections.Generic;
using FlowCiao.Models.Core;

namespace FlowCiao.Models.Builder;

public class FlowStep
{
    public State For { get; set; }
    public List<Transition> Allowed { get; set; }
}