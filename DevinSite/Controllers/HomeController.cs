﻿
namespace DevinSite.Controllers;

[Authorize(Roles = "student, admin")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //DDBContext _repo { get; set; }
    private MoodleWare _moodle;
    private readonly ISiteRepository _repo;

    public HomeController(ILogger<HomeController> logger, System.IServiceProvider services)
    {
        // injected dependencies. 
        _logger = logger;
        _repo = services.GetRequiredService<ISiteRepository>();
        _moodle = services.GetRequiredService<MoodleWare>();
        _moodle.GetCalendarAsync();
    }

    /// <summary>
    /// The instructions for retrieving a Moodle Calendar String are as follows:
    /// 1. Login to your Moodle student account
    /// 2. Navigate to the Calendar page.
    /// 3. Make sure to be on the Day or Month view, You should see a button the says "Export Calendar"
    /// 4. select eiter this week or next week as the time frame, and all events.
    /// 5. push "Get Calendar URL"
    /// 6. A URL is generated and diswplayed at the bottom of the screen, copy that and paste as the parameter to this method.
    /// </summary>
    /// <param name="newMoodle">The new moodle calendar connection string.</param>
    public void SetMoodleString(string newMoodle)
    {
        _moodle.MoodleString = newMoodle;
    }

    // if navigated to by a search, deteremine if search string is date.
    public IActionResult Index(string searchString)
    {
        // retrieve all assignments in db.
        var assignments = from m in _repo.Assignments.Include(a => a.Course)
                          select m;
        DateTime searchDate;

        // try parse search string into a Date
        bool didParse = DateTime.TryParse(searchString, out searchDate);
        if (didParse)
        {
            // if date -> search by due date
            assignments = assignments.Where(a => a.DueDate.Equals(searchDate));
        }
        else if (searchString is not null)
        {
            // if not date -> search by title
            assignments = assignments.Where(a => a.Title.Contains(searchString));
        }
        return View(assignments.ToList());
    }

    public IActionResult AddNewAssignment()
    {
        Assignment assignment = new();
        return View(assignment);
    }

    [HttpPost]
    public IActionResult AddNewAssignment(Assignment assignment)
    {
        // add assignment to Assignments table.
        _repo.AddAssignment(assignment);
        return RedirectToAction("Index");
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

