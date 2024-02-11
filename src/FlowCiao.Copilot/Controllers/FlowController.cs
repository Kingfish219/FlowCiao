using FlowCiao.Operators;
using Microsoft.AspNetCore.Mvc;

namespace FlowCiao.Copilot.Controllers
{
    [ApiController]
    [Route("flowciao/api/flows")]
    public class FlowController : ControllerBase
    {
        private readonly ILogger<FlowController> _logger;
        private readonly IFlowOperator _operator;

        public FlowController(ILogger<FlowController> logger,
            IFlowOperator flowOperator)
        {
            _logger = logger;
            _operator = flowOperator;
        }

        [HttpGet, Route("{key}/state")]
        public async Task<IActionResult> GetFLowState([FromRoute] string key)
        {
            var state = await _operator.GetFLowState(key);

            return Ok(state);
        }

        [HttpPost, Route("{key}/fire")]
        public async Task<IActionResult> Fire([FromRoute] string key, int action)
        {
            var result = await _operator.Fire(key, action);

            return Ok(result);
        }
    }
}