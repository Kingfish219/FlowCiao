using System;
using System.Linq;
using FlowCiao.Interfaces;
using Newtonsoft.Json;

namespace FlowCiao.Models.Core
{
    public class Activity
    {
        public Activity()
        {
            
        }
        
        public Activity(IFlowActivity actor)
        {
            Actor = actor;
            var actorType = actor.GetType();
            if (string.IsNullOrWhiteSpace(actorType.FullName))
            {
                return;
            }
            
            Name = actorType.FullName.Split('.').Last();
            ActorName = actorType.FullName;
        }
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ActivityTypeCode { get; set; } = 1;
        public string ActorName { get; set; }
        public byte[] ActorContent { get; set; }
        
        [JsonIgnore]
        public IFlowActivity Actor { get; set; }
    }
}

