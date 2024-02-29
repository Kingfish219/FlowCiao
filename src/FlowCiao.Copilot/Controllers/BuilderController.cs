using FlowCiao.Builders;
using FlowCiao.Copilot.Models;
using FlowCiao.Models.Builder.Json;
using Microsoft.AspNetCore.Mvc;

namespace FlowCiao.Copilot.Controllers
{
    [Route("flowciao/api/builder")]
    public class BuilderController : FlowCiaoApiControllerBase
    {
        private readonly ILogger<BuilderController> _logger;
        private readonly IFlowBuilder _flowBuilder;

        public BuilderController(ILogger<BuilderController> logger, IFlowBuilder flowBuilder)
        {
            _logger = logger;
            _flowBuilder = flowBuilder;
        }

        [HttpPost, Route("json")]
        public async Task<IActionResult> BuildFromJson(JsonFlow jsonFlow)
        {
            var process = await _flowBuilder.Build(jsonFlow);
            var processViewModel = new ProcessViewModel
            {
                Key = process.Key,
                Id = process.Id
            };

            return Ok(processViewModel);
        }
    }
}