namespace DocumentViewerApp.Models
{
    /// <summary>
    /// Options for accessing Azure Blob Storage.
    /// </summary>
    public class BlobStorageOptions
    {
        public string AccountName { get; set; } = string.Empty;
        public string AccountKey { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
    }
}
