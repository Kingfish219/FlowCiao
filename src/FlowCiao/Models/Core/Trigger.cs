using System;
using System.ComponentModel.DataAnnotations;

namespace FlowCiao.Models.Core
{
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
        public Guid FlowId { get; set; }
        public int Priority { get; set; }
    }
}
