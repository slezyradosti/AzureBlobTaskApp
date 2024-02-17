namespace Application.Core
{
    public record BlobSecurity
    {
        public string AzureBlobConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}
