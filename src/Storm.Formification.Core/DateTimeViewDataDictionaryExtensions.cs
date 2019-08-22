using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace Storm.Formification.Core
{
    public static class DateTimeViewDataDictionaryExtensions
    {
        public static string GetAttemptedDayValue(this ViewDataDictionary viewData, string datePart = "Day")
        {
            if (!viewData.ModelState.IsValid)
            {
                var name = viewData.TemplateInfo.GetFullHtmlFieldName(datePart);
                viewData.ModelState.TryGetValue(name, out var dayModelValue);

                return dayModelValue?.AttemptedValue;
            }

            var dt = viewData.Model as DateTime?;

            if (dt.HasValue && dt.Value != DateTime.MinValue && viewData.ModelMetadata.UnderlyingOrModelType == typeof(DateTime))
            {
                return dt.Value.ToString("dd");
            }

            return string.Empty;
        }

        public static string GetAttemptedMonthValue(this ViewDataDictionary viewData, string datePart = "Month")
        {
            if (!viewData.ModelState.IsValid)
            {
                var name = viewData.TemplateInfo.GetFullHtmlFieldName(datePart);
                viewData.ModelState.TryGetValue(name, out var dayModelValue);

                return dayModelValue?.AttemptedValue;
            }

            var dt = viewData.Model as DateTime?;

            if (dt.HasValue && dt.Value != DateTime.MinValue && viewData.ModelMetadata.UnderlyingOrModelType == typeof(DateTime))
            {
                return dt.Value.ToString("MM");
            }

            return string.Empty;
        }

        public static string GetAttemptedYearValue(this ViewDataDictionary viewData, string datePart = "Year")
        {
            if (!viewData.ModelState.IsValid)
            {
                var name = viewData.TemplateInfo.GetFullHtmlFieldName(datePart);
                viewData.ModelState.TryGetValue(name, out var dayModelValue);

                return dayModelValue?.AttemptedValue;
            }

            var dt = viewData.Model as DateTime?;

            if (dt.HasValue && dt.Value != DateTime.MinValue && viewData.ModelMetadata.UnderlyingOrModelType == typeof(DateTime))
            {
                return dt.Value.ToString("yyyy");
            }

            return string.Empty;
        }
    }
}
