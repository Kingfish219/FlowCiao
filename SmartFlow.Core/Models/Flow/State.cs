using Dapper.Contrib.Extensions;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    [Table("State")]
    public class State
    {
        public State(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public State()
        {

        }

        [Key]
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StateType TypeId { get; set; }
        public Guid ProcessId { get; set; }
        public bool IsFinal { get; set; }
        public bool IsInitial { get; set; }
        public string OwnerId { get; set; }
        public List<Activity> Activities { get; set; }
    }

    public class StateType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    internal class StateMap : EntityMap<State>
    {
        internal StateMap()
        {
            Map(x => x.Id).ToColumn("StateId");
        }
    }
}
