using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Models;
using Project.ViewModels;

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
                await userManager.AddToRoleAsync(user, "Doctor");

                Location clinicLocation = new Location
                {
                    City = model.City,
                    Area = model.Area,
                    Address = model.Address,
                    GpsLoc = model.GpsLoc
                };

                Clinic clinic = new Clinic
                {
                    Name = model.ClinicName,
                    Price = model.ClinicPrice,
                    Location = clinicLocation
                };

                // تحويل الاسم إلى Enum
                if (!Enum.TryParse(model.SpecializationName, true, out SpecializationName specializationName))
                {
                    ModelState.AddModelError("SpecializationName", "Invalid specialization selected.");
                    return View(model);
                }

                // إنشاء كائن Specialization وحفظه في قاعدة البيانات
                var specialization = new Specialization
                {
                    Name = specializationName.ToString() // تحويل القيمة إلى string
                };

                // أضف التخصص إلى قاعدة البيانات
                context.Specializations.Add(specialization);
                await context.SaveChangesAsync(); // لحفظ الـ ID الخاص بالتخصص

                // إنشاء الـ Doctor
                Doctor doctor = new Doctor
                {
                    DoctorId = user.Id,
                    Bio = model.Bio,
                    LicenseNumber = model.LicenseNumber,
                    Clinic = clinic,
                    SpecializationId = specialization.SpecializationId, // تعيين الـ ID مباشرة
                    UserId = user.Id,
                    User = user
                };

                context.Add(doctor);
                await context.SaveChangesAsync();

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




    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        // Get the currently authenticated user
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch Bio and LicenseNumber of the doctor, filtering by UserId
        var doctor = await context.Doctors
            .Where(d => d.UserId == user.Id)
            .Include(d => d.Specialization)
            .Select(d => new {
                d.Bio,
                d.LicenseNumber,
                ClinicName = d.Clinic.Name,
                ClinicPrice = d.Clinic.Price,
                City = d.Clinic.Location.City,
                Area = d.Clinic.Location.Area,
                Address = d.Clinic.Location.Address,
                GpsLoc = d.Clinic.Location.GpsLoc,
                Name = d.User.Name,
                Email = d.User.Email,
                PhoneNumber = d.User.PhoneNumber,
                SpecializationName = d.Specialization.Name // هنا تأكد أن تستخدم الاسم مباشرة

            }) // Select both Bio and LicenseNumber fields
            .FirstOrDefaultAsync();

        // If doctor is not found, handle it
        if (doctor == null)
        {
            ModelState.AddModelError("", "Doctor not found.");
            return RedirectToAction("Error", "Home");
        }

        // Construct the view model with Bio and LicenseNumber
        var model = new DoctorProfileViewModel
        {
            Bio = doctor.Bio,
            LicenseNumber = doctor.LicenseNumber,
            ClinicName = doctor.ClinicName, // Pass the clinic name to the view model
            ClinicPrice = doctor.ClinicPrice,
            City = doctor.City,
            Area = doctor.Area,
            Address = doctor.Address,
            GpsLoc = doctor.GpsLoc,
            SpecializationName = doctor.SpecializationName, // أعد تعيين اسم التخصص هنا
            Name = doctor.Name,
            Email = doctor.Email,
            PhoneNumber = doctor.PhoneNumber
        };

        // Return the view with the model containing Bio and LicenseNumber
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var doctor = await context.Doctors
            .Include(d => d.Specialization)
            .Include(d => d.Clinic)
            .FirstOrDefaultAsync(d => d.UserId == user.Id);

        if (doctor == null)
        {
            ModelState.AddModelError("", "Doctor not found.");
            return RedirectToAction("Error", "Home");
        }

        var model = new DoctorProfileEditViewModel
        {
            Name = doctor.User.Name,
            Email = doctor.User.Email,
            PhoneNumber = doctor.User.PhoneNumber,
            Bio = doctor.Bio,
            Gender = doctor.User.Gender,
            LicenseNumber = doctor.LicenseNumber,
          //  SpecializationName = doctor.Specialization.Name,
            ClinicName = doctor.Clinic.Name,
            ClinicPrice = doctor.Clinic.Price
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> EditProfile(DoctorProfileEditViewModel model)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var doctor = await context.Doctors
            .Include(d => d.Specialization)
            .Include(d => d.Clinic)
            .FirstOrDefaultAsync(d => d.UserId == user.Id);

        if (doctor == null)
        {
            ModelState.AddModelError("", "Doctor not found.");
            return RedirectToAction("Error", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(model); 
        }

        
        doctor.User.Name = model.Name;
        doctor.User.Email = model.Email;
        doctor.User.PhoneNumber = model.PhoneNumber;
        doctor.Bio = model.Bio;
        doctor.User.Gender = model.Gender;
        doctor.LicenseNumber = model.LicenseNumber;

        doctor.Clinic.Name = model.ClinicName;
        doctor.Clinic.Price = model.ClinicPrice;

        
        var updateResult = await userManager.UpdateAsync(user);
        if (updateResult.Succeeded)
        {
           
            context.Doctors.Update(doctor);
            await context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Index", "Home");
        }

      
        foreach (var error in updateResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }




}