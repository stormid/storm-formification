namespace Storm.Formification.Core.Infrastructure
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class AdditionalMetadataAttribute : Attribute
    {
        public AdditionalMetadataAttribute(string name, object value)
        {
            Name = name ?? throw new ArgumentNullException("name");
            Value = value;
        }

        public string Name { get; private set; }

        public object Value { get; private set; }
    }
}
