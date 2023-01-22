using System;
using Dapper.Contrib.Extensions;
using Dapper.FluentMap.Mapping;

namespace SmartFlow.Core.Models
{
    [Table("Action")]
    public class ProcessAction
    {
        public ProcessAction(int code)
        {
            Code = code;
        }

        public ProcessAction()
        {

        }

        [Key]
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int ActionTypeCode { get; set; }
        public Guid ProcessId { get; set; }
    }

    internal class ActionMap : EntityMap<ProcessAction>
    {
        internal ActionMap()
        {
            Map(x => x.Id).ToColumn("ActionId");
        }
    }

    public sealed class ProcessActionType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TypeCode { get; set; }
    }

    public enum ActionTypes
    {
        Crm = 1
    }
}
