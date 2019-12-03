using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Storm.Formification.Core
{
    public static class DateTimeViewDataDictionaryExtensions
    {
        public static string? GetAttemptedValue(this ViewDataDictionary viewData, string datePart)
        {
            var datePartRegistry = new Dictionary<string, string>()
            {
                { "Day", "dd" },
                { "Month", "MM" },
                { "Year", "yy" },
            };

            if (!viewData.ModelState.IsValid)
            {
                var name = viewData.TemplateInfo.GetFullHtmlFieldName(datePart);
                viewData.ModelState.TryGetValue(name, out var modelValue);

                return modelValue?.AttemptedValue;
            }

            var dateMonthYearAttribute = (viewData.ModelMetadata as DefaultModelMetadata)?.Attributes.PropertyAttributes.OfType<Forms.DateMonthYearAttribute>().FirstOrDefault();

            if (dateMonthYearAttribute != null && datePartRegistry.TryGetValue(datePart, out var dateFormatString))
            {
                var dt = viewData.Model as DateTime?;
                if (dt.HasValue && dt.Value != DateTime.MinValue && viewData.ModelMetadata.UnderlyingOrModelType == typeof(DateTime))
                {
                    return dt.Value.ToString(dateFormatString);
                }
                else if (viewData.Model is string stringValue && DateTime.TryParseExact(stringValue, dateMonthYearAttribute.DateStringFormat, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var dateValue))
                {
                    return dateValue.ToString(dateFormatString);
                }
            }

            return string.Empty;
        }

        public static string? GetAttemptedDayValue(this ViewDataDictionary viewData)
        {
            return GetAttemptedValue(viewData, "Day");
        }

        public static string? GetAttemptedMonthValue(this ViewDataDictionary viewData)
        {
            return GetAttemptedValue(viewData, "Month");
        }

        public static string? GetAttemptedYearValue(this ViewDataDictionary viewData)
        {
            return GetAttemptedValue(viewData, "Year");
        }
    }
}
