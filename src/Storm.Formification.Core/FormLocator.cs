using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Storm.Formification.Core
{
    public class FormLocator : IFormLocator
    {
        private readonly IDictionary<string, (Forms.IInfo Info, Type Type)> formTypes;

        public FormLocator(IEnumerable<Assembly> assemblies)
        {
            formTypes = assemblies?.SelectMany(a => a.GetExportedTypes().Where(t => t.GetCustomAttribute<Forms.InfoAttribute>() != null))
                            .Select(t => {
                                var info = Info(t);
                                var kv = new KeyValuePair<string, (Forms.IInfo Info, Type Type)>(info.Id, (info, t));
                                return kv;
                                })
                            .ToDictionary(k => k.Key, v => v.Value);
        }

        public IEnumerable<Type> All()
        {
            return formTypes.Values.Select(s => s.Type);
        }

        public Type Get(string id)
        {
            return Get(f => f.Id == id).FirstOrDefault();
        }

        public IEnumerable<Type> Get(Func<Forms.IInfo, bool> predicate)
        {
            return formTypes.Where(v => predicate(v.Value.Info)).Select(s => s.Value.Type).ToList();
        }

        public FormLayoutDescriptor GetFormLayoutDescriptor(Type type)
        {
            return new FormLayoutDescriptor(type);
        }

        public FormLayoutDescriptor GetFormLayoutDescriptor(string id)
        {
            var type = Get(id);
            if(type != null)
            {
                return new FormLayoutDescriptor(type);
            }

            return null;
        }

        public FormLayoutDescriptor GetFormLayoutDescriptor(Forms.IInfo formInfo)
        {
            return GetFormLayoutDescriptor(formInfo);
        }

        public Forms.IInfo Info(Type type)
        {
            return formTypes.Values.FirstOrDefault(f => f.Type == type).Info;
        }

        public Forms.IInfo Info(string id)
        {
            var type = Get(id);
            return Info(type);
        }
    }
}
