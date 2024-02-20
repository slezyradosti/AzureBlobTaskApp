using Application.Core;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.BlobService
{
    public interface IBlobService
    {
        public Task<Result<string>> UploadBlobAsync(BlobFormDto blobFormDto, string containerName);
    }
}
