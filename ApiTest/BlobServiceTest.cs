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

        [Fact]
        public async Task UploadFilesFail()
        {
            IFormFile fakeFormFile1 = A.Fake<IFormFile>();
            var blobResult1 = await _blobService.UploadBlobAsync("badFileName", "badContainerName", fakeFormFile1);
            Assert.False(blobResult1.IsSuccess);

            var blobResult2 = await _blobService.UploadBlobAsync(null, null, null);
            Assert.False(blobResult2.IsSuccess);

            var blobResult3 = await _blobService.UploadBlobAsync("", null, null);
            Assert.False(blobResult3.IsSuccess);

            var blobResult4 = await _blobService.UploadBlobAsync(null, "", null);
            Assert.False(blobResult4.IsSuccess);

            IFormFile fakeFormFile2 = A.Fake<IFormFile>();
            var blobResult5 = await _blobService.UploadBlobAsync(null, null, fakeFormFile2);
            Assert.False(blobResult5.IsSuccess);

            IFormFile fakeFormFile3 = A.Fake<IFormFile>();
            var blobResult6 = await _blobService.UploadBlobAsync("ss", "ss", fakeFormFile3);
            Assert.False(blobResult6.IsSuccess);

            var blobResult7 = await _blobService.UploadBlobAsync("ss", null, fakeFormFile3);
            Assert.False(blobResult7.IsSuccess);

            var blobResult8 = await _blobService.UploadBlobAsync(null, "ss", fakeFormFile3);
            Assert.False(blobResult8.IsSuccess);
        }

        [Fact]
        public async Task UploadFilesSuccess()
        {
            var workingDir = Directory.GetCurrentDirectory();
            string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            string filePath = $@"{projectDir}\BlobFiles\TestImage.png";
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


            filePath = $@"{projectDir}\BlobFiles\TestDoc.docx";
            fileBytes = File.ReadAllBytes(filePath); 
            Result<string> blobResult10;
            using (var ms = new MemoryStream(fileBytes))
            {
                trueFormFile = new FormFile(ms, 0, ms.Length, null, Path.GetFileName(filePath));
                blobResult10 = await _blobService.UploadBlobAsync(trueFormFile.FileName, "tesktask", trueFormFile);
            }
            Assert.True(blobResult10.IsSuccess);
        }
    }
}
