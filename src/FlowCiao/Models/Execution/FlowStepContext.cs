using System.Collections.Generic;

namespace FlowCiao.Models.Execution
{
    public class FlowStepContext
    {
        internal FlowExecution FlowExecution { get; set; }
        internal FlowExecutionStep FlowExecutionStep { get; set; }
        internal FlowExecutionStepDetail FlowExecutionStepDetail { get; set; }
        public Dictionary<object, object> Data { get; set; }

        internal FlowStepContext()
        {
            Data = new Dictionary<object, object>();
        }

        internal void ClearDictionary()
        {
            Data = new Dictionary<object, object>();
        }
    }
}
