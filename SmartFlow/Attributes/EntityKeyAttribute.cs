using System;

namespace SmartFlow.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EntityKeyAttribute : Attribute
    {
        public EntityKeyAttribute() { }
    }
}
