using Microsoft.AspNetCore.Mvc;

namespace CampusJobs.Controllers
{
    public class HomepageController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Homepage";
            return View();
        }
    }
}