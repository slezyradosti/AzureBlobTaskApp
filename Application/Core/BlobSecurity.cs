namespace Application.Core
{
    public record BlobSecurity
    {
        public string StorageAccount { get; set; }
        public string StorageKey { get; set; }
        public string AzureBlobConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}
