using System;
using System.Collections.Generic;

namespace FlowCiao.Models.Builder.Json;

public class JsonStep
{
    public JsonState From { get; set; }
    public List<Tuple<JsonState, int>> Allows { get; set; }
}