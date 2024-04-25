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

        public FlowController(IFlowOperator flowOperator, FlowService flowService,
            FlowJsonSerializer flowJsonSerializer,
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

        [HttpPost, Route("{key}/ciao")]
        public async Task<IActionResult> Ciao([FromRoute] string key)
        {
            var result = await _operator.Ciao(key);

            return Ok(result is null
                ? new ApiResponse(ApiResponse.ApiResponseStatus.Error, null, "Could not create Flow Instance")
                : new ApiResponse(result));
        }

        [HttpPost, Route("{key}/fire")]
        public async Task<IActionResult> Fire([FromRoute] string key, int triggerCode)
        {
            var result = await _operator.CiaoAndTriggerAsync(key, triggerCode);

            return Ok(result.Status == "failed"
                ? new ApiResponse(ApiResponse.ApiResponseStatus.Error, result, result.Message)
                : new ApiResponse(result));
        }
        
        [HttpGet, Route("{key}/instances")]
        public async Task<IActionResult> GetFlowExecutions([FromRoute] string key)
        {
            var flow = await _flowService.GetByKey(key: key);
            var flowInstances = await _flowInstanceService.Get(flow.Id);

            return Ok(new ApiResponse(flowInstances));
        }
        
        [HttpGet, Route("instances/{id}")]
        public async Task<IActionResult> GetFlowExecution([FromRoute] string id)
        {
            var flowInstance = await _flowInstanceService.GetById(new Guid(id));

            return Ok(new ApiResponse(flowInstance));
        }

        [HttpPost, Route("instances/{id}/trigger")]
        public async Task<IActionResult> TriggerFlowExecution([FromRoute] string id, int triggerCode)
        {
            var flowInstance = await _flowInstanceService.GetById(new Guid(id));
            var result = await _operator.TriggerAsync(flowInstance, triggerCode);

            return Ok(result.Status == "failed"
                ? new ApiResponse(ApiResponse.ApiResponseStatus.Error, result, result.Message)
                : new ApiResponse(result));
        }
    }
}