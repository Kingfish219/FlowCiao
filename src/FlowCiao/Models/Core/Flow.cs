using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FlowCiao.Models.Core
{
    public class Flow : BaseEntity
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Transition> Transitions { get; set; } = null!;

        public List<State> States { get; set; } = null!;

        [NotMapped]
        public List<Trigger> Triggers => Transitions?.SelectMany(t => t.Triggers)?
            .DistinctBy(t => t.Code)
            .ToList();

        [NotMapped]
        public string SerializedJson { get; set; }

        [NotMapped]
        public List<State> InitialStates => Transitions?.Where(x => x.From.IsInitial).Select(x => x.From).ToList();
    }
}