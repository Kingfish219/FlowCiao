using System;

namespace SmartFlow.Models
{
    public class ProcessEntity
    {
        public Guid Id { get; set; }
        public Guid LastStatus { get; set; }
        public int EntityType { get; set; }
    }

    public enum EntityCommandType
    {
        Create,
        Update
    }
}
