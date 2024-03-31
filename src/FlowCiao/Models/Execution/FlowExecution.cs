using System;
using System.Collections.Generic;
using System.Linq;
using FlowCiao.Models.Core;
using Newtonsoft.Json;

namespace FlowCiao.Models.Execution
{
    public class FlowExecution
    {
        public Guid Id { get; set; }
        public Flow Flow { get; set; }
        public FlowExecutionState ExecutionState { get; set; }
        public FlowExecutionStep ActiveExecutionStep => ExecutionSteps.SingleOrDefault(x => !x.IsCompleted);
        public List<FlowExecutionStep> ExecutionSteps { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Progress
        {
            get => JsonConvert.SerializeObject(ExecutionSteps);
            set => ExecutionSteps = JsonConvert.DeserializeObject<List<FlowExecutionStep>>(value);
        }
        public State State { get; set; }

        public enum FlowExecutionState
        {
            Initial,
            Pending,
            Running,
            Suspended,
            Finished
        }
    }
}
