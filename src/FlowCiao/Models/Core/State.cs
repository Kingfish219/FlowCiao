using System;
using System.Collections.Generic;

namespace FlowCiao.Models.Core
{
    public class State : BaseEntity
    {
        public State(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public int Code { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public Guid FlowId { get; set; }

        public Flow Flow { get; set; } = null!;
        
        public bool IsFinal { get; set; }
        
        public bool IsInitial { get; set; }
        
        public List<Activity> Activities { get; set; } = null!;

        public List<StateActivity> StateActivities { get; set; } = null!;
    }
}
