using FlowCiao.Builders.Plan;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Studio.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlowCiao.Studio.Controllers
{
    [Route("flowciao/api/builder")]
    public class BuilderController : FlowCiaoControllerBase
    {
        private readonly IFlowBuilder _flowBuilder;
        private readonly FlowSerializer _flowSerializer;

        public BuilderController(IFlowBuilder flowBuilder, FlowSerializer flowSerializer)
        {
            _flowBuilder = flowBuilder;
            _flowSerializer = flowSerializer;
        }

        [HttpPost, Route("json")]
        public async Task<IActionResult> BuildFromJson(SerializedFlow serializedFlow)
        {
            var planner = _flowSerializer.ImportJson(JsonConvert.SerializeObject(serializedFlow));
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