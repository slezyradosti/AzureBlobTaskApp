using Application.BlobService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BlobController : BaseApiController
    {
        private readonly IBlobService _blobService;

        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBlob(IFormFile file)
        {
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".docx") return BadRequest("Incorrect file type!\nFile type must be '.docx'");
                
                string ContainerName = "tesktask";
                var result = await _blobService.UploadBlobAsync(fileName, ContainerName, file);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
