using Microsoft.AspNetCore.Mvc;
using SmartFlow.Operators;

namespace SampleWebApp.Controllers
{
    [ApiController]
    [Route("api/flows")]
    public class FlowApiController : ControllerBase
    {
        private readonly ILogger<FlowApiController> _logger;
        private readonly ISmartFlowOperator _operator;

        public FlowApiController(ILogger<FlowApiController> logger,
            ISmartFlowOperator smartFlowOperator)
        {
            _logger = logger;
            _operator = smartFlowOperator;
        }

        [HttpGet]
        [Route("{smartFlowKey}/state")]
        public async Task<IActionResult> GetFLowState([FromRoute] string smartFlowKey)
        {
            var state = await _operator.GetFLowState(smartFlowKey);

            return Ok(state);
        }

        [HttpPost]
        [Route("{smartFlowKey}/fire")]
        public async Task<IActionResult> Fire([FromRoute] string smartFlowKey, int action)
        {
            var result = await _operator.Fire(smartFlowKey, action);

            return Ok(result);
        }
    }
}