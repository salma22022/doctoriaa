using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class BlockOutHoursController : Controller
    {
        private readonly VeseetaDBContext context;
        public BlockOutHoursController(VeseetaDBContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Delete(int HourId,int DayId) {
            //Console.WriteLine($"{HourId} - {DayId}");

           var hour = context.BlockOutHours.SingleOrDefault(bhour => bhour.BlockOutHoursId == HourId);

            if (hour != null) { 
                context.BlockOutHours.Remove(hour);
                context.SaveChanges();
                TempData["BHMessage"] = "Deleted Successfully";
            }

            return RedirectToAction("Details","Day", new {DayId = DayId});

        }
    }
}
