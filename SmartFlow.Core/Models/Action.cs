using System;
using Dapper.Contrib.Extensions;
using Dapper.FluentMap.Mapping;

namespace SmartFlow.Core.Models
{
    [Table("Action")]
    public class Action
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ActionTypeCode { get; set; }
        public Guid ProcessId { get; set; }
    }

    internal class ActionMap : EntityMap<Action>
    {
        internal ActionMap()
        {
            Map(x => x.Id).ToColumn("ActionId");
        }
    }
}
