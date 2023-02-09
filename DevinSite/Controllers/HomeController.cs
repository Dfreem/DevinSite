
using Microsoft.AspNetCore.Identity;

namespace DevinSite.Controllers;

//[Authorize(Roles ="Student")]
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IServiceProvider _services;
    private readonly ISiteRepository _repo;
    private readonly IConfiguration _config;
    private readonly SignInManager<Student> _signInManager;
    private readonly UserManager<Student> _userManager;

    public HomeController(ILogger<HomeController> logger, System.IServiceProvider services, IConfiguration configuration)
    {
        // injected dependencies. 
        _services = services;
        _config = configuration;
        _signInManager = _services.GetRequiredService<SignInManager<Student>>();
        _userManager = _services.GetRequiredService<UserManager<Student>>();_logger = logger;
        _repo = services.GetRequiredService<ISiteRepository>();
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
        _config["MoodleString"] = newMoodle;
    }

    // if navigated to by a search, deteremine if search string is date.
    public IActionResult Index(string searchString)
    {
        // retrieve all assignments in db.
        var assignments = from m in _repo.Assignments
                          select m;
        DateTime searchDate;
        var currentUser = _signInManager.Context.User.Identity!.Name;
        if (currentUser is not null) UpdateSchedule().Wait();

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

    public async Task UpdateSchedule()
    {
        string? current = _signInManager.Context.User.Identity!.Name;
        var currentUser = await _userManager.FindByNameAsync(current);
        if (DateTime.Today.Subtract(currentUser.LastUpdate).Days > 3)
        {
            string moodleString = _config["ConnectionStrings:MoodleString"];
            _repo.DeleteAssignmentRange(_repo.Assignments);
            var cal = await MoodleWare.GetCalendarAsync(_services, moodleString);
            await _repo.AddAssignmentRangeAsync(cal);
        }
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

    public IActionResult RemoveAllAssignments(List<Assignment> assignments)
    {
        _repo.DeleteAssignmentRange(assignments);

        return RedirectToAction("Index", new List<Assignment>());
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

