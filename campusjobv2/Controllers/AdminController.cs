using Microsoft.AspNetCore.Mvc;

namespace campusjobv2.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
