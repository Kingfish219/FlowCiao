using System;

namespace SmartFlow.Core.Interfaces
{
    public interface ISmartFlow
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
    }
}
