using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlowCiao.Models.Core
{
    public class Flow
    {
        [Key]
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Transition> Transitions { get; set; } = new();
        public State InitialState { get; set; }
    }
}
