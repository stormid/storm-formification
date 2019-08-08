using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Storm.Formification.Core.Infrastructure
{

    public class NullableDateTimeModelBinder : IModelBinder
    {
        private DateTime? ParseDate(string year, string month, string day)
        {
            if (DateTime.TryParseExact($"{year}-{month}-{day}", "yyyy-M-d", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var dateValue))
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

        private Task PerformBindingActionAsync(ModelBindingContext bindingContext)
        {
            var dayPartModelName = $"{bindingContext.FieldName}.{nameof(DateTime.Day)}".Trim('.');
            var monthPartModelName = $"{bindingContext.FieldName}.{nameof(DateTime.Month)}".Trim('.');
            var yearPartModelName = $"{bindingContext.FieldName}.{nameof(DateTime.Year)}".Trim('.');

            var dayValueResult = bindingContext.ValueProvider.GetValue(dayPartModelName);
            var monthValueResult = bindingContext.ValueProvider.GetValue(monthPartModelName);
            var yearValueResult = bindingContext.ValueProvider.GetValue(yearPartModelName);

            if (dayValueResult == ValueProviderResult.None || monthValueResult == ValueProviderResult.None || yearValueResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            // if nullable and bind parameters are null or empty
            if (bindingContext.ModelMetadata.IsReferenceOrNullableType && string.IsNullOrWhiteSpace(dayValueResult.FirstValue) && string.IsNullOrWhiteSpace(monthValueResult.FirstValue) && string.IsNullOrWhiteSpace(yearValueResult.FirstValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var dateValue = ParseDate(yearValueResult.FirstValue, monthValueResult.FirstValue, dayValueResult.FirstValue);

            if (!dateValue.HasValue)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"'{bindingContext.ModelMetadata.DisplayName}' must be a valid date");
                bindingContext.ModelState.SetModelValue(dayPartModelName, dayValueResult);
                bindingContext.ModelState.SetModelValue(monthPartModelName, monthValueResult);
                bindingContext.ModelState.SetModelValue(yearPartModelName, yearValueResult);
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(dateValue);
            }

            return Task.CompletedTask;
        }
    }
}
