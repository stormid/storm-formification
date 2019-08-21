using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Storm.Formification.Core
{
    public static class ViewDataExtensions
    {
        public const string CurrentSectionKey = "__forms__currentsection";
        public const string CurrentPropertiesKey = "__forms__currentproperties";

        public static Type GetFormType(this ViewDataDictionary viewData)
        {
            var modelType = viewData.ModelMetadata.ModelType;

            if (modelType == typeof(object))
            {
                modelType = viewData.Model.GetType();
            }

            return modelType;
        }


        public static FormLayoutDescriptor GetFormStructure(this ViewDataDictionary viewData)
        {
            var formType = GetFormType(viewData);

            //if(formType.GetCustomAttribute<Forms.NestedAttribute>() != null)
            //{
            //    var parentFormType = formType.DeclaringType;

            //    var parent

            //    return new FormLayoutDescriptor("", "", Enumerable.Empty<FormSection>().ToList());
            //}

            return FormLayoutDescriptor.Build(formType);
        }

        public static void SetCurrentFormSection(this ViewDataDictionary viewData, FormSection formSection)
        {
            viewData[CurrentSectionKey] = formSection;
        }

        public static FormSection GetCurrentFormSection(this ViewDataDictionary viewData)
        {
            if (viewData.TryGetValue(CurrentSectionKey, out var o) && o is FormSection formSection)
            {
                viewData.SetCurrentFormProperties(formSection.Properties);
                return formSection;
            }

            return null;
        }

        public static void SetCurrentFormProperties(this ViewDataDictionary viewData, IEnumerable<FormProperty> properties)
        {
            viewData[CurrentPropertiesKey] = properties;
        }

        public static IEnumerable<FormProperty> GetCurrentFormProperties(this ViewDataDictionary viewData)
        {
            if (viewData.TryGetValue(CurrentPropertiesKey, out var o) && o is IEnumerable<FormProperty> properties)
            {
                return properties;
            }

            return Enumerable.Empty<FormProperty>();
        }

        public static FormProperty GetCurrentFormProperty(this ViewDataDictionary viewData)
        {
            if (viewData.TryGetValue(CurrentPropertiesKey, out var o) && o is IEnumerable<FormProperty> properties)
            {
                return properties.FirstOrDefault(p => p.Property.Name == viewData.ModelMetadata.Name);
            }

            return null;
        }
    }
}
