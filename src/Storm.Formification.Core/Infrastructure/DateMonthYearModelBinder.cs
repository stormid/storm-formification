using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Storm.Formification.Core.Infrastructure
{
    public class DateMonthYearModelBinder : IModelBinder
    {
        private (string year, string month)? ParseDate(string year, string month)
        {
            if(int.TryParse(year, out var dateYear) && int.TryParse(month, out var dateMonth) && DateTime.TryParseExact($"{dateMonth:00}/{dateYear:00}", "MM/yy", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var dateValue))
            {
                return (dateYear.ToString("00"), dateMonth.ToString("00"));
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

            var dateFields = ParseDate(yearValueResult.FirstValue, monthValueResult.FirstValue);

            if (!dateFields.HasValue)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"'{bindingContext.ModelMetadata.DisplayName}' must be a valid month and year");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                bindingContext.ModelState.MarkFieldValid(monthPartModelName);
                bindingContext.ModelState.MarkFieldValid(yearPartModelName);
                bindingContext.Result = ModelBindingResult.Success($"{dateFields.Value.month}/{dateFields.Value.year}");
            }

            return Task.CompletedTask;
        }
    }
}
