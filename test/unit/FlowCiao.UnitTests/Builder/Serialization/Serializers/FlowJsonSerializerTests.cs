using FlowCiao.Builder.Serialization;
using FlowCiao.Builder.Serialization.Serializers;
using FlowCiao.Interfaces.Builder;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models.Builder.Serialized;
using FlowCiao.Models.Core;
using Moq;
using FlowCiao.UnitTests.TestUtils.Flows;
using Newtonsoft.Json;

namespace FlowCiao.UnitTests.Builder.Serialization.Serializers
{
    public class FlowJsonSerializerTests
    {
        private readonly IFlowJsonSerializer _flowSerializer;
        private readonly SerializedFlow _serializedFlow;
        private readonly Flow _flow;

        public FlowJsonSerializerTests()
        {
            var mockActivityService = new Mock<IActivityService>();
            var mockFlowSerializerHelper = new Mock<IFlowSerializerHelper>();

            mockFlowSerializerHelper.Setup(service => service.CreateFlowPlanner(It.IsAny<SerializedFlow>()))
                .Returns((SerializedFlow serializedFlow) =>
                    new SerializerFlowPlanner(serializedFlow, mockActivityService.Object));

            _flow = Sample.Flow;
            _serializedFlow = Sample.SerializedFlow;

            mockFlowSerializerHelper.Setup(service => service.CreateSerializedFlow(It.IsAny<Flow>()))
                .Returns((Flow flow) => _serializedFlow);


            _flowSerializer = new FlowJsonSerializer(mockFlowSerializerHelper.Object);
        }

        [Fact]
        public void Import_ShouldWork()
        {
            var json = Sample.JsonFlow;
            var result = _flowSerializer.Import(json);
            
            Assert.Equal("phone", result.Key);
        }


        [Fact]
        public void Export_ShouldWork()
        {
            var result = _flowSerializer.Export(_flow);

            var expectedJson = JsonConvert.SerializeObject(_serializedFlow, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            
            Assert.Equal(expectedJson, result);
        }
    }
}