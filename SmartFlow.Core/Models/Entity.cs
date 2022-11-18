using System;

namespace SmartFlow.Core.Models
{
    public class Entity
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
