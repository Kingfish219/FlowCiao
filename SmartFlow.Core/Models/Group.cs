using System;
using Dapper.FluentMap.Mapping;

namespace SmartFlow.Core.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    internal class GroupMap : EntityMap<Group>
    {
        internal GroupMap()
        {
            Map(x => x.Id).ToColumn("GroupId");
        }
    }
}
