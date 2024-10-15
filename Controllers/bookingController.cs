using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Doctoriaa.Models;
using Doctoriaa.Services;
using System.Security.Claims;


namespace Doctoriaa.Controllers
{
    public class BookingController : Controller
    {

        IUserRepository UserRepository;
        IBookingRepository BookingRepository;
        public BookingController(IUserRepository UserRepository, IBookingRepository BookingRepository)
        {
            this.UserRepository = UserRepository;
            this.BookingRepository = BookingRepository;
        }


        //for specific doctor 
        public IActionResult Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Fetch all bookings for the specific logged-in user
            var bookings = BookingRepository.GetBookingsByUserId(userId);

            return View(bookings); // Pass the filtered bookings to the view

        }
        public IActionResult Details(int id)
        {
            var booking = BookingRepository.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        public IActionResult ManageToday(int bookingId, string action, string cancelReason, string cancelBy)
        {
            switch (action)
            {
                case "done":
                    BookingRepository.MarkAsDone(bookingId);
                    break;
                case "cancel":
                    BookingRepository.CancelBooking(bookingId, cancelReason, cancelBy);
                    break;
                case "no-show":
                    BookingRepository.MarkAsNoShow(bookingId);
                    break;
            }

            return RedirectToAction(nameof(Index));
        }



    }
}


