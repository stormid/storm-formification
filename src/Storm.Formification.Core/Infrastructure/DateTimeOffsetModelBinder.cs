using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Storm.Formification.Core.Infrastructure
{
    public class DateTimeOffsetModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.UnderlyingOrModelType == typeof(DateTimeOffset) || context.Metadata.UnderlyingOrModelType == typeof(DateTime))
            {
                return new DateTimeModelBinder();
            }

            return null;
        }
    }

    public class DateTimeModelBinder : IModelBinder
    {
        private DateTime? ParseDate(string year, string month, string day)
        {
            if(DateTime.TryParseExact($"{year}-{month}-{day}", "yyyy-M-d", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var dateValue))
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

            var dayPartModelName = $"{bindingContext.FieldName}.{nameof(DateTimeOffset.Day)}".Trim('.');
            var monthPartModelName = $"{bindingContext.FieldName}.{nameof(DateTimeOffset.Month)}".Trim('.');
            var yearPartModelName = $"{bindingContext.FieldName}.{nameof(DateTimeOffset.Year)}".Trim('.');

            var dayValueResult = bindingContext.ValueProvider.GetValue(dayPartModelName);
            var monthValueResult = bindingContext.ValueProvider.GetValue(monthPartModelName);
            var yearValueResult = bindingContext.ValueProvider.GetValue(yearPartModelName);

            if (bindingContext.ModelMetadata.UnderlyingOrModelType == typeof(DateTimeOffset) || bindingContext.ModelMetadata.UnderlyingOrModelType == typeof(DateTime))
            {
                if (dayValueResult == ValueProviderResult.None || monthValueResult == ValueProviderResult.None || yearValueResult == ValueProviderResult.None)
                {
                    return Task.CompletedTask;
                }

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
                    bindingContext.Result = ModelBindingResult.Success(bindingContext.ModelMetadata.UnderlyingOrModelType == typeof(DateTime) ? dateValue.Value : new DateTimeOffset(dateValue.Value));
                }
            }

            return Task.CompletedTask;
        }
    }
}
