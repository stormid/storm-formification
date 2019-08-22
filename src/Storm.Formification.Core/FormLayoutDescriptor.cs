using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Storm.Formification.Core
{
    public class FormLayoutDescriptor
    {
        public FormLayoutDescriptor(string id, string name, IReadOnlyList<FormSection> sections)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Sections = sections.All(s => !string.IsNullOrWhiteSpace(s.Name)) ? sections.ToList() : Enumerable.Empty<FormSection>();
            Properties = sections.SelectMany(p => p.Properties);
            Id = id;
            Name = name;
        }

        public string Id { get; }

        public string Name { get; }

        public IEnumerable<FormSection> Sections { get; }

        public IEnumerable<FormProperty> Properties { get; }

        public bool HasSections() => Sections.Any();

        public static FormLayoutDescriptor Build(Type type)
        {
            var info = RetrieveFormInfo(type);
            if (info != null)
            {
                return new FormLayoutDescriptor(info.Id, info.Name, RetrieveFormSections(type));
            }

            return null;
        }

        private static Forms.IInfo RetrieveFormInfo(Type formType)
        {
            var formInfo = formType.GetCustomAttribute<Forms.InfoAttribute>() as Forms.IInfo ?? (formType.IsNestedPublic && formType.DeclaringType.GetCustomAttribute<Forms.InfoAttribute>() is Forms.IInfo fi ? fi : null);
            return formInfo;
        }

        private static IReadOnlyList<FormSection> RetrieveFormSections(Type formType)
        {
            var properties = formType.GetProperties().Where(p => Attribute.IsDefined(p, typeof(DataTypeAttribute))).ToList();
            var sections = properties.GroupBy(p => p.GetCustomAttribute<Forms.SectionAttribute>()?.Name).ToList();

            var formSections = sections?.Select(section =>
            {
                var formSection = new FormSection(section.Key, section.Select(p =>
                {
                    var formProperty = new FormProperty(p);
                    var triggerAttribute = p.GetCustomAttributes().OfType<Forms.IAmConditionalTriggerAware>()?.FirstOrDefault();

                    if (triggerAttribute != null && !string.IsNullOrWhiteSpace(triggerAttribute.ConditionalTrigger))
                    {
                        formProperty.SetConditionalTrigger(triggerAttribute.ConditionalTrigger);
                    }

                    var triggerTargetAttribute = p.GetCustomAttribute<Forms.ConditionalTargetAttribute>();

                    if (triggerTargetAttribute != null)
                    {
                        formProperty.SetConditionalTriggerTarget(triggerTargetAttribute.TriggerKey);
                    }

                    return formProperty;
                }));

                return formSection;
            });

            return formSections.ToList() ?? Enumerable.Empty<FormSection>().ToList();
        }
    }
}
