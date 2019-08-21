using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Storm.Formification.Core
{
    public static class DateTimeHtmlHelperExtensions
    {
        public static string GetAttemptedDayValue(this IHtmlHelper<DateTime?> htmlHelper, string name = "Day")
        {
            return htmlHelper.ViewData.GetAttemptedDayValue(name);
        }

        public static string GetAttemptedMonthValue(this IHtmlHelper<DateTime?> htmlHelper, string name = "Month")
        {
            return htmlHelper.ViewData.GetAttemptedMonthValue(name);
        }

        public static string GetAttemptedYearValue(this IHtmlHelper<DateTime?> htmlHelper, string name = "Year")
        {
            return htmlHelper.ViewData.GetAttemptedYearValue(name);
        }
    }
}
