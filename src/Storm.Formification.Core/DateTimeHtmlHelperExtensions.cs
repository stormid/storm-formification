using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Storm.Formification.Core
{
    public static class DateTimeHtmlHelperExtensions
    {
        public static string? GetAttemptedDayValue(this IHtmlHelper<DateTime?> htmlHelper)
        {
            return htmlHelper.ViewData.GetAttemptedDayValue();
        }

        public static string? GetAttemptedMonthValue(this IHtmlHelper<DateTime?> htmlHelper)
        {
            return htmlHelper.ViewData.GetAttemptedMonthValue();
        }

        public static string? GetAttemptedYearValue(this IHtmlHelper<DateTime?> htmlHelper)
        {
            return htmlHelper.ViewData.GetAttemptedYearValue();
        }
    }
}
