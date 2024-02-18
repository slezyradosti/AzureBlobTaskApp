using BlazorClientApp.Services;
using Microsoft.AspNetCore.Components;
using WebApi.Models;

namespace BlazorClientApp.Data
{
    public class Blobs
    {
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public BlobFormDto blobFormDto { get; set; }

        [Inject]
        private IBlobService _blobService { get; set; }

        public void HandleFailedRequest()
        {
            ErrorMessage = "Something went wrong, form not submited.";
        }

        public async Task HandleValidRequestAsync()
        {
            if (blobFormDto == null || blobFormDto.File == null || blobFormDto.Email == null) ErrorMessage = "All the fields are required!";

            var result = await _blobService.UploadBlobAsync(blobFormDto);

            if (result == null) ErrorMessage = "Something went wrong, form not submited.";
            else if (result == "") SuccessMessage = "File successfully uploaded!";
            else ErrorMessage = result;
        }

        public Blobs()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            blobFormDto = new BlobFormDto();
            _blobService = new BlobService(new HttpClient());
        }
    }
}
