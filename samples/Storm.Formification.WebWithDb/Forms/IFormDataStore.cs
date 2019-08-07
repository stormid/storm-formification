using System.Threading.Tasks;

namespace Storm.Formification.WebWithDb.Forms
{
    public interface IFormDataStore<TForm> where TForm : class, new()
    {
        Task<FormDataStoreResult> Save(string documentId, string secretId, TForm formData);
        Task<TForm> Retrieve(string documentId, string secretId);
    }
}
