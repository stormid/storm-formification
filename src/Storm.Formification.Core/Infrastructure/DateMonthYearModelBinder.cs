using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Storm.Formification.Core.Infrastructure
{
    public class DateMonthYearModelBinder : IModelBinder
    {
        private string dateStringFormat;
        private readonly Forms.DateMonthYearAttribute.DayValueOption dayValue;

        public DateMonthYearModelBinder(string dateStringFormat) : this(dateStringFormat, Forms.DateMonthYearAttribute.DayValueOption.FirstDay)
        {
        }

        public DateMonthYearModelBinder() : this("MM/yy", Forms.DateMonthYearAttribute.DayValueOption.FirstDay)
        {
        }

        public DateMonthYearModelBinder(string dateStringFormat, Forms.DateMonthYearAttribute.DayValueOption dayValue)
        {
            if(string.IsNullOrWhiteSpace(dateStringFormat))
            {
                throw new ArgumentNullException(nameof(dateStringFormat), "A date format string must be supplied");
            }

            this.dateStringFormat = dateStringFormat;
            this.dayValue = dayValue;
        }

        private DateTime? ParseDate(string year, string month)
        {
            if (int.TryParse(year, out var dateYear) && int.TryParse(month, out var dateMonth) && DateTime.TryParseExact($"{dateMonth:00}/{dateYear:00}", "MM/yy", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var dateValue))
            {
                switch(dayValue)
                {
                    case Forms.DateMonthYearAttribute.DayValueOption.LastDay:
                        return new DateTime(dateValue.Year, dateValue.Month, DateTime.DaysInMonth(dateValue.Year, dateValue.Month));
                    default:
                        return dateValue;
                }
            }

            return null;
        }
        
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            return PerformBindingActionAsync(bindingContext);
        }

        private string GetDatePartModelName(ModelBindingContext bindingContext, string datePart)
        {
            if(bindingContext.ModelName == bindingContext.ModelType.Name)
            {
                return $"{bindingContext.FieldName}.{datePart}".Trim('.');
            }

            return $"{bindingContext.ModelName}.{datePart}".Trim('.');
        }

        private Task PerformBindingActionAsync(ModelBindingContext bindingContext)
        {
            var monthPartModelName = GetDatePartModelName(bindingContext, nameof(DateTime.Month));
            var yearPartModelName = GetDatePartModelName(bindingContext, nameof(DateTime.Year));

            var monthValueResult = bindingContext.ValueProvider.GetValue(monthPartModelName);
            var yearValueResult = bindingContext.ValueProvider.GetValue(yearPartModelName);

            if (monthValueResult == ValueProviderResult.None || yearValueResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(monthPartModelName, monthValueResult);
            bindingContext.ModelState.SetModelValue(yearPartModelName, yearValueResult);

            if (bindingContext.ModelMetadata.IsReferenceOrNullableType && string.IsNullOrWhiteSpace(monthValueResult.FirstValue) && string.IsNullOrWhiteSpace(yearValueResult.FirstValue))
            {
                bindingContext.ModelState.MarkFieldValid(monthPartModelName);
                bindingContext.ModelState.MarkFieldValid(yearPartModelName);
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var dateValue = ParseDate(yearValueResult.FirstValue, monthValueResult.FirstValue);

            if (!dateValue.HasValue)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"'{bindingContext.ModelMetadata.DisplayName}' must be a valid month and 2 digit year");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                bindingContext.ModelState.MarkFieldValid(monthPartModelName);
                bindingContext.ModelState.MarkFieldValid(yearPartModelName);
                bindingContext.Result = ModelBindingResult.Success(dateValue.Value.ToString(dateStringFormat, CultureInfo.InvariantCulture.DateTimeFormat));
            }

            return Task.CompletedTask;
        }
    }
}
