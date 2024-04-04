using FlowCiao.Builders.Serialization;
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
        private readonly FlowJsonSerializer _flowJsonSerializer;

        public FlowController(IFlowOperator flowOperator, FlowService flowService, FlowJsonSerializer flowJsonSerializer)
        {
            _operator = flowOperator;
            _flowService = flowService;
            _flowJsonSerializer = flowJsonSerializer;
        }
        
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var flows = await _flowService.Get();
            flows.AsParallel()
                .ForAll(f => f.SerializedJson = _flowJsonSerializer.Export(f));

            return Ok(flows);
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