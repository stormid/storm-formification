using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Globalization;
using System.Linq;

namespace Storm.Formification.Core
{
    public static class DateTimeViewDataDictionaryExtensions
    {
        public static string? GetAttemptedValue(this ViewDataDictionary viewData, string datePart)
        {
            if (!viewData.ModelState.IsValid)
            {
                var name = viewData.TemplateInfo.GetFullHtmlFieldName(datePart);
                viewData.ModelState.TryGetValue(name, out var modelValue);

                return modelValue?.AttemptedValue;
            }


            var dt = viewData.Model as DateTime?;
            if (dt.HasValue && dt.Value != DateTime.MinValue && viewData.ModelMetadata.UnderlyingOrModelType == typeof(DateTime))
            {
                switch (datePart)
                {
                    case "Day":
                        return dt.Value.ToString("dd");
                    case "Month":
                        return dt.Value.ToString("MM");
                    case "Year":
                        return dt.Value.ToString("yyyy");
                }
            }
            else if (viewData.Model is string stringValue)
            {
                var dateMonthYearAttribute = (viewData.ModelMetadata as DefaultModelMetadata)?.Attributes.PropertyAttributes.OfType<Forms.DateMonthYearAttribute>().FirstOrDefault();

                if (dateMonthYearAttribute != null && DateTime.TryParseExact(stringValue, dateMonthYearAttribute.DateFormatString, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var dateValue))
                {
                    switch(datePart)
                    {
                        case "Month":
                            return dateValue.ToString(dateMonthYearAttribute.MonthFormatString);
                        case "Year":
                            return dateValue.ToString(dateMonthYearAttribute.YearFormatString);
                    }
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
