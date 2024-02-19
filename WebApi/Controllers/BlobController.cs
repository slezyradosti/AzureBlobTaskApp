using Application.BlobService;
using Application.Email;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class BlobController : BaseApiController
    {
        private readonly IBlobService _blobService;
        private readonly IEmailService _emailService;
        private const string ContainerName = "tesktask";

        public BlobController(IBlobService blobService, IEmailService emailService)
        {
            _blobService = blobService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBlob([FromForm] BlobFormDto blobFormDto)
        {
            var blobResult = await _blobService.UploadBlobAsync(blobFormDto.File.FileName, ContainerName, blobFormDto.File);
            
            if (blobResult.IsSuccess)
            {
                var emailResult = await _emailService.SendAsync(blobFormDto.Email, blobResult.Value);
                return HandleResult(emailResult);
            }
            else
            {
                return HandleResult(blobResult);
            }
        }
    }
}
