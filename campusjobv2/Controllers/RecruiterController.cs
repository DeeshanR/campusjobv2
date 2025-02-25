using Microsoft.AspNetCore.Mvc;

namespace campusjobv2.Controllers
{
    public class RecruiterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
