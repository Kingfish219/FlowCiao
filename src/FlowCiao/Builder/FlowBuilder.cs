using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Builder;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Services.Interfaces;
using FlowCiao.Utils;
using Newtonsoft.Json.Linq;

namespace FlowCiao.Builder
{
    internal class FlowBuilder : IFlowBuilder
    {
        private List<IFlowStepBuilder> StepBuilders { get; set; }
        private IFlowStepBuilder InitialStepBuilder { get; set; }
        private readonly IFlowService _flowService;
        private readonly IStateService _stateService;
        private readonly ITransitionService _transitionService;
        private readonly IFlowJsonSerializer _flowJsonSerializer;

        public FlowBuilder(IFlowService flowService, IStateService stateService, ITransitionService transitionService,
            IFlowJsonSerializer flowJsonSerializer)
        {
            StepBuilders = new List<IFlowStepBuilder>();
            _flowService = flowService;
            _stateService = stateService;
            _transitionService = transitionService;
            _flowJsonSerializer = flowJsonSerializer;
        }

        public IFlowBuilder Initial(Action<IFlowStepBuilder> action)
        {
            StepBuilders = new List<IFlowStepBuilder>();
            InitialStepBuilder = new FlowStepBuilder(_stateService, _transitionService);
            action(InitialStepBuilder);
            InitialStepBuilder.AsInitialStep();
            StepBuilders.Add(InitialStepBuilder);

            return this;
        }

        public IFlowBuilder NewStep(Action<IFlowStepBuilder> action)
        {
            var builder = new FlowStepBuilder(_stateService, _transitionService);
            action(builder);
            StepBuilders.Add(builder);

            return this;
        }

        public Flow Build(string flowKey, Action<IFlowBuilder> build)
        {
            return BuildAsync(flowKey, build).GetAwaiter().GetResult();
        }

        public async Task<Flow> BuildAsync(string flowKey, Action<IFlowBuilder> build)
        {
            try
            {
                build(this);

                if (StepBuilders.IsNullOrEmpty())
                {
                    throw new FlowCiaoException("There is noting to build. Use Initial() and NewStep() to add Steps to your desired Flow");
                }
                
                var flow = new Flow
                {
                    Key = flowKey,
                    Name = flowKey,
                    IsActive = true
                };

                var flowSteps = StepBuilders
                    .Select(stepBuilder => stepBuilder.Build(flow.Id))
                    .ToList();
                
                if (flowSteps
                    .GroupBy(f => f.For.Code)
                    .Any(g => g.Count() > 1))
                {
                    throw new FlowCiaoException("Cannot have more than 1 Step with the same starting State");
                }
                
                flow.States ??= new List<State>();
                flow.Transitions ??= new List<Transition>();
    
                foreach (var flowStep in flowSteps)
                {
                    flow.States.Add(flowStep.For);
                    if (flowStep.Allowed.IsNullOrEmpty())
                    {
                        continue;
                    }

                    flow.Transitions.AddRange(flowStep.Allowed);
                    flow.States.AddRange(flow.Transitions.Select(t => t.To));
                }
                
                flow = FixAnomalies(flow);
                
                var persist = await Persist(flow);
                if (!persist.Success)
                {
                    throw new FlowCiaoPersistencyException($"Error occurred while saving Flow: {persist.Message}");
                }

                return persist.Data;
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        private static Flow FixAnomalies(Flow flow)
        {
            flow.States = flow.States.DistinctBy(s => s.Code).ToList();
            flow.Transitions = flow.Transitions.DistinctBy(t => (t.From.Code, t.To.Code)).ToList();
            flow.Transitions.ForEach(transition =>
            {
                if (flow.InitialStates.Any(s => transition.From.Code == s.Code))
                {
                    transition.From.IsInitial = flow.InitialStates
                        .Single(s => s.Code == transition.From.Code)
                        .IsInitial;
                }
                
                if (flow.InitialStates.Any(s => transition.To.Code == s.Code))
                {
                    transition.To.IsInitial = flow.InitialStates
                        .Single(s => s.Code == transition.To.Code)
                        .IsInitial;
                }
            });
            
            return flow;
        }

        public Flow Build<T>() where T : IFlowPlanner, new()
        {
            var flowPlanner = Activator.CreateInstance<T>();
            return BuildAsync(flowPlanner.Key, builder => flowPlanner.Plan(builder)).GetAwaiter().GetResult();
        }

        public async Task<Flow> BuildAsync<T>() where T : IFlowPlanner, new()
        {
            var flowPlanner = Activator.CreateInstance<T>();
            return await BuildAsync(flowPlanner.Key, builder => flowPlanner.Plan(builder));
        }

        public Flow Build(IFlowPlanner flowPlanner)
        {
            return BuildAsync(flowPlanner.Key, builder => flowPlanner.Plan(builder)).GetAwaiter().GetResult();
        }

        public async Task<Flow> BuildAsync(IFlowPlanner flowPlanner)
        {
            return await BuildAsync(flowPlanner.Key, builder => flowPlanner.Plan(builder));
        }

        private void Rollback()
        {
            // ignored
        }
        
        public async Task<FuncResult<Flow>> Persist(Flow flow)
        {
            var existed = await _flowService.GetByKey(key: flow.Key);
            if (existed is not null)
            {
                if (JToken.DeepEquals(_flowJsonSerializer.Export(existed), _flowJsonSerializer.Export(flow)))
                {
                    return new FuncResult<Flow>(true, data: existed);
                }

                var funcResult = await _flowService.Deactivate(existed);
                if (!funcResult.Success)
                {
                    throw new FlowCiaoPersistencyException("Error in deactivating previous version of this Flow");
                }
            }

            var result = await _flowService.Modify(flow);
            if (result == default)
            {
                throw new FlowCiaoPersistencyException("Error in modifying Flow");
            }

            foreach (var stepBuilder in StepBuilders)
            {
                var persisted = await stepBuilder.Persist(flow);
                if (!persisted.Success)
                {
                    return new FuncResult<Flow>(false);
                }
            }

            return new FuncResult<Flow>(true, data: flow);
        }
    }
}