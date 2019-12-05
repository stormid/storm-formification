using System;
using System.Collections.Generic;

namespace Storm.Formification.Core
{
    public interface IFormLocator
    {
        IEnumerable<Type> All();

        Forms.IInfo? Info(Type type);

        FormLayoutDescriptor? GetFormLayoutDescriptor(Type type);

        Forms.IInfo? Info(string id);

        FormLayoutDescriptor? GetFormLayoutDescriptor(string id);

        Type Get(string id);

        FormLayoutDescriptor? GetFormLayoutDescriptor(Forms.IInfo formInfo);

        IEnumerable<Type> Get(Func<Forms.IInfo, bool> predicate);
    }
}
