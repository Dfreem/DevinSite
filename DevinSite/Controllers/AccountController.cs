using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Identity;

namespace DevinSite.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<Student> _userManager;
    private readonly SignInManager<Student> _signinManager;
    private readonly ISiteRepository _repo;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;
    readonly INotyfService _toast;

    public AccountController(IServiceProvider services, ISiteRepository repo, IConfiguration configuration, INotyfService notyf)
    {
        _userManager = services.GetRequiredService<UserManager<Student>>();
        _signinManager = services.GetRequiredService<SignInManager<Student>>();
        _repo = repo;
        _config = configuration;
        _services = services;
        _toast = notyf;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        // Retrieve the name of the currently signed in user
        string? un = _signinManager.Context.User.Identity!.Name;

        // get signed in user from UserManager
        var student = await _userManager.FindByNameAsync(un!);
        UserProfileVM userProfile = new(student!);
        return View(userProfile);
    }

    [HttpGet]
    public IActionResult RegisterAsync()
    {
        return View(new RegisterVM());
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = (Student)model;
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signinManager.SignInAsync(user, isPersistent: false);
            _toast.Success("Successfully Registered new user " + user.UserName);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            _toast.Error("There was an Error\n" + result.Errors.ToString());
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogOutAsync()
    {
        await _signinManager.SignOutAsync();
        _toast.Success("You are now signed out, Goodbye!");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult LogInAsync(string returnUrl = "")
    {
        var model = new LoginVM { ReturnUrl = returnUrl };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogInAsync(LoginVM model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _toast.Success("Successfully Logged in as " + model.UserName);
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                { return Redirect(model.ReturnUrl); }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            _toast.Error("Unable to Sign in\n" + result.ToString());
        }
        ModelState.AddModelError("", "Invalid username/password.");
        return View(model);
    }

    [HttpPost]
    public async Task EditProfileInfoAsync(Student student)
    {
        await _userManager.UpdateAsync(student);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordAsync(UserProfileVM uvm)
    {
        // Check to see that the same Password was entered twice.
        if (!uvm.NewPassword.Equals(uvm.ConfirmPassword)) return RedirectToAction("Index");
        // get userName that is currently signed in
        string? currentUserName = _signinManager.Context.User.Identity!.Name;
        var currentUser = await _userManager.FindByNameAsync(currentUserName!);

        //UserManager checks the current password first before changing to the new one.
        var result = await _userManager.ChangePasswordAsync(currentUser!, uvm.Password, uvm.NewPassword);
        if (result.Succeeded)
        {
            _toast.Success("Your password has successfully been changed.");
            return RedirectToAction("Index", "Home");
        }
        _toast.Error("unsuccessful" + result.Errors);
        return RedirectToAction("Index");
    }

    /// <summary>
    /// The instructions for retrieving a Moodle Calendar String are as follows:<br />
    /// 1. Login to your Moodle student account<br />
    /// 2. Navigate to the Calendar page.<br />
    /// 3. Make sure to be on the Day or Month view, You should see a button the says "Export Calendar"<br />
    /// 4. select eiter this week or next week as the time frame, and all events.<br />
    /// 5. push "Get Calendar URL"<br />
    /// 6. A URL is generated and displayed at the bottom of the screen, copy that and paste as the parameter to this method.<br />
    /// </summary>
    /// <param name = "UserProfileVM" > the viewmodel used in the UserProfile view</param>
    [HttpPost]
    public async Task<IActionResult> SetMoodleString(UserProfileVM uvm)
    {

        // Remove the common parts of the URL.
        //int theSpot = uvm.NewMoodle.IndexOf("preset_what");
        //string newMoodle = uvm.NewMoodle[..theSpot];

        //// The important information all end with a & symbol
        //string[] parts = newMoodle.Split('&')[0..2];
        //foreach (string item in parts)
        //{
        //    // the user data in the url are labeled and then dilimited with a = symbol
        //    // like this authtoken=23871y4691238479187
        //    string[] data = item.Split('=');

        //    // the environment variables we want are labeled with these in appsettings
        //    string selector = data[0].Contains("userid") ? "Moodle:UID" : "Moodle:Token";
        //    _config[selector] += data[1];
        //}

        // for now, just store the entire moodle string for a user in the database.
        string userName = _signinManager.Context.User.Identity!.Name!;
        var toUpdate = await _userManager.FindByNameAsync(userName);
        toUpdate!.MoodleString = uvm.NewMoodle;
        toUpdate!.MoodleIsSet = true;
        var result = await _userManager.UpdateAsync(toUpdate!);

        if (result.Succeeded) _toast.Success("successfully set your connection to Moodle");

        else { _toast.Warning("moodle connection was not set." + result.Errors.ToString()); }

        return RedirectToAction(nameof(Index));
    }

}