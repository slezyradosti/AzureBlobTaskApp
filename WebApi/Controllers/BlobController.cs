using Application.BlobService;
using Application.Data;
using Application.Email;
using Microsoft.AspNetCore.Mvc;
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
            var file = blobFormDto.File;
            if (file != null)
            {
                if (Path.GetExtension(file.FileName) != ".docx") return BadRequest("Incorrect file type!\nFile type must be '.docx'");
                
                var blobResult = await _blobService.UploadBlobAsync(file.FileName, ContainerName, file);

                if (blobResult.IsSuccess)
                {
                    var emailResult = await _emailService.Send("oleg.sergushin11@mail.ru", "FileLink");
                    return HandleResult(emailResult);
                }
                else
                {
                    return HandleResult(blobResult);
                }
            }

            return BadRequest("File is not found");
        }
    }
}
