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
            if (string.IsNullOrEmpty(BlobName))
                return Result<string>.Failure("Blob name email value cannot be null/empty");
            if (string.IsNullOrEmpty(containerName))
                return Result<string>.Failure("Container name value cannot be null/empty");
            if (file == null)
                return Result<string>.Failure("File value cannot be null");


            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName.ToLower());
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(BlobName);


            //
            blockBlob.Metadata["email"] = "oleg.sergushin11@mail.ru";
            blockBlob.Metadata["fileLink"] = blockBlob.StorageUri.PrimaryUri.ToString();
            await blockBlob.SetMetadataAsync();
            //

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
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
