using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public List<State> States { get; set; } = new();

        public List<Trigger> Triggers { get; set; } = new();

        [NotMapped]
        public string SerializedJson { get; set; }

        [NotMapped]
        public List<State> InitialStates => Transitions.Where(x => x.From.IsInitial).Select(x => x.From).ToList();
    }
}