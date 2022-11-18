using System;
using Dapper.Contrib.Extensions;

namespace SmartFlow.Core.Models
{
    [Table("Log")]
    public class Log
    {
        [ExplicitKey]
        public Guid Id { get; set; }
        public Guid? ProcessId { get; set; }
        public Guid? ProcessStepId { get; set; }
        public Guid? EntityId { get; set; }
        public string Handler { get; set; }
        public string Data { get; set; }
        public int Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
