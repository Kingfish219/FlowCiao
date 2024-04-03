using System.Collections.Generic;

namespace FlowCiao.Models.Builder.Json;

public class SerializedFlow
{
    public string Key { get; set; }
    public string Name { get; set; }
    public List<SerializedState> States { get; set; }
    public List<SerializedTrigger> Triggers { get; set; }
    public SerializedStep Initial { get; set; }
    public List<SerializedStep> Steps { get; set; }
}