namespace Project.Models
{
    public class Specialization
    {
        //[Key]
        public int SpecializationId { get; set; } // Primary Key
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<Doctor> Doctors { get; set; }

    }
}
