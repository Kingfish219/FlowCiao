using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlowCiao.Utils;

internal class FlowCiaoJsonSerializer
{
    public static string SerializeObject(object obj, int maxDepth)
    {
        using var strWriter = new StringWriter();
        using (var jsonWriter = new CustomJsonTextWriter(strWriter))
        {
            bool Include() => jsonWriter.CurrentDepth <= maxDepth;
            var resolver = new CustomContractResolver(Include);
            var serializer = new JsonSerializer {ContractResolver = resolver, ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
            serializer.Serialize(jsonWriter, obj);
        }
        
        return strWriter.ToString();
    }
    
    private class CustomJsonTextWriter : JsonTextWriter
    {
        public CustomJsonTextWriter(TextWriter textWriter) : base(textWriter) {}

        public int CurrentDepth { get; set; }

        public override void WriteStartObject()
        {
            CurrentDepth++;
            base.WriteStartObject();
        }

        public override void WriteEndObject()
        {
            CurrentDepth--;
            base.WriteEndObject();
        }
    }
    
    private class CustomContractResolver : DefaultContractResolver
    {
        private readonly Func<bool> _includeProperty;

        public CustomContractResolver(Func<bool> includeProperty)
        {
            _includeProperty = includeProperty;
        }

        protected override JsonProperty CreateProperty(
            MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var shouldSerialize = property.ShouldSerialize;
            property.ShouldSerialize = obj => _includeProperty() &&
                                              (shouldSerialize == null ||
                                               shouldSerialize(obj));
            return property;
        }
    }
}