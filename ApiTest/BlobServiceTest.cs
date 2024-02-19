//using Application.BlobService;
//using Application.Core;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using WebApi.Models;

//namespace ApiTest
//{
//    public class BlobServiceTest
//    {
//        private readonly IBlobService _blobService;

//        public BlobServiceTest()
//        {
//            var configuration = new ConfigurationBuilder()
//            .AddUserSecrets<BlobServiceTest>()
//            .Build();

//            BlobSecurity blobSecurity = new BlobSecurity();
//            configuration.GetSection("AzureBlob").Bind(blobSecurity);

//            _blobService = new BlobService(Options.Create(blobSecurity));
//        }

//        [Fact]
//        public async Task UploadFilesFail()
//        {
//            1var blobResult1 = await _blobService.UploadBlobAsync(blobFormDto.File.FileName, ContainerName, blobFormDto.File);
//            Assert.False(result1.IsSuccess);

//            var result2 = await _emailService.Send(string.Empty, string.Empty);
//            Assert.False(result2.IsSuccess);

//            var result3 = await _emailService.Send("sss", "sss");
//            Assert.False(result3.IsSuccess);
//        }

//        [Fact]
//        public async Task UploadFilesSuccess()
//        {
//            var result1 = await _emailService.Send("test@testmail.com", "testlink");
//            Assert.True(result1.IsSuccess);

//            var result2 = await _emailService.Send("t@mail.com", "link2");
//            Assert.True(result2.IsSuccess);
//        }
//    }
//}
