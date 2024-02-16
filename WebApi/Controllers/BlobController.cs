using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BlobController : BaseApiController
    {
        [HttpPost]
        public IActionResult AddBlob()
        {
            return View();
        }
    }
}
