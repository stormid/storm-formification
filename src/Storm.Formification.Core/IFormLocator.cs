using System;
using System.Collections.Generic;

namespace Storm.Formification.Core
{
    public interface IFormLocator
    {
        IEnumerable<Type> All();
        Forms.InfoAttribute Info(Type type);

        FormLayoutDescriptor GetFormLayoutDescriptor(Type type);
    }
}
