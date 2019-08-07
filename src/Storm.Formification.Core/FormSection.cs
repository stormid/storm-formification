using System.Collections.Generic;

namespace Storm.Formification.Core
{
    public class FormSection
    {
        public FormSection(string name, IEnumerable<FormProperty> properties)
        {
            Name = name;
            Properties = properties;
        }

        public string Name { get; set; }

        public IEnumerable<FormProperty> Properties { get; set; }
    }
}
