using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BlobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
