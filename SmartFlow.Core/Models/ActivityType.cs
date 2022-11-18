using System;

namespace SmartFlow.Core.Models
{
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
