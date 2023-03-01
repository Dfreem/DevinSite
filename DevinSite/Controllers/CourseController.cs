namespace DevinSite.Controllers;

[Authorize]
public class CourseController : Controller
{
    IServiceProvider _services;
    SignInManager<Student> _signInManager;
    ISiteRepository _repo;

    public CourseController(
        IServiceProvider services,
        SignInManager<Student> signInManager,
        ISiteRepository repo)
    {
        _services = services;
        _signInManager = signInManager;
        _repo = repo;
    }
    
}

