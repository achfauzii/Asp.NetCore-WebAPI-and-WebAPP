using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class TestCorsController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
