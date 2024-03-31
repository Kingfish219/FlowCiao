using Dapper.FluentMap.Mapping;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Persistence.Providers.SqlServer.ModelMappers
{
    internal class ModelMappers
    {
        internal class TriggerMap : EntityMap<Trigger>
        {
            internal TriggerMap()
            {
                Map(x => x.Id).ToColumn("ActionId");
            }
        }

        internal class TransitionMap : EntityMap<Transition>
        {
            internal TransitionMap()
            {
                Map(x => x.Id).ToColumn("TransitionId");
            }
        }

        internal class FlowExecutionMap : EntityMap<FlowExecution>
        {
            internal FlowExecutionMap()
            {
                Map(x => x.State.Id).ToColumn("State");
            }
        }

        internal class ProcessMap : EntityMap<Flow>
        {
            internal ProcessMap()
            {
                Map(x => x.Id).ToColumn("FlowId");
            }
        }

        internal class StateMap : EntityMap<State>
        {
            internal StateMap()
            {
                Map(x => x.Id).ToColumn("StateId");
            }
        }
    }
}
