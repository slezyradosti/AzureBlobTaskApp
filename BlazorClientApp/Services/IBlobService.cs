using WebApi.Models;

namespace BlazorClientApp.Services
{
    public interface IBlobService
    {
        public Task<HttpResponseMessage> UploadBlobAsync(BlobForm blobFormDto);
    }
}
