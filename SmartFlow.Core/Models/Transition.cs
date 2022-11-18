using System;
using Dapper.Contrib.Extensions;
using Dapper.FluentMap.Mapping;

namespace SmartFlow.Core.Models
{
    [Table("Transition")]
    public class Transition
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProcessId { get; set; }
        public Guid CurrentStateId { get; set; }
        public Guid NextStateId { get; set; }
    }

    internal class TransitionMap : EntityMap<Transition>
    {
        internal TransitionMap()
        {
            Map(x => x.Id).ToColumn("TransitionId");
        }
    }
}
