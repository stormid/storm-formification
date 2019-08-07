using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Storm.Formification.Core
{
    public class FormLocator : IFormLocator
    {
        private readonly IEnumerable<Type> formTypes;

        public FormLocator(IEnumerable<Assembly> assemblies)
        {
            formTypes = assemblies?.SelectMany(a => a.GetExportedTypes().Where(t => t.GetCustomAttribute<Forms.InfoAttribute>() != null))?.ToList();
        }

        public IEnumerable<Type> All()
        {
            return formTypes;
        }

        public FormLayoutDescriptor GetFormLayoutDescriptor(Type type)
        {
            return new FormLayoutDescriptor(type);
        }

        public Forms.InfoAttribute Info(Type type)
        {
            return type.GetCustomAttribute<Forms.InfoAttribute>();
        }
    }
}
