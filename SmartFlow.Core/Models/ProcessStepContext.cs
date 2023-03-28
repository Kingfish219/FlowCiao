using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class ProcessStepContext
    {
        internal Process Process { get; set; }
        internal ProcessExecutionStep ProcessStep { get; set; }
        internal ProcessExecutionStepDetail ProcessStepDetail { get; set; }
        internal ProcessUser ProcessUser { get; set; }
        internal ProcessStepInput ProcessStepInput { get; set; }
        public Dictionary<string, object> Data { get; set; }

        internal ProcessStepContext()
        {
            Data = new Dictionary<string, object>();
        }

        internal void ClearDictionary()
        {
            Data = new Dictionary<string, object>();
        }
    }
}
