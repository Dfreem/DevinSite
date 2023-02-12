using Microsoft.AspNetCore.Identity;

namespace DevinSite.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<Student> _userManager;
    private readonly SignInManager<Student> _signinManager;
    private readonly ISiteRepository _repo;
    private readonly IConfiguration _config;


    public AccountController(UserManager<Student> userM, SignInManager<Student> signin, ISiteRepository repo, IConfiguration configuration)
    {
        _userManager = userM;
        _signinManager = signin;
        _repo = repo;
        _config = configuration;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        string? un = _signinManager.Context.User.Identity!.Name;
        var student = await _userManager.FindByNameAsync(un);
        return View(student);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterVM());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (ModelState.IsValid)
        {
            var user = (Student)model;
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
        return RedirectToAction("Index", "Home");
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

    [HttpPost]
    public async Task EditProfileInfo(Student student)
    {
        await _userManager.UpdateAsync(student);
    }
}