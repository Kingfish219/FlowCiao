using FlowCiao.Models.Core;

namespace FlowCiao.Samples.Ticketing.Flow.Models;

public static class Triggers
{
    static Triggers()
    {
        Created = new Trigger(1, "Create");
        Assign = new Trigger(2, "Assign");
        Respond = new Trigger(3, "Respond");
        Accept = new Trigger(4, "Accept");
        Reject = new Trigger(5, "Reject");
    }
    
    public static Trigger Created { get; }
    public static Trigger Assign { get; }
    public static Trigger Respond { get; }
    public static Trigger Accept { get; }
    public static Trigger Reject { get; }
}