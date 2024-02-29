using System.Collections.Generic;

namespace FlowCiao.Models.Builder.Json;

public class JsonStep
{
    public int FromStateCode { get; set; }
    public List<JsonTransition> Allows { get; set; }
    public JsonActivity OnEntry { get; set; }
    public JsonActivity OnExit { get; set; }
}