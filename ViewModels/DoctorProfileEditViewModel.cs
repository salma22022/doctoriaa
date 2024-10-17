namespace Project.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class DoctorProfileEditViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Bio is required")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "License Number is required")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "Specialization Name is required")]
        public SpecializationName SpecializationName { get; set; }

        [Required(ErrorMessage = "Clinic Name is required")]
        public string ClinicName { get; set; }

        [Required(ErrorMessage = "Clinic Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Clinic Price must be greater than zero")]
        public double ClinicPrice { get; set; }

        public string LocationAddress { get; set; }

    }

}
