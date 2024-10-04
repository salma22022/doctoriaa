namespace Project.Models
{
    public class User
    {
        // [Key]
        public int UserId { get; set; } // Primary Key
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; } // e.g., Doctor, Patient
        public string Gender { get; set; }
        public string Picture { get; set; }
        public string Db { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public Doctor Doc { get; set; }

    }
}
