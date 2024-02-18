using Azure;
using BlazorClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;
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
            if (blobFormDto == null || blobFormDto.File == null || blobFormDto.Email == null)
            {
                ErrorMessage = "All the fields are required!";
                return;
            }

            var response = await _blobService.UploadBlobAsync(blobFormDto);

            if (response == null) ErrorMessage = "Something went wrong, form not submited.";
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                SuccessMessage = "File successfully uploaded!";
                return;
            }
            else
            {
                StringBuilder erros = new StringBuilder();

                var body = await response.Content.ReadAsStringAsync();
                var validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(body);

                if (validationProblemDetails.Errors != null)
                {
                    foreach (var error in validationProblemDetails.Errors)
                    {
                        erros.AppendLine(error.Value.FirstOrDefault());
                    }
                }

                ErrorMessage = erros.ToString();
            }
        }

        public Blobs()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            blobFormDto = new BlobFormDto();
            _blobService = new BlobService(new HttpClient());
        }

        public Blobs(string errorMessage, string successMessage)
        {
            ErrorMessage = errorMessage;
            SuccessMessage = successMessage;
            blobFormDto = new BlobFormDto();
            _blobService = new BlobService(new HttpClient());
        }
    }
}
