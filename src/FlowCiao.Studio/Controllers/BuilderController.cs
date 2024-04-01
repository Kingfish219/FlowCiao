using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Studio.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowCiao.Studio.Controllers
{
    [Route("flowciao/api/builder")]
    public class BuilderController : FlowCiaoControllerBase
    {
        private readonly IFlowBuilder _flowBuilder;

        public BuilderController(IFlowBuilder flowBuilder)
        {
            _flowBuilder = flowBuilder;
        }

        [HttpPost, Route("json")]
        public async Task<IActionResult> BuildFromJson(JsonFlow jsonFlow)
        {
            var process = await _flowBuilder.BuildFromJsonAsync(jsonFlow.ToString());
            var flowViewModel = new FlowViewModel
            {
                Key = process.Key,
                Id = process.Id
            };

            return Ok(flowViewModel);
        }
    }
}