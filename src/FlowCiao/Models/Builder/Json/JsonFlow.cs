using System.Collections.Generic;

namespace FlowCiao.Models.Builder.Json;

public class JsonFlow
{
    public string Key { get; set; }
    public string Name { get; set; }
    public List<JsonState> States { get; set; }
    public List<JsonTrigger> Triggers { get; set; }
    public JsonStep Initial { get; set; }
    public List<JsonStep> Steps { get; set; }
}