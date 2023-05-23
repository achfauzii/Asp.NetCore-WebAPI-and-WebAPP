using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class DasbhoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
