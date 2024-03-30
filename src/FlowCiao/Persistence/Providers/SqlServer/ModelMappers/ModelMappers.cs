using Dapper.FluentMap.Mapping;
using FlowCiao.Models;
using FlowCiao.Models.Flow;

namespace FlowCiao.Persistence.Providers.SqlServer.ModelMappers
{
    internal class ModelMappers
    {
        internal class ActionMap : EntityMap<ProcessAction>
        {
            internal ActionMap()
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

        internal class UserMap : EntityMap<ProcessUser>
        {
            internal UserMap()
            {
                Map(x => x.Id).ToColumn("UserId");
            }
        }
    }
}
