using System;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces.Services;

public interface ITransitionService
{
    Task<FuncResult<Guid>> Modify(Transition transition);
}