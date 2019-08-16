using System.Collections.Generic;
using System.Threading.Tasks;

namespace Storm.Formification.Core.Infrastructure
{
    public interface IChoiceDataSource
    {
        Task<IEnumerable<ChoiceItem>> GetAsync();
        Task<ChoiceItem> GetAsync(string value);
    }
}
