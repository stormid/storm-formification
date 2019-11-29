using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Storm.Formification.Core.Infrastructure
{
    public class DateMonthYearModelBinder : IModelBinder
    {
        private DateTime? ParseDate(string year, string month)
        {
            if (DateTime.TryParseExact($"{year}-{month}", "yyyy-M", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var dateValue))
            {
                return dateValue;
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
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"'{bindingContext.ModelMetadata.DisplayName}' must be a valid month and year");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                bindingContext.ModelState.MarkFieldValid(monthPartModelName);
                bindingContext.ModelState.MarkFieldValid(yearPartModelName);
                bindingContext.Result = ModelBindingResult.Success(dateValue.Value.ToString("yyyy-MM"));
            }

            return Task.CompletedTask;
        }
    }
}
