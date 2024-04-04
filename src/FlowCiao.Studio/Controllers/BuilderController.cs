using FlowCiao.Builders.Serialization;
using FlowCiao.Interfaces;
using FlowCiao.Studio.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowCiao.Studio.Controllers
{
    [Route("flowciao/api/builder")]
    public class BuilderController : FlowCiaoControllerBase
    {
        private readonly IFlowBuilder _flowBuilder;
        private readonly FlowJsonSerializer _flowJsonSerializer;

        public BuilderController(IFlowBuilder flowBuilder, FlowJsonSerializer flowJsonSerializer)
        {
            _flowBuilder = flowBuilder;
            _flowJsonSerializer = flowJsonSerializer;
        }

        [HttpPost, Route("json")]
        public async Task<IActionResult> BuildFromJson(SerializationViewModel serializationViewModel)
        {
            var planner = _flowJsonSerializer.Import(serializationViewModel.Content);
            var flow = await _flowBuilder.BuildAsync(planner);
            var flowViewModel = new FlowViewModel
            {
                Key = flow.Key,
                Id = flow.Id
            };

            return Ok(flowViewModel);
        }
    }
}