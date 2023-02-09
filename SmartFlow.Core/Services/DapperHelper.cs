
using Dapper.FluentMap;
using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    internal class DapperHelper
    {
        internal static void EnsureMappings()
        {
            if (FluentMapper.EntityMaps.Count > 0)
            {
                return;
            }

            FluentMapper.Initialize(config =>
            {
                //var entityMappers = AppDomain.CurrentDomain.GetAssemblies()
                //    .SelectMany(type => type.GetTypes())
                //    .Where(p => typeof(IEntityMap).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

                //foreach (var mapper in entityMappers)
                //{
                //    var method = typeof(FluentMapConfiguration).GetMethod(nameof(FluentMapConfiguration.AddMap));
                //    var generic = method.MakeGenericMethod(mapper);
                //    var obj = Activator.CreateInstance(mapper);
                //    generic.Invoke(config, new[]{ obj });
                //}

                config.AddMap(new ActionMap());
                config.AddMap(new TransitionMap());
                config.AddMap(new StateMap());
                config.AddMap(new UserMap());
                config.AddMap(new GroupMap());
            });
        }
    }
}
