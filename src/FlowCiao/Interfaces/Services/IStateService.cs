using System;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces.Services;

public interface IStateService
{
    Task<FuncResult<Guid>> Modify(State state);
}