using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Serialized;
using FlowCiao.Models.Core;
using Newtonsoft.Json;

namespace FlowCiao.Builder.Serialization.Serializers;

public class FlowJsonSerializer
{
    private readonly FlowSerializerHelper _flowSerializerHelper;

    public FlowJsonSerializer(FlowSerializerHelper flowSerializerHelper)
    {
        _flowSerializerHelper = flowSerializerHelper;
    }

    public IFlowPlanner Import(string json)
    {
        var jsonFlow = JsonConvert.DeserializeObject<SerializedFlow>(json);

        return _flowSerializerHelper.CreateFlowPlanner(jsonFlow);
    }

    public string Export(Flow flow)
    {
        var serializedFlow = _flowSerializerHelper.CreateSerializedFlow(flow);

        return JsonConvert.SerializeObject(serializedFlow, settings: new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }
}