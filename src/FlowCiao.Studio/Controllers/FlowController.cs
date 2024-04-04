using FlowCiao.Builder.Serialization.Serializers;
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
        private readonly FlowExecutionService _flowExecutionService;

        public FlowController(IFlowOperator flowOperator, FlowService flowService, FlowJsonSerializer flowJsonSerializer,
            FlowExecutionService flowExecutionService)
        {
            _operator = flowOperator;
            _flowService = flowService;
            _flowJsonSerializer = flowJsonSerializer;
            _flowExecutionService = flowExecutionService;
        }
        
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var flows = await _flowService.Get();
            flows.AsParallel()
                .ForAll(f => f.SerializedJson = _flowJsonSerializer.Export(f));

            return Ok(flows);
        }

        [HttpGet, Route("/executions/{id}")]
        public async Task<IActionResult> GetFlowExecution([FromRoute] string id)
        {
            var flowExecutions = await _flowExecutionService.GetById(new Guid(id));

            return Ok(flowExecutions);
        }

        [HttpGet, Route("{key}/executions")]
        public async Task<IActionResult> GetFlowExecutions([FromRoute] string key)
        {
            var flow = await _flowService.GetByKey(key: key);
            var flowExecutions = await _flowExecutionService.Get(flow.Id);

            return Ok(flowExecutions);
        }

        [HttpPost, Route("{key}/fire")]
        public async Task<IActionResult> Fire([FromRoute] string key, int action)
        {
            var result = await _operator.Fire(key, action);

            return Ok(result);
        }
    }
}