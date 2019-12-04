using Microsoft.Extensions.Caching.Memory;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    public class CreditCardFormActions : IFormActions<CreditCard>
    {
        private readonly IMemoryCache memoryCache;

        public CreditCardFormActions(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task<CreditCard> Retrieve(Guid id, CancellationToken cancellationToken = default)
        {
            if(memoryCache.TryGetValue(id, out var obj) && obj is CreditCard creditCardFormInstance)
            {
                return Task.FromResult(creditCardFormInstance);
            }

            return Task.FromResult(new CreditCard());
        }

        public Task<Guid> Save(Guid id, CreditCard form, CancellationToken cancellationToken = default)
        {
            memoryCache.Set(id, form);

            return Task.FromResult(id);
        }
    }

    [Info("Credit card", Id)]
    public class CreditCard
    {
        public const string Id = "69CE927B-0315-4DD0-91AB-08ED483EEE95";

        [Text]
        [Required]
        [MaxLength(25)]
        public string Issuer { get; set; }

        [Text]
        public string NameOnCard { get; set; }

        [DateMonthYear]
        [Required]
        public string? ValidFrom { get; set; }

        [DateMonthYear]
        [Required]
        public string? ValidTo { get; set; }
    }
}
