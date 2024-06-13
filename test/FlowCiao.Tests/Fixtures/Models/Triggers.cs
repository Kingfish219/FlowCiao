using FlowCiao.Models.Core;

namespace FlowCiao.Tests.Fixtures.Models;

public static class Triggers
{
    public static Trigger Ring => new(1, "Ring");
    public static Trigger Call => new(2, "Call");
    public static Trigger Pickup => new(3, "Pickup");
    public static Trigger Hangup => new(4, "Hangup");
    public static Trigger PowerOff => new(5, "Power off");
}
