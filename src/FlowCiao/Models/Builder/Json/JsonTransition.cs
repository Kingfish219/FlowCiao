namespace FlowCiao.Models.Builder.Json;

public class JsonTransition
{
    public JsonState From { get; set; }
    public JsonState To { get; set; }
    public JsonAction Action { get; set; }
}