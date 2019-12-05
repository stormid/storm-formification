using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    public class MemoryCacheFormActions<TForm> : IFormActions<TForm>
        where TForm : class, new()
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheFormActions(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task<TForm?> Retrieve(Guid id, CancellationToken cancellationToken = default)
        {
            if (memoryCache.TryGetValue(id, out var obj) && obj is TForm formInstance)
            {
                return Task.FromResult<TForm?>(formInstance);
            }

            return Task.FromResult<TForm?>(new TForm());
        }

        public Task<Guid> Save(Guid id, TForm form, CancellationToken cancellationToken = default)
        {
            memoryCache.Set(id, form);

            return Task.FromResult(id);
        }
    }
}
