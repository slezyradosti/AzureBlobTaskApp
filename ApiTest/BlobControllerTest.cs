using Application.BlobService;
using Application.Core;
using Application.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebApi.Controllers;
using WebApi.Models;

namespace ApiTest
{
    public class BlobControllerTest
    {
        private readonly BlobController _blobController;

        public BlobControllerTest()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<BlobControllerTest>()
            .Build();

            SmtpSecutiry smtpSecurity = new SmtpSecutiry();
            configuration.GetSection("SmtpSecurity").Bind(smtpSecurity);

            var emailService = new EmailService(Options.Create(smtpSecurity));

            BlobSecurity blobSecurity = new BlobSecurity();
            configuration.GetSection("AzureBlob").Bind(blobSecurity);

            var blobService = new BlobService(Options.Create(blobSecurity));

            _blobController = new BlobController(blobService, emailService);
        }

        [Fact]
        public async Task PostRequestsFail()
        {

        }

        [Fact]
        public async Task PostRequestsSuccess()
        {
            var workingDir = Directory.GetCurrentDirectory();
            string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            string filePath = $@"{projectDir}\BlobFiles\TestDoc.docx";
            // Read the file into a byte array
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Create a MemoryStream from the byte array
            IFormFile trueFormFile;
            IActionResult result = null;
            using (var ms = new MemoryStream(fileBytes))
            {
                // Create an IFormFile instance using FormFile
                trueFormFile = new FormFile(ms, 0, ms.Length, null, Path.GetFileName(filePath));

                var blobFormDto = new BlobFormDto()
                {
                    Email = "test@mail.com",
                    File = trueFormFile,
                };

                result = await _blobController.AddBlob(blobFormDto);
            }
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
