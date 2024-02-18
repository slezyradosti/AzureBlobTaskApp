using WebApi.Models;

namespace BlazorClientApp.Services
{
    public interface IBlobService
    {
        public Task<string> UploadBlobAsync(BlobFormDto blobFormDto);
    }
}
