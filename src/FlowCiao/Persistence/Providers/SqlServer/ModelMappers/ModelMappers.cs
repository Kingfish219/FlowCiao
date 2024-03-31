using Dapper.FluentMap.Mapping;
using FlowCiao.Models;
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

        internal class ProcessExecutionMap : EntityMap<ProcessExecution>
        {
            internal ProcessExecutionMap()
            {
                Map(x => x.State.Id).ToColumn("State");
            }
        }

        internal class ProcessMap : EntityMap<Process>
        {
            internal ProcessMap()
            {
                Map(x => x.Id).ToColumn("ProcessId");
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
