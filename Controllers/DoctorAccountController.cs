using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Models;

public class DoctorAccountController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    
    private readonly VeseetaDBContext context;

    public DoctorAccountController(SignInManager<User> signInManager, UserManager<User> userManager, VeseetaDBContext context)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.context=context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(DoctorLoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(model);
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(DoctorRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var specialization = new Specialization { Name = model.SpecializationName };
                Clinic clinic = new Clinic { Name = model.ClinicName };
                Doctor doctor = new Doctor
                {
                    DoctorId=user.Id,
                    Bio = model.Bio,
                    LicenseNumber = model.LicenseNumber,
                    // ClinicId = model.ClinicId, // Foreign key to Clinic
                    Clinic = clinic,
                    Specialization = specialization, // Assuming you have Specializations
                    UserId = user.Id, // Link to the user
                    User = user // Navigation property to ApplicationUser
                };
                
                context.Doctors.Add(doctor);
                context.SaveChanges();

                await userManager.AddToRoleAsync(user, "Doctor"); // assigning role to user
                return RedirectToAction("Login", "DoctorAccount");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult VerifyEmail()
    {
        return View();
    }
    public IActionResult ChangePassword()
    {
        return View();
    }
}