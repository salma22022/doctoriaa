using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;
using System.Security.Claims;


namespace Project.Controllers
{
    public class BookingController : Controller
    {

        IUserRepository UserRepository;
        IBookingRepository BookingRepository;
        IReviewRepository ReviewRepository;
        public BookingController(IUserRepository UserRepository, IBookingRepository BookingRepository,IReviewRepository reviewRepository)
        {
            this.UserRepository = UserRepository;
            this.BookingRepository = BookingRepository;
            this.ReviewRepository = reviewRepository;
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

        public IActionResult ManageToday(string Id,Review review,int bookingId, string action, string cancelReason, string cancelBy)
        {
            if (action == "done")
            {
                review.UserId = Id;
                ReviewRepository.AddReview(review);
                BookingRepository.MarkAsDone(bookingId);
            }
            else if (action == "cancel")
            {
                BookingRepository.CancelBooking(bookingId, cancelReason, cancelBy);
            }
            else if (action == "no-show")
            { 
                    BookingRepository.MarkAsNoShow(bookingId);
            }
            return RedirectToAction(nameof(Index));
        }



    }
}


