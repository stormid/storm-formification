using System.Collections.Generic;

namespace Storm.Formification.Core
{
    public class FormSection
    {
        public FormSection(string name, IEnumerable<FormProperty> properties)
        {
            Name = name;
            var props = new List<FormProperty>(properties);
            Properties = props;
        }

        public string Name { get; set; }

        public IEnumerable<FormProperty> Properties { get; set; }
    }
}
