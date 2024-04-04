using System.Collections.Generic;

namespace FlowCiao.Models.Builder.Serialized;

public class SerializedStep
{
    public int FromStateCode { get; set; }
    public List<SerializedTransition> Allows { get; set; } = new();
    public SerializedActivity OnEntry { get; set; }
    public SerializedActivity OnExit { get; set; }
}
