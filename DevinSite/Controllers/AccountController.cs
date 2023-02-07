using Microsoft.AspNetCore.Identity;

namespace DevinSite.Controllers;

public class AccountController : Controller
{
    private UserManager<Student> _userManager;
    private SignInManager<Student> _signinManager;

    public AccountController(UserManager<Student> userM, SignInManager<Student> signin)
    {
        _userManager = userM;
        _signinManager = signin;
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        // if (ModelState.IsValid)
        {
            var user = new Student { UserName = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signinManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signinManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult LogIn(string returnURL = "")
    {
        var model = new LoginVM { ReturnUrl = returnURL };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LoginVM model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                { return Redirect(model.ReturnUrl); }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        ModelState.AddModelError("", "Invalid username/password.");
        return View(model);
    }
    public ViewResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        var model = new ChangePasswordViewModel
        {
            Username = User.Identity?.Name ?? ""
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            Student user = await _userManager.FindByNameAsync(model.Username);
            var result = await _userManager.ChangePasswordAsync(user,
                model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["message"] = "Password changed successfully";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
        return View(model);
    }

}
