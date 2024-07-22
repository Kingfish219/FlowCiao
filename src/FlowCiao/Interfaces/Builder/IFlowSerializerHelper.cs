using FlowCiao.Models.Builder.Serialized;
using FlowCiao.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowCiao.Interfaces.Builder
{
    public interface IFlowSerializerHelper
    {
        SerializedFlow CreateSerializedFlow(Flow flow);
        IFlowPlanner CreateFlowPlanner(SerializedFlow serializedFlow);
    }
}
