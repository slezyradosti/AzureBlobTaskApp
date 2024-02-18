using System.Text;
using System.Text.Json;
using WebApi.Models;

namespace BlazorClientApp.Services
{
    public class BlobService : IBlobService
    {
        private readonly HttpClient _httpClient;

        public BlobService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> UploadBlobAsync(BlobFormDto blobFormDto)
        {
            try
            {
                var blob = new StringContent(JsonSerializer.Serialize(blobFormDto), Encoding.UTF8, "multipart/form-data");
                using var content = new MultipartFormDataContent();
                content.Add(new StringContent(blobFormDto.Email), "Email");

                // Convert IFormFile to StreamContent
                var streamContent = new StreamContent(blobFormDto.File.OpenReadStream());
                streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = blobFormDto.File.FileName
                };
                content.Add(streamContent);

                var response = await _httpClient.PostAsync("https://localhost:7153/Blob", content);

                if (response == null) return null;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
