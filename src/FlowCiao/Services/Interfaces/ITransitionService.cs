using System;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;

namespace FlowCiao.Services.Interfaces;

public interface ITransitionService
{
    Task<FuncResult<Guid>> Modify(Transition transition);
}