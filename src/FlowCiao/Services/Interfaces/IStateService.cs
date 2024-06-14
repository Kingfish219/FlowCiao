using System;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;

namespace FlowCiao.Services.Interfaces;

public interface IStateService
{
    Task<FuncResult<Guid>> Modify(State state);
}