using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FlowCiao.Models.Core;
using Newtonsoft.Json;

namespace FlowCiao.Models.Execution
{
    public class FlowExecution
    {
        public Guid Id { get; set; }

        [ForeignKey("FlowId")] 
        public Flow Flow { get; set; }

        public FlowExecutionState ExecutionState { get; set; }

        [NotMapped] 
        public FlowExecutionStep ActiveExecutionStep => ExecutionSteps.SingleOrDefault(x => !x.IsCompleted);

        [NotMapped] 
        public List<FlowExecutionStep> ExecutionSteps { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Progress
        {
            get => JsonConvert.SerializeObject(ExecutionSteps);
            set => ExecutionSteps = JsonConvert.DeserializeObject<List<FlowExecutionStep>>(value);
        }

        [NotMapped] 
        public State State => ExecutionSteps.FirstOrDefault()?.Details.FirstOrDefault()?.Transition?.From;

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