using Microsoft.Extensions.Caching.Memory;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    [Info("Dates", Id)]
    [Handler(typeof(MemoryCacheFormActions<DateForm>))]
    public class DateForm
    {
        public const string Id = "69CE927B-0315-4DD0-91AB-08ED483EEE96";

        [DateMonthYear]
        public string? OptionalMonthYearField { get; set; }

        [Date]
        public DateTime? OptionalDateField { get; set; }
    }
}
