using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Doctor
    {
        // [Key]
        public string DoctorId { get; set; } // Primary Key
        public string Bio { get; set; }

        // Foreign Key to Specialization

        public virtual Specialization Specialization { get; set; }

        // Foreign Key to Clinic
        [ForeignKey(nameof(Clinic))]
        public int ClinicId { get; set; } // Add this line for the foreign key
        public virtual Clinic Clinic { get; set; }

        // Foreign Key to User
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        // Navigation Properties
        public virtual ICollection<Day> Days { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}
