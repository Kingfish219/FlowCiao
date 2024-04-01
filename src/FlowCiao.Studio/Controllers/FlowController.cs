using FlowCiao.Operators;
using FlowCiao.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowCiao.Studio.Controllers
{
    [ApiController]
    [Route("flowciao/api/flows")]
    public class FlowController : ControllerBase
    {
        private readonly IFlowOperator _operator;
        private readonly FlowService _flowService;

        public FlowController(IFlowOperator flowOperator, FlowService flowService)
        {
            _operator = flowOperator;
            _flowService = flowService;
        }
        
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var state = await _flowService.Get();

            return Ok(state);
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