using System.ComponentModel.DataAnnotations;

public class DoctorRegisterViewModel
{
    [Required(ErrorMessage = "Name is required")] // name
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")] // email
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required")] // phone
    [DataType(DataType.PhoneNumber)]
    [Display(Name ="Phone Number")]
    public string PhoneNumber { get; set; }

    public string Bio { get; set; }

    [Required(ErrorMessage = "Gender is required")] // gender
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "License number  is required")] // gender
    [Display(Name ="License Number")]
    public string LicenseNumber { get; set; }

    [Required(ErrorMessage = "Specilization  is required")]
    public SpecializationName SpecializationName { get; set; }

    [Required(ErrorMessage = "Clinic name  is required")]
    [Display(Name ="Clinic Name")]
    public string ClinicName { get; set; }

    [Required(ErrorMessage = "Price  is required")]
    [Display(Name ="Clinic Price")]
    public double ClinicPrice { get; set; }

    [Required(ErrorMessage = "Clinic location  is required")]
    [Display(Name ="Clinic Location Address")]
    public string LocationAddress { get; set; }

    [Required(ErrorMessage = "Password is required")] // password
    [DataType(DataType.Password)]
    [StringLength(40, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 40 character")]
    [Compare("ConfirmPassword", ErrorMessage = "Password doesn't match")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password confirmation is required")] //confirm password
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password doesn't match")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }

}

