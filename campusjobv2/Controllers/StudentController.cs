using Microsoft.AspNetCore.Mvc;

namespace campusjobv2.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
