using Application.BlobService;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class BlobController : BaseApiController
    {
        private readonly IBlobService _blobService;
        private const string ContainerName = "tesktask";

        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBlob([FromForm] BlobForm blobFormDto)
        {
            var blobDto = new BlobFormDto{ Email = blobFormDto.Email, File = blobFormDto.File };
            var blobResult = await _blobService.UploadBlobAsync(blobDto, ContainerName);

            return HandleResult(blobResult);
        }
    }
}
