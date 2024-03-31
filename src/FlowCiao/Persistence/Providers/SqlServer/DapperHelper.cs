using Dapper.FluentMap;
using Microsoft.IdentityModel.Tokens;
using static FlowCiao.Persistence.Providers.SqlServer.ModelMappers.ModelMappers;

namespace FlowCiao.Persistence.Providers.SqlServer
{
    internal class DapperHelper
    {
        internal static void EnsureMappings()
        {
            if (!FluentMapper.EntityMaps.IsNullOrEmpty())
            {
                return;
            }

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new TriggerMap());
                config.AddMap(new TransitionMap());
                config.AddMap(new ProcessMap());
                config.AddMap(new StateMap());
            });
        }
    }
}
