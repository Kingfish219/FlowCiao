using FlowCiao.Builder.Serialization.Serializers;
using FlowCiao.Operators;
using FlowCiao.Services;
using FlowCiao.Studio.Models;
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
        private readonly FlowInstanceService _flowInstanceService;

        public FlowController(IFlowOperator flowOperator, FlowService flowService, FlowJsonSerializer flowJsonSerializer,
            FlowInstanceService flowInstanceService)
        {
            _operator = flowOperator;
            _flowService = flowService;
            _flowJsonSerializer = flowJsonSerializer;
            _flowInstanceService = flowInstanceService;
        }
        
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var flows = await _flowService.Get();
            flows.AsParallel()
                .ForAll(f => f.SerializedJson = _flowJsonSerializer.Export(f));

            return Ok(new ApiResponse(flows));
        }

        [HttpGet, Route("instances/{id}")]
        public async Task<IActionResult> GetFlowExecution([FromRoute] string id)
        {
            var flowInstances = await _flowInstanceService.GetById(new Guid(id));

            return Ok(new ApiResponse(flowInstances));
        }

        [HttpGet, Route("{key}/instances")]
        public async Task<IActionResult> GetFlowExecutions([FromRoute] string key)
        {
            var flow = await _flowService.GetByKey(key: key);
            var flowInstances = await _flowInstanceService.Get(flow.Id);

            return Ok(new ApiResponse(flowInstances));
        }
        
        [HttpPost, Route("{key}/fire")]
        public async Task<IActionResult> Fire([FromRoute] string key, int triggerCode)
        {
            var result = await _operator.CiaoAndTriggerAsync(key, triggerCode);

            return Ok(new ApiResponse(result));
        }
    }
}