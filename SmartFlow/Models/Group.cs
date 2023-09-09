using System;
using Dapper.FluentMap.Mapping;

namespace SmartFlow.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
