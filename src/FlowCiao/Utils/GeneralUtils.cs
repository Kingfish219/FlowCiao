using System;
using System.Linq;

namespace FlowCiao.Utils;

internal static class GeneralUtils
{
    public static Type FindType(string typeName, Type assignableFrom = null)
    {
        var type = assignableFrom != null ?
            FindTypeDeep(typeName, assignableFrom) :
            FindTypeDeep(typeName);
        
        return type;
    }
    
    private static Type FindTypeDeep(string typeName)
    {
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(t => t.GetTypes())
            .SingleOrDefault(p => string.Equals(p.Name, typeName, StringComparison.Ordinal));

        return type;
    }

    private static Type FindTypeDeep(string typeName, Type assignableFrom)
    {
        var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .SingleOrDefault(p => assignableFrom.IsAssignableFrom(p) && !p.IsAbstract &&
                             (string.Equals(p.Name, typeName, StringComparison.Ordinal) ||
                              string.Equals(p.FullName, typeName, StringComparison.Ordinal)));

        return type;
    }
}