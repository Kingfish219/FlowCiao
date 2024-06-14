using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Interfaces.Services;

internal interface IFlowService
{
    Task<List<Flow>> Get();
    Task<Flow> GetByKey(Guid flowId = default, string key = default);
    Task<Guid> Modify(Flow flow);
    FlowInstanceStep GenerateFlowStep(Flow flow, State state);
    Task<FuncResult> Deactivate(Flow flow);
}