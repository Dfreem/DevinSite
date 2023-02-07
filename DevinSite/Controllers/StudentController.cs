
namespace DevinSite.Controllers;

[Authorize(Roles = "Admin")]
//[Area("Admin")]  From the book, use with scaffolding / Area.
public class StudentController : Controller
{
    private readonly UserManager<Student> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public StudentController(UserManager<Student> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        List<Student> students = new();
        foreach (var student in _userManager.Users.ToList<Student>())
        {
            student.RoleNames = await _userManager.GetRolesAsync(student);
            students.Add(student);
        }
        StudentVM model = new()
        {
            Students = students,
            Roles = _roleManager.Roles
        };
        return View(model);
    }
    //Other action methods of the Student controller[HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        Student user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                string errorMessage = "";
                foreach (IdentityError error in result.Errors)
                {
                    errorMessage += error.Description + " | ";
                }
                TempData["message"] = errorMessage;
            }
        }
        return RedirectToAction("Index");
    }

    public IActionResult Add()
    {
        return View();
    }

    public async Task<IActionResult> AddToAdmin(string id)
    {
        IdentityRole adminRole = await _roleManager.FindByNameAsync("Admin");
        if (adminRole == null)
        {
            TempData["message"] = "Admin role does not exist. " + "Click 'Create Admin Role' button to create it.";
        }
        else
        {
            Student user = await _userManager.FindByIdAsync(id); await _userManager.AddToRoleAsync(user, adminRole.Name);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromAdmin(string id)
    {
        Student user = await _userManager.FindByIdAsync(id); await _userManager.RemoveFromRoleAsync(user, "Admin"); return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRole(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id); await _roleManager.DeleteAsync(role); return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdminRole()
    {
        await _roleManager.CreateAsync(new IdentityRole("Admin")); return RedirectToAction("Index");
    }
}

