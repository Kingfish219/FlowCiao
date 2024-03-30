using System;
using FlowCiao.Interfaces;
using Newtonsoft.Json;

namespace FlowCiao.Models.Core
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ActivityTypeCode { get; set; }
        [JsonIgnore]
        public IProcessActivity ProcessActivityExecutor { get; set; }
    }
}

