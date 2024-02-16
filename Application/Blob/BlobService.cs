using Application.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.BlobService
{
    public class BlobService : IBlobService
    {
        private readonly BlobSecurity _blobSecurity;
        public CloudStorageAccount storageAccount;

        public BlobService(IOptions<BlobSecurity> options)
        {
            _blobSecurity = options.Value;
            storageAccount = CloudStorageAccount.Parse(options.Value.AzureBlobConnectionString);
        }

        public async Task<Result<CloudBlockBlob>> UploadBlobAsync(string BlobName, string ContainerName, IFormFile file)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(ContainerName.ToLower());
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(BlobName);

            try
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    await blockBlob.UploadFromStreamAsync(ms);
                }
                return Result<CloudBlockBlob>.Success(blockBlob);
            }
            catch (Exception e)
            {
                return Result<CloudBlockBlob>.Failure(e.Message);
            }
        }
    }
}
