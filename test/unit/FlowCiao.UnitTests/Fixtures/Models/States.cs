using FlowCiao.Models.Core;

namespace FlowCiao.UnitTests.Fixtures.Models;

public static class States
{
    public static State Idle => new(1, "Idle");
    public static State Ringing => new(2, "Ringing");
    public static State Busy => new(3, "Busy");
    public static State Offline => new(4, "Offline");
}