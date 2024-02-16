using Application.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.BlobService
{
    public interface IBlobService
    {
        public Task<Result<CloudBlockBlob>> UploadBlobAsync(string BlobName, string ContainerName, IFormFile file);
    }
}
