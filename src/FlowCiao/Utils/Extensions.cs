using System.Collections.Generic;
using Newtonsoft.Json;

namespace FlowCiao.Utils;

internal static class Extensions
{
    internal static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list is null || list.Count == 0;
    }
}