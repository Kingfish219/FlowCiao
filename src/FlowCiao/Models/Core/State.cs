using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCiao.Models.Core
{
    public class State
    {
        public State(int code, string name)
        {
            Code = code;
            Name = name;
        }

        [Key]
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("FlowId")]
        public Flow Flow { get; set; }
        public bool IsFinal { get; set; }
        public bool IsInitial { get; set; }
        public List<Activity> Activities { get; set; }

        public List<StateActivity> StateActivities { get; set; }
    }
}
