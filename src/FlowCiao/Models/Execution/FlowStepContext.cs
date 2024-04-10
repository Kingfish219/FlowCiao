using System.Collections.Generic;

namespace FlowCiao.Models.Execution
{
    public class FlowStepContext
    {
        internal FlowInstance FlowInstance { get; set; }
        internal FlowInstanceStep FlowInstanceStep { get; set; }
        internal FlowInstanceStepDetail FlowInstanceStepDetail { get; set; }
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
