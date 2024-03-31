using System;
using Dapper.Contrib.Extensions;

namespace FlowCiao.Models.Core
{
    [Table("Trigger")]
    public class Trigger
    {
        public Trigger(int code)
        {
            Code = code;
        }

        [Key]
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int TriggerType { get; set; }
        public Guid ProcessId { get; set; }
        public int Priority { get; set; }
    }
}
