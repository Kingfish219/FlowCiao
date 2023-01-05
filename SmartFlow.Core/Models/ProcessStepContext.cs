using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class ProcessStepContext
    {
        public ProcessStep ProcessStep { get; set; }
        public ProcessUser ProcessUser { get; set; }
        public ProcessStepInput ProcessStepInput { get; set; }
        public EntityCommandType EntityCommandType { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public ProcessStepContext()
        {
            Data = new Dictionary<string, object>();
        }

        internal void ClearDictionary()
        {
            Data = new Dictionary<string, object>();
        }
    }
}
