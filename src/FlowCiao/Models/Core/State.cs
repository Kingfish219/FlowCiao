using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlowCiao.Models.Core
{
    public class State
    {
        public State(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public State()
        {

        }

        [Key]
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid FlowId { get; set; }
        public bool IsFinal { get; set; }
        public bool IsInitial { get; set; }
        public string OwnerId { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
