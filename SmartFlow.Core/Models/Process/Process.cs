using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Interfaces;

namespace SmartFlow.Core.Models
{
    [Table("Process")]
    public class Process : ISmartFlow
    {
        public Process()
        {
            Transitions = new List<Transition>();
        }

        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Transition> Transitions { get; set; }
        public ISmartFlow Construct<T>(ISmartFlowBuilder action) where T : ISmartFlow, new()
        {
            throw new NotImplementedException();
        }
    }
}
