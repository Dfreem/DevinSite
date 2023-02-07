
namespace DevinSite.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //private readonly ISiteRepository _repo;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, System.IServiceProvider services)
    {
        _logger = logger;
        _context = services.GetRequiredService<ApplicationDbContext>();
    }

    // if navigated to by a search, deteremine if search string is date.
    public IActionResult Index(string searchString)
    {
        // retrieve all assignments in db.
        var assignments = from m in _context.Assignments
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
        Assignment? toDelete = _context.Assignments.First(assignment => assignment.AssignmentId.Equals(id));
        if (toDelete is not null)
        {
            // remove assignment from DbSet. discard the result.
            _ = _context.Assignments.Remove(toDelete);

            // save changes after removing
            int linesChanged = _context.SaveChanges();

            // log the number of line that changed. 
            Console.WriteLine("Lines Removed" + linesChanged);
        }
        return RedirectToAction("Index", "Home");
    }

    //public IActionResult EditAssignment(int id)
    //{
    //    return View(_context.Assignments.Find(id));
    //}

    //[HttpPost]
    //public IActionResult EditAssignment(Assignment assignment)
    //{
    //    // if this assignment is in  the DB, get that version.
    //    _context.Assignments.Update(assignment);
    //    return RedirectToAction("Index", "Home");
    //}

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

