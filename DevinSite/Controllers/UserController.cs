
using Microsoft.AspNetCore.Identity;

namespace DevinSite.Controllers
{
    public class UserController : Controller
    {
        readonly IServiceProvider _services;
        readonly SignInManager<Student> _signInManager;
        readonly UserManager<Student> _userManager;
        readonly ISiteRepository _repo;
        readonly IConfiguration _config;

        public UserController(IServiceProvider services,
                              ISiteRepository repo,
                              SignInManager<Student> signInManager,
                              UserManager<Student> userManager,
                              IConfiguration config)
        {
            _services = services;
            _signInManager = signInManager;
            _userManager = userManager;
            _repo = repo;
            _config = config;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        
    }
}

