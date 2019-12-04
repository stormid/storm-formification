using Microsoft.Extensions.Caching.Memory;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    public class DateFormActions : IFormActions<DateForm>
    {
        private readonly IMemoryCache memoryCache;

        public DateFormActions(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task<DateForm> Retrieve(Guid id, CancellationToken cancellationToken = default)
        {
            if(memoryCache.TryGetValue(id, out var obj) && obj is DateForm DateFormFormInstance)
            {
                return Task.FromResult(DateFormFormInstance);
            }

            return Task.FromResult(new DateForm());
        }

        public Task<Guid> Save(Guid id, DateForm form, CancellationToken cancellationToken = default)
        {
            memoryCache.Set(id, form);
            System.Diagnostics.Debug.WriteLine(id.ToString());

            return Task.FromResult(id);
        }
    }

    [Info("Dates", Id)]
    public class DateForm
    {
        public const string Id = "69CE927B-0315-4DD0-91AB-08ED483EEE96";

        [DateMonthYear]
        public string? OptionalMonthYearField { get; set; }

        [Date]
        public DateTime? OptionalDateField { get; set; }
    }
}
