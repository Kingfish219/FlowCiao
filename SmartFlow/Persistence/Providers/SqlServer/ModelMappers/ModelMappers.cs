using Dapper.FluentMap.Mapping;
using SmartFlow.Models;
using SmartFlow.Models.Flow;

namespace SmartFlow.Persistence.Providers.SqlServer.ModelMappers
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

        internal class GroupMap : EntityMap<Group>
        {
            internal GroupMap()
            {
                Map(x => x.Id).ToColumn("GroupId");
            }
        }
    }
}
