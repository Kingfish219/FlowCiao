﻿using FlowCiao.Interfaces;
using FlowCiao.Models;

namespace FlowCiao.Samples.Phone.FLowCiao.Activities
{
    public class GoodbyeWorld : IProcessActivity
    {
        public ProcessResult Execute(ProcessStepContext context)
        {
            Console.WriteLine("Goodbye world");

            return ProcessResult.SuccessResult();
        }
    }
}
