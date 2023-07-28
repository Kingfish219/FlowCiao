
using System;

namespace SmartFlow.Models
{
    internal class UserGroup
    {
        public Guid Id { get; set; }
        public Group Group { get; set; }
        public ProcessUser User { get; set; }
    }
}
