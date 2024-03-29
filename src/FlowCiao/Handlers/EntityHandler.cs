﻿using System;
using FlowCiao.Models;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handlers
{
    internal class EntityHandler : WorkflowHandler
    {
        private readonly Func<ProcessEntity, ProcessResult> _command;

        public EntityHandler(IProcessRepository processRepository
            , IProcessService processService
            , Func<ProcessEntity, ProcessResult> command) : base(processRepository, processService)
        {
            _command = command;
        }

        //internal EntityHandler(IProcessRepository processRepository, Func<Entity, ProcessResult> command) : base(processRepository)
        //{
        //    _command = command;
        //}

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                //processStepContext.ProcessStep.Entity.LastStatus = processStepContext.ProcessStep.TransitionActions.FirstOrDefault().Transition.CurrentStateId;
                //var result = _command(processStepContext.ProcessStep.Entity);
                //if (result.Status != ProcessResultStatus.Completed)
                //{
                //    return result;
                //}

                return NextHandler.Handle(processStepContext);
            }
            catch (Exception exception)
            {
                return new ProcessResult
                {
                    Status = ProcessResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }

        public override ProcessResult RollBack(ProcessStepContext processStepContext)
        {
            return PreviousHandler?.RollBack(processStepContext) ?? new ProcessResult
            {
                Status = ProcessResultStatus.Failed
            };
        }
    }
}
