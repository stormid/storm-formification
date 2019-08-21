using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Storm.Formification.Core
{
    public static class DateTimeHtmlHelperExtensions
    {
        public static string GetAttemptedDayValue(this IHtmlHelper<DateTime?> htmlHelper, string name = "Day")
        {
            return htmlHelper.ViewData.GetAttemptedDayValue(name);

            //var dayValue = string.Empty;

            //if (!htmlHelper.ViewData.ModelState.IsValid)
            //{
            //    htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.Name(name), out var dayModelValue);

            //    dayValue = dayModelValue?.AttemptedValue;
            //}
            //else if (htmlHelper.ViewData.Model.HasValue && htmlHelper.ViewData.Model.Value != DateTime.MinValue && htmlHelper.ViewData.ModelMetadata.IsReferenceOrNullableType)
            //{
            //    dayValue = htmlHelper.ViewData.Model.Value.ToString("dd");
            //}

            //return dayValue;
        }

        public static string GetAttemptedMonthValue(this IHtmlHelper<DateTime?> htmlHelper, string name = "Month")
        {
            return htmlHelper.ViewData.GetAttemptedMonthValue(name);

            //var dayValue = string.Empty;

            //if (!htmlHelper.ViewData.ModelState.IsValid)
            //{
            //    htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.Name(name), out var dayModelValue);

            //    dayValue = dayModelValue?.AttemptedValue;
            //}
            //else if (htmlHelper.ViewData.Model.HasValue && htmlHelper.ViewData.Model.Value != DateTime.MinValue && htmlHelper.ViewData.ModelMetadata.IsReferenceOrNullableType)
            //{
            //    dayValue = htmlHelper.ViewData.Model.Value.ToString("MM");
            //}

            //return dayValue;
        }

        public static string GetAttemptedYearValue(this IHtmlHelper<DateTime?> htmlHelper, string name = "Year")
        {
            return htmlHelper.ViewData.GetAttemptedYearValue(name);

            //var dayValue = string.Empty;

            //if (!htmlHelper.ViewData.ModelState.IsValid)
            //{
            //    htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.Name(name), out var dayModelValue);

            //    dayValue = dayModelValue?.AttemptedValue;
            //}
            //else if (htmlHelper.ViewData.Model.HasValue && htmlHelper.ViewData.Model.Value != DateTime.MinValue && htmlHelper.ViewData.ModelMetadata.IsReferenceOrNullableType)
            //{
            //    dayValue = htmlHelper.ViewData.Model.Value.ToString("yyyy");
            //}

            //return dayValue;
        }
    }
}
