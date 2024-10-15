using static Project.Models.Booking;


namespace Project
{
    public interface IBookingRepository
    {
        public IEnumerable<Booking> GetBookingsByUserId(string userId);
        Booking GetBookingById(int bookingId);
        void UpdateBooking(Booking booking);
        void CancelBooking(int bookingId, string reason, string canceledBy);
        void MarkAsDone(int bookingId);
        void MarkAsNoShow(int bookingId);
    }
}
    