using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Storm.Formification.Core
{
    public class DateModel : IValidatableObject
    {
        [Range(1, 31)]
        public int? Day { get; set; }

        [Range(1, 12)]
        public int? Month { get; set; }

        [Range(0, 9999)]
        public int? Year { get; set; }

        public static implicit operator DateTimeOffset?(DateModel model)
        {
            if (model != null && DateTimeOffset.TryParse($"{model.Year}/{model.Month}/{model.Day}", CultureInfo.GetCultureInfo("en-Gb").DateTimeFormat, DateTimeStyles.AdjustToUniversal, out var dateValue))
            {
                return dateValue;
            }

            return null;
        }

        public static implicit operator DateModel(DateTimeOffset? date)
        {
            if (date.HasValue)
            {
                return new DateModel
                {
                    Year = date.Value.Year,
                    Month = date.Value.Month,
                    Day = date.Value.Day
                };
            }

            return null;
        }

        public static implicit operator DateTimeOffset(DateModel model)
        {
            if (model != null && DateTimeOffset.TryParse($"{model.Year}/{model.Month}/{model.Day}", CultureInfo.GetCultureInfo("en-Gb").DateTimeFormat, DateTimeStyles.AdjustToUniversal, out var dateValue))
            {
                return dateValue;
            }

            throw new FormatException("A date value is required");
        }

        public static implicit operator DateModel(DateTimeOffset date)
        {
            return new DateModel
            {
                Year = date.Year,
                Month = date.Month,
                Day = date.Day
            };
        }

        public static implicit operator DateTime?(DateModel model)
        {
            if (model != null && DateTime.TryParse($"{model.Year}/{model.Month}/{model.Day}", CultureInfo.GetCultureInfo("en-Gb").DateTimeFormat, DateTimeStyles.AdjustToUniversal, out var dateValue))
            {
                return dateValue;
            }

            return null;
        }

        public static implicit operator DateModel(DateTime? date)
        {
            if (date.HasValue)
            {
                return new DateModel
                {
                    Year = date.Value.Year,
                    Month = date.Value.Month,
                    Day = date.Value.Day
                };
            }

            return null;
        }

        public static implicit operator DateTime(DateModel model)
        {
            if (model != null && DateTime.TryParse($"{model.Year}/{model.Month}/{model.Day}", CultureInfo.GetCultureInfo("en-Gb").DateTimeFormat, DateTimeStyles.AdjustToUniversal, out var dateValue))
            {
                return dateValue;
            }

            throw new FormatException("A date value is required");
        }

        public static implicit operator DateModel(DateTime date)
        {
            return new DateModel
            {
                Year = date.Year,
                Month = date.Month,
                Day = date.Day
            };
        }

        public bool IsNull()
        {
            return !Year.HasValue && !Month.HasValue && !Day.HasValue;
        }

        public bool IsValid()
        {
            if (Year.HasValue && Month.HasValue && Day.HasValue)
            {
                return DateTimeOffset.TryParse($"{Year}/{Month}/{Day}", CultureInfo.GetCultureInfo("en-Gb").DateTimeFormat, DateTimeStyles.AdjustToUniversal, out var dateValue);
            }

            // allows nullable by default
            return true;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsValid())
            {
                yield return new ValidationResult("The date provided can not be used");
            }
        }
    }
}
