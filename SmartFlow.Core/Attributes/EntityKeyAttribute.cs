using System;

namespace SmartFlow.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EntityKeyAttribute : Attribute
    {
        public EntityKeyAttribute() { }
    }
}
