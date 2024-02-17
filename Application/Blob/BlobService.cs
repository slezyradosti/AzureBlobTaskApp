using Application.Core;
using Azure.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.BlobService
{
    public class BlobService : IBlobService
    {
        private readonly BlobSecurity _blobSecurity;
        private readonly CloudStorageAccount _storageAccount;

        public BlobService(IOptions<BlobSecurity> options)
        {
            _blobSecurity = options.Value;
            _storageAccount = CloudStorageAccount.Parse(options.Value.AzureBlobConnectionString);
        }

        public async Task<Result<string>> UploadBlobAsync(string BlobName, string containerName, IFormFile file)
        {
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName.ToLower());
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(BlobName);

            try
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    await blockBlob.UploadFromStreamAsync(ms);
                }

                var sasTocken = GetBlobSASTokenByFile(BlobName, containerName);
                var blobUrl = blockBlob.StorageUri.PrimaryUri + "?" + sasTocken;

                return Result<string>.Success(blobUrl);
            }
            catch (Exception e)
            {
                return Result<string>.Failure(e.Message);
            }
        }

        public string GetBlobSASTokenByFile(string fileName, string containerName)
        {
            try
            {
                var azureStorageAccount = _blobSecurity.StorageAccount;
                var azureStorageAccessKey = _blobSecurity.StorageKey;
                Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = fileName,
                    ExpiresOn = DateTime.UtcNow.AddHours(1),
                };

                blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);
                var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount,
                    azureStorageAccessKey)).ToString();
                return sasToken;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
