
using System;
using Newtonsoft.Json;

namespace SmartFlow.Core.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public int ActivityTypeCode { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public IProcessActivity ProcessActivityExecutor { get; set; }
    }

    public sealed class ActivityType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TypeCode { get; set; }
    }

    public enum ActivityTypes
    {
        Crm = 1,
        SendToManager = 3,
        NotifyStakeHolders = 4,
        UpdateCaseStatusInCrm = 2
    }
}
