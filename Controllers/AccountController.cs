using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Models;

public class AccountController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);

            if(result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("","Email or password is incorrect");
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
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Db = model.Birthdate,
                Gender = model.Gender,
                // Type = "PATIENT"
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user,"User"); // assigning role to user
                return RedirectToAction("Login", "Account");
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
        return RedirectToAction("Index","Home");
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