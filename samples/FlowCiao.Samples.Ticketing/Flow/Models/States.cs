using FlowCiao.Models.Core;

namespace FlowCiao.Samples.Ticketing.Flow.Models;

public static class States
{
    public static State Start => new(1, "Start");
    public static State New => new(2, "New");
    public static State InProgress => new(3, "In progress");
    public static State AwaitingApproval => new(4, "Awaiting answer");
    public static State Done => new(5, "Done");
}