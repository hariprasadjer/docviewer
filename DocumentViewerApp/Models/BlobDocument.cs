using System;

namespace DocumentViewerApp.Models
{
    /// <summary>
    /// Represents a blob stored document.
    /// </summary>
    public class BlobDocument
    {
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long SizeKb { get; set; }
    }
}
