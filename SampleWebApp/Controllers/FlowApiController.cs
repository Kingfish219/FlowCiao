using FlowCiao.Operators;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebApp.Controllers
{
    [ApiController]
    [Route("api/flows")]
    public class FlowApiController : ControllerBase
    {
        private readonly ILogger<FlowApiController> _logger;
        private readonly IFlowOperator _operator;

        public FlowApiController(ILogger<FlowApiController> logger,
            IFlowOperator flowOperator)
        {
            _logger = logger;
            _operator = flowOperator;
        }

        [HttpGet]
        [Route("{flowKey}/state")]
        public async Task<IActionResult> GetFLowState([FromRoute] string flowKey)
        {
            var state = await _operator.GetFLowState(flowKey);

            return Ok(state);
        }

        [HttpPost]
        [Route("{flowKey}/fire")]
        public async Task<IActionResult> Fire([FromRoute] string flowKey, int action)
        {
            var result = await _operator.Fire(flowKey, action);

            return Ok(result);
        }
    }
}