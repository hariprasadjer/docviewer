using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using DocumentViewerApp.Models;

namespace DocumentViewerApp.Services
{
    /// <summary>
    /// Service for interacting with Azure Blob Storage.
    /// </summary>
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(BlobStorageOptions options)
        {
            var credential = new Azure.Storage.StorageSharedKeyCredential(options.AccountName, options.AccountKey);
            var uri = new Uri($"https://{options.AccountName}.blob.core.windows.net/{options.ContainerName}");
            _containerClient = new BlobContainerClient(uri, credential);
        }

        /// <summary>
        /// Gets a list of blobs in the configured container.
        /// </summary>
        public async Task<List<BlobDocument>> ListDocumentsAsync()
        {
            var results = new List<BlobDocument>();
            await foreach (var blob in _containerClient.GetBlobsAsync())
            {
                results.Add(new BlobDocument
                {
                    Name = blob.Name,
                    Extension = Path.GetExtension(blob.Name),
                    SizeKb = blob.Properties.ContentLength.GetValueOrDefault() / 1024
                });
            }
            return results;
        }

        /// <summary>
        /// Generates a SAS URI for the specified blob that expires in one minute.
        /// </summary>
        public Uri GenerateSasUri(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            return blobClient.GenerateSasUri(
                BlobSasPermissions.Read,
                DateTimeOffset.UtcNow.AddMinutes(1));
        }
    }
}
