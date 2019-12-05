using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Storm.Formification.WebWithDb.Forms
{
    public class AzureStorageFormDataStore<TForm> : IFormDataStore<TForm> where TForm : class, new()
    {
        private readonly CloudBlobClient blobClient;

        public AzureStorageFormDataStore(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("AzureStorageConnectionString");
            var cloudStorageAccount = string.IsNullOrWhiteSpace(connectionString) ? CloudStorageAccount.DevelopmentStorageAccount : CloudStorageAccount.Parse(connectionString);

            blobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task<TForm?> Retrieve(string documentId, string secretId)
        {
            var container = blobClient.GetContainerReference(typeof(TForm).Name.ToLowerInvariant());
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(documentId);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                if (blob.Metadata.TryGetValue("secretId", out var sid) && sid == secretId)
                {
                    if (await blob.ExistsAsync())
                    {
                        var jsonData = await blob.DownloadTextAsync();
                        return JsonConvert.DeserializeObject<TForm>(jsonData);
                    }
                }
            }

            return null;
        }

        public async Task<FormDataStoreResult> Save(string documentId, string secretId, TForm formData)
        {
            var container = blobClient.GetContainerReference(typeof(TForm).Name.ToLowerInvariant());
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(documentId);

            var jsonData = JsonConvert.SerializeObject(formData, Formatting.None);
            await blob.UploadTextAsync(jsonData);

            blob.Metadata.Add("secretId", secretId);
            await blob.SetMetadataAsync();

            return new FormDataStoreResult { DocumentId = documentId, SecretId = secretId };
        }
    }
}
