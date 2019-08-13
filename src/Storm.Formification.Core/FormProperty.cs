using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Storm.Formification.Core
{

    public class FormProperty
    {
        public FormProperty(PropertyInfo property)
        {
            Property = property;
            DataType = property.GetCustomAttribute<DataTypeAttribute>()?.CustomDataType ?? "Forms__Text";
        }

        public PropertyInfo Property { get; set; }

        public string DataType { get; }

        public IDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
}
