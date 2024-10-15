
using Doctoriaa.Models;
using static Doctoriaa.Models.Booking;
using Microsoft.EntityFrameworkCore;


namespace Doctoriaa.Services
{
    public class BookingRepository : IBookingRepository
    {
        VeseetaDBContext context;
        public BookingRepository(VeseetaDBContext context)
        {
            this.context = context;

        }

        public void CancelBooking(int bookingId, string cancelReason, string cancelBy)
        {
            var booking = context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.Status = Booking.BookingStatus.Cancelled;
                booking.CancelReasons = cancelReason;
                booking.CanceledBy = cancelBy;
                context.SaveChanges();
            }
        }

        public IEnumerable<Booking> GetBookingsByUserId(string userId)
        {
            return context.Bookings
                           .Where(b => b.UserId == userId) // Filter by the logged-in user's ID
                           .Include(b => b.User) // Include related user data if necessary
                           .Include(b => b.Insurance) // Include insurance details if needed
                           .ToList();
        }

        public Booking GetBookingById(int bookingId)
        {
            return context.Bookings
            .Include(b => b.User)
            .Include(b => b.Insurance)
            .Include(b => b.Day)
            .SingleOrDefault(b => b.BookingId == bookingId);
        }

        public void MarkAsDone(int bookingId)
        {
            var booking = context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.Status = Booking.BookingStatus.Completed;
                context.SaveChanges();
            }
        }

        public void MarkAsNoShow(int bookingId)
        {
            var booking = context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.Status = Booking.BookingStatus.NoShow;
                context.SaveChanges();
            }
        }

        public void UpdateBooking(Booking booking)
        {
            context.Bookings.Update(booking);
            context.SaveChanges();
        }
    }
}
