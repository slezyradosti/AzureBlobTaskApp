using BlazorClientApp.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using WebApi.Models;

namespace BlazorClientApp.Data
{
    public class Blobs
    {
        public string Message { get; set; }
        public BlobFormDto blobFormDto { get; set; }

        [Inject]
        private IBlobService _blobService { get; set; }

        public void HandleFailedRequest()
        {
            Message = "Something went wrong, form not submited.";
        }

        public async void HandleValidRequest()
        {
            if (blobFormDto == null) Message = "All the fields are required!";

            var result = await _blobService.UploadBlobAsync(blobFormDto);

            //if (result == null) Message = "Something went wrong, form not submited.";
            if (result != null) Message = result;
        }

        public Blobs()//(IBlobService blobService)
        {
            Message = string.Empty;
            blobFormDto = new BlobFormDto();
            _blobService = new BlobService(new HttpClient());
        }
    }
}
