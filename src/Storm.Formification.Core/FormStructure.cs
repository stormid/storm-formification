using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Storm.Formification.Core
{
    public class FormLayoutDescriptor
    {
        public FormLayoutDescriptor(string name, IReadOnlyList<FormSection> sections)
        {
            Sections = sections.Count > 1 ? sections : Enumerable.Empty<FormSection>();
            Properties = sections.SelectMany(p => p.Properties);
            Name = name;
        }

        public FormLayoutDescriptor(Type formType) : this(RetrieveFormName(formType), RetrieveFormProperties(formType))
        {
        }

        public string Name { get; }

        public IEnumerable<FormSection> Sections { get; }

        public IEnumerable<FormProperty> Properties { get; }

        public bool HasSections() => Sections.Any();

        private static string RetrieveFormName(Type formType)
        {
            return formType.GetCustomAttribute<Forms.InfoAttribute>()?.Name;
        }

        private static IReadOnlyList<FormSection> RetrieveFormProperties(Type formType)
        {
            var properties = formType.GetProperties().Where(p => Attribute.IsDefined(p, typeof(DataTypeAttribute))).ToList();
            var sections = properties.GroupBy(p => p.GetCustomAttribute<Forms.SectionAttribute>()?.Name);
            return sections.Select(section => new FormSection(section.Key, section.Select(p => new FormProperty(p)))).ToList();
        }
    }
}
