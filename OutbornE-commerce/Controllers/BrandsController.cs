using Microsoft.AspNetCore.Mvc;

namespace OutbornE_commerce.Controllers
{
    public class BrandsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
