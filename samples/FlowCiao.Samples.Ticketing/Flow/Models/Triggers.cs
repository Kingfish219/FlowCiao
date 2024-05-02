using FlowCiao.Models.Core;

namespace FlowCiao.Samples.Ticketing.Flow.Models;

public static class Triggers
{
    public static Trigger Created => new(1, "Create");
    public static Trigger Assign => new(2, "Assign");
    public static Trigger Respond => new(3, "Respond");
    public static Trigger Accept => new(4, "Accept");
    public static Trigger Reject => new(5, "Reject");
}
