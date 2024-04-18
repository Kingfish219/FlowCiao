using FlowCiao.Models.Core;

namespace FlowCiao.Samples.Ticketing.Flow.Models;

public static class States
{
    static States()
    {
        Created = new State(1, "Created");
        New = new State(2, "New");
        InProgress = new State(3, "In Progress 22");
        AwaitingApproval = new State(4, "Awaiting Answer");
        Done = new State(5, "Done");
    }

    public static State Created { get; }
    public static State New { get; }
    public static State InProgress { get; }
    public static State AwaitingApproval { get; }
    public static State Done { get; }
}