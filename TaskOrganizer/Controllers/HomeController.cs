using Microsoft.AspNetCore.Mvc;

namespace TaskOrganizer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
