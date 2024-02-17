using Application.BlobService;
using Application.Data;
using Application.Email;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddBlob(IFormFile file)
        {
            if (file != null)
            {
                if (Path.GetExtension(file.FileName) != ".docx") return BadRequest("Incorrect file type!\nFile type must be '.docx'");
                
                var result = await _blobService.UploadBlobAsync(file.FileName, ContainerName, file);

                if (result.IsSuccess)
                {
                    await _emailService.Send("oleg.sergushin11@mail.ru", "FileLink");
                    return Ok();
                }
                return BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
