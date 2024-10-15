using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Models;
using Project.Services;
using Project.ViewModels;

namespace Project.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DayController : Controller
    {
        private readonly VeseetaDBContext context;
        private readonly UserManager<User> _userManager;

        IDayRepository dayRepository;
        public DayController(VeseetaDBContext context, UserManager<User> userManager, IDayRepository dayRepository)
        {
            this.context = context;
            this._userManager = userManager;
            this.dayRepository = dayRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var docId = _userManager.GetUserId(User);
            Console.WriteLine(docId);

            if (docId == null)
                return View("404");
            var doctor = context.Doctors.FirstOrDefault(d => d.UserId == docId);
            Console.WriteLine(doctor);
            var Days = context.Days
            .Where(day => day.Doc == doctor).ToList();
            //Console.WriteLine(Days[0]?.Doc.DoctorId);
            ViewBag.message = TempData["Message"];

            if (Days != null)
                return View(Days);
            return View(Days);
        }

        [HttpGet]
        public ActionResult Create() { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(DayViewModel d)
        {
            if (ModelState.IsValid)
            {
                var docId = _userManager.GetUserId(User);
           
                var doctor = context.Doctors.FirstOrDefault(d => d.UserId == docId);
               
                foreach (var day in d.Days)
                {
                    var newDay = new Day
                    {
                        DayName = day.DayName,
                        StartTime = day.StartTime,
                        EndTime = day.EndTime,
                        WaitingPeriod = day.WaitingPeriod,
                        AppointmentTime = day.AppointmentTime,
                        Doc = doctor,
                    };

                    //context.Days.Add(newDay);
                    //context.SaveChanges();
                    dayRepository.AddDay(newDay);

                    foreach (var blockhours in day.BlockOutHours)
                    {
                        var newblockhour = new BlockOutHours
                        {
                            StartTime = blockhours.StartTime,
                            EndTime = blockhours.EndTime,
                            Day = newDay,
                        };
                        context.BlockOutHours.Add(newblockhour);
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Index");
        }


        public IActionResult Delete(int DayId) {

            var day = context.Days
                .Include(day => day.BlockOutHours)
                .Include(day => day.Bookings)
                .FirstOrDefault(day => day.DayId == DayId);

            if (day != null) {
                

                if (day.BlockOutHours.Any() && day.BlockOutHours != null) {
                    context.BlockOutHours.RemoveRange(day.BlockOutHours);
                }
                if(day.Bookings.Any() && day.Bookings != null) {
                    context.Bookings.RemoveRange(day.Bookings);
                }
                
                context.Days.Remove(day);

                context.SaveChanges();
                TempData["Message"] = "Day Deleted Succesffully";
                return RedirectToAction("Index");

            }


            return RedirectToAction("Index");

        }

        public IActionResult Details(int DayId) {

            var day = context.Days
                .Include(day => day.BlockOutHours)
                .SingleOrDefault(day => day.DayId == DayId);

            if (day != null) { 
            
                var blockOutHours = day.BlockOutHours.ToList();
                ViewBag.BHMessage = TempData["BHMessage"];
                return View(blockOutHours);
            }

            return RedirectToAction("Index");
        
        }


    }
}
