﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        private string GetDatePartModelName(ModelBindingContext bindingContext, string datePart)
        {
            if (bindingContext.ModelName == bindingContext.ModelType.Name)
            {
                return $"{bindingContext.FieldName}.{datePart}".Trim('.');
            }

            return $"{bindingContext.ModelName}.{datePart}".Trim('.');
        }

        private Task PerformBindingActionAsync(ModelBindingContext bindingContext)
        {
            var dayPartModelName = GetDatePartModelName(bindingContext, nameof(DateTime.Day));
            var monthPartModelName = GetDatePartModelName(bindingContext, nameof(DateTime.Month));
            var yearPartModelName = GetDatePartModelName(bindingContext, nameof(DateTime.Year));

            var dayValueResult = bindingContext.ValueProvider.GetValue(dayPartModelName);
            var monthValueResult = bindingContext.ValueProvider.GetValue(monthPartModelName);
            var yearValueResult = bindingContext.ValueProvider.GetValue(yearPartModelName);

            if (dayValueResult == ValueProviderResult.None || monthValueResult == ValueProviderResult.None || yearValueResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(dayPartModelName, dayValueResult);
            bindingContext.ModelState.SetModelValue(monthPartModelName, monthValueResult);
            bindingContext.ModelState.SetModelValue(yearPartModelName, yearValueResult);

            // if nullable and bind parameters are null or empty
            if (bindingContext.ModelMetadata.IsReferenceOrNullableType && string.IsNullOrWhiteSpace(dayValueResult.FirstValue) && string.IsNullOrWhiteSpace(monthValueResult.FirstValue) && string.IsNullOrWhiteSpace(yearValueResult.FirstValue))
            {
                bindingContext.ModelState.MarkFieldValid(dayPartModelName);
                bindingContext.ModelState.MarkFieldValid(monthPartModelName);
                bindingContext.ModelState.MarkFieldValid(yearPartModelName);
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var dateValue = ParseDate(yearValueResult.FirstValue, monthValueResult.FirstValue, dayValueResult.FirstValue);

            if (!dateValue.HasValue)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"'{bindingContext.ModelMetadata.DisplayName}' must be a valid date");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                bindingContext.ModelState.MarkFieldValid(dayPartModelName);
                bindingContext.ModelState.MarkFieldValid(monthPartModelName);
                bindingContext.ModelState.MarkFieldValid(yearPartModelName);
                bindingContext.Result = ModelBindingResult.Success(dateValue);
            }

            return Task.CompletedTask;
        }
    }
}
