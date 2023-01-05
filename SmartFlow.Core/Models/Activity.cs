
using System;

namespace SmartFlow.Core.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public int ActivityTypeCode { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public IProcessActivity ProcessActivityExecutor { get; set; }
    }
}
