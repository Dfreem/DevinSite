
namespace DevinSite.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISiteRepository _repo;

    public HomeController(ILogger<HomeController> logger, System.IServiceProvider services)
    {
        _logger = logger;
        _repo = services.GetRequiredService<ISiteRepository>();
    }

    // if navigated to by a search, deteremine if search string is date.
    public IActionResult Index(string searchString)
    {
        // retrieve all assignments in db.
        var assignments = from m in _repo.Assignments
                          select m;
        DateTime searchDate;

        // try parse search string into a Date
        bool didParse = DateTime.TryParse(searchString, out searchDate);
        if (didParse)
        {
            // if date was parsed syuccessfully -> search by due date
            assignments = assignments.Where(a => a.DueDate.Equals(searchDate));
        }
        else if (searchString is not null)
        {
            // if not date -> search by title
            assignments = assignments.Where(a => a.Title.Contains(searchString));
        }
        return View(assignments.ToList());
    }

    public IActionResult RemoveAssignment(int id)
    {
        // look for assignment in db with AssignmentId == id
        Assignment? toDelete = _repo.Assignments.First(assignment => assignment.AssignmentId.Equals(id));
        if (toDelete is not null)
        {
            _repo.DeleteAssignmnent(toDelete);
        }
        return RedirectToAction("Index", "Home");
    }

    public IActionResult EditAssignment(int id)
    {
        return View(_repo.Assignments.First(a => a.AssignmentId.Equals(id)));
    }

    [HttpPost]
    public IActionResult EditAssignment(Assignment assignment)
    {
        // if this assignment is in  the DB, get that version.
        _repo.UpdateAssignmnent(assignment);
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

