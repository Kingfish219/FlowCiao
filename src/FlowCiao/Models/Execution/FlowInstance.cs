using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FlowCiao.Models.Core;
using Newtonsoft.Json;

namespace FlowCiao.Models.Execution
{
    public class FlowInstance : BaseEntity
    {
        public Guid FlowId { get; set; }
        
        public Flow Flow { get; set; }

        public FlowExecutionState ExecutionState { get; set; }

        [NotMapped] 
        public FlowInstanceStep ActiveInstanceStep => InstanceSteps.SingleOrDefault(x => !x.IsCompleted);

        [NotMapped] 
        public List<FlowInstanceStep> InstanceSteps { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Progress
        {
            get => JsonConvert.SerializeObject(InstanceSteps, settings: new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            set => InstanceSteps = JsonConvert.DeserializeObject<List<FlowInstanceStep>>(value);
        }

        [NotMapped] 
        public State State => InstanceSteps.FirstOrDefault()?.Details.FirstOrDefault()?.Transition?.From;

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