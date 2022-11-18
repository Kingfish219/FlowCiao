using System;

namespace SmartFlow.Core.Models
{
    public class Status
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool RequestResponse { get; set; }
        public string ResponseController { get; set; }
        public string ResponseActions { get; set; }
        public bool IsFinalResponse { get; set; }
        public string Number { get; set; }

    }
}
