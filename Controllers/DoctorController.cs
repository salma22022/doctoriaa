using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
