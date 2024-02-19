using Application.BlobService;
using Application.Core;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ApiTest
{
    public class BlobServiceTest
    {
        private readonly IBlobService _blobService;

        public BlobServiceTest()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<BlobServiceTest>()
            .Build();

            BlobSecurity blobSecurity = new BlobSecurity();
            configuration.GetSection("AzureBlob").Bind(blobSecurity);

            _blobService = new BlobService(Options.Create(blobSecurity));
        }

        [Theory]
        [InlineData("badFileName", "badContainerName")]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData(null, null)]
        [InlineData("ss", "ss")]
        [InlineData("ss", null)]
        [InlineData(null, "ss")]
        public async Task UploadFilesFail(string BlobName, string ContainerName)
        {
            IFormFile fakeFormFile = A.Fake<IFormFile>();
            var blobResult1 = await _blobService.UploadBlobAsync(BlobName, ContainerName, fakeFormFile);
            Assert.False(blobResult1.IsSuccess);

            var blobResult2 = await _blobService.UploadBlobAsync(BlobName, ContainerName, null);
            Assert.False(blobResult2.IsSuccess);
        }

        [Theory]
        [InlineData("TestImage.png")]
        [InlineData("TestDoc.docx")]
        public async Task UploadFilesSuccess(string fileName)
        {
            var workingDir = Directory.GetCurrentDirectory();
            string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            string filePath = $@"{projectDir}\BlobFiles\{fileName}";
            // Read the file into a byte array
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Create a MemoryStream from the byte array
            IFormFile trueFormFile;
            Result<string> blobResult9;
            using (var ms = new MemoryStream(fileBytes))
            {
                // Create an IFormFile instance using FormFile
                trueFormFile = new FormFile(ms, 0, ms.Length, null, Path.GetFileName(filePath));
                blobResult9 = await _blobService.UploadBlobAsync(trueFormFile.FileName, "tesktask", trueFormFile);
            }
            Assert.True(blobResult9.IsSuccess);
        }
    }
}
